using ReservationSystemApi.Objects;
using ReservationSystemBusinessLogic.ApiClients;
using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Enums;
using ReservationSystemBusinessLogic.Log;
using ReservationSystemBusinessLogic.Objects;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Services
{
    public class RoomManipulationService
    {
        private Room ConvertRoomFromPayload(CreateRoomPayload payload, long? userId)
        {
            return new Room
            {
                OwnerId = userId,
                Size = payload.Size ?? 0,
                MaxNumberOfPeople = payload.MaxNumberOfPeople ?? 0,
                Country = payload.Country,
                City = payload.City,
                Street = payload.Street,
                BuildingNumber = payload.BuildingNumber,
                IsActive = payload.IsActive
            };
        }

        public RoomResponse ConvertRoomToResponse(Room room)
        {
            WorkingHoursManipulationService workingHoursManipulationService = new WorkingHoursManipulationService();
            return new RoomResponse
            {
                Id = room.Id,
                Size = room.Size,
                MaxNumberOfPeople = room.MaxNumberOfPeople,
                Country = room.Country,
                City = room.City,
                Street = room.Street,
                BuildingNumber = room.BuildingNumber,
                Lat = room.Lat,
                Lon = room.Lon,
                IsActive = room.IsActive,
                WorkingHours = room.WorkingHours?.Select(workingHoursManipulationService.ConvertWorkingHoursToPayload).ToList()
            };
        }

        public async Task<GenericObjectResponse<RoomResponse>> AddRoom(CreateRoomPayload payload, long? userId)
        {
            try
            {
                Logger.Debug($"Attempting to create new room for user {userId}");
                RoomResponse roomResponse = null;
                RoomValidationService service = new RoomValidationService();
                GenericStatusMessage validationResult = service.ValidateRoomData(payload);
                if (!validationResult.Success)
                {
                    return new GenericObjectResponse<RoomResponse>(validationResult.Message);
                }

                using (ReservationDataContext context = new ReservationDataContext())
                {
                    Room room = ConvertRoomFromPayload(payload, userId);
                    context.Rooms.Add(room);
                    context.SaveChanges();
                    roomResponse = ConvertRoomToResponse(room);
                }

                Logger.Debug($"Attempting to create working hours for room {roomResponse.Id}");
                WorkingHoursManipulationService workingHoursService = new WorkingHoursManipulationService();
                GenericObjectResponse<List<WorkingHoursPayload>> workingHours = workingHoursService.ChangeWorkingHoursForRoom(roomResponse.Id, payload.WorkingHours);
                if (!workingHours.Status.Success)
                {
                    Logger.Error($"failed to create working hours for room {roomResponse.Id}, Deleting created room.");
                    using (ReservationDataContext context = new ReservationDataContext())
                    {
                        Room room = context.Rooms.Single(x => x.Id == roomResponse.Id);
                        context.Rooms.Remove(room);
                        context.SaveChanges();
                    }
                    return new GenericObjectResponse<RoomResponse>($"Failed to add room due to faulty working hours: {workingHours.Status.Message}");
                }
                roomResponse.WorkingHours = workingHours.Object;
                Logger.Debug($"Room created for user {userId}.");
                LatLon latLon = await AddLatLonForRoom(roomResponse.Id);
                if (latLon != null)
                {
                    roomResponse.Lat = latLon.Lat;
                    roomResponse.Lon = latLon.Lon;
                }
                return new GenericObjectResponse<RoomResponse>(roomResponse);
            }
            catch (DbEntityValidationException e)
            {
                string exceptionMessage = e.EntityValidationErrors.FirstOrDefault()?.ValidationErrors.FirstOrDefault()?.ErrorMessage;
                Logger.Error($"Failed to create room. Error: '{exceptionMessage}'");
                return new GenericObjectResponse<RoomResponse>("Failed to add the room, please contact support.");
            }
        }

        public async Task<GenericObjectResponse<RoomResponse>> ChangeRoomData(CreateRoomPayload payload, long roomId)
        {
            RoomValidationService service = new RoomValidationService();
            GenericStatusMessage validationResponse = service.ValidateRoomData(payload, false);
            RoomResponse roomResponse = null;
            if (validationResponse.Success)
            {
                using (ReservationDataContext context = new ReservationDataContext())
                {
                    Room room = context.Rooms.Include(x => x.WorkingHours).Single(x => x.Id == roomId);
                    room.Size = payload.Size ?? room.Size;
                    room.MaxNumberOfPeople = payload.MaxNumberOfPeople ?? room.MaxNumberOfPeople;
                    room.Country = payload.Country ?? room.Country;
                    room.City = payload.City ?? room.City;
                    room.Street = payload.Street ?? room.Street;
                    room.BuildingNumber = payload.BuildingNumber ?? room.BuildingNumber;
                    context.SaveChanges();
                    roomResponse = ConvertRoomToResponse(room);
                }

                LatLon latLon = await AddLatLonForRoom(roomResponse.Id);
                if (latLon != null)
                {
                    roomResponse.Lat = latLon.Lat;
                    roomResponse.Lon = latLon.Lon;
                }

                return new GenericObjectResponse<RoomResponse>(roomResponse);
            }
            else
            {
                return new GenericObjectResponse<RoomResponse>(validationResponse.Message);
            }
        }

        private async Task<LatLon> AddLatLonForRoom(long roomId)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                Room room = context.Rooms.Single(x => x.Id == roomId);
                IGeolocationClient geolocationClient = new MapQuestClient();
                try
                {
                    LatLon latLon = await geolocationClient.ForwardGeolocation(room.Country, room.City, room.Street, room.BuildingNumber);
                    room.Lat = latLon.Lat;
                    room.Lon = latLon.Lon;
                    context.SaveChanges();
                    return latLon;
                }
                catch (Exception e)
                {
                    Logger.Error($"Failed to extract lat lon information for {roomId}: {e.Message}");
                    return null;
                }
            }
        }

        public GenericStatusMessage ChangeRoomActivation(long roomId, bool activate, bool force)
        {
            DateTime now = DateTime.Now;
            using (ReservationDataContext context = new ReservationDataContext())
            {
                Room room = context.Rooms.Include(x => x.Reservations).Single(x => x.Id == roomId);
                if (!activate)
                {
                    bool hasFutureReservations = room.Reservations.Any(x => x.RentStart >= now && x.Status != ReservationStatus.Rejected);
                    if (hasFutureReservations)
                    {
                        if (!force)
                        {
                            return new GenericStatusMessage(false, "Could not complete operation, room has future reservations.");
                        }

                        IEnumerable<ReservationRequest> futureReservations = room.Reservations.Where(x => x.RentStart >= now && x.Status != ReservationStatus.Rejected);
                        foreach (ReservationRequest reservation in futureReservations)
                        {
                            reservation.Status = ReservationStatus.Rejected;
                        }
                    }
                }

                room.IsActive = activate;
                context.SaveChanges();
                return new GenericStatusMessage(true);
            }
        }
    }
}
