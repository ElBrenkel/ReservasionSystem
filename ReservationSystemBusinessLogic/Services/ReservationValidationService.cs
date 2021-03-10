using ReservationSystemBusinessLogic.Common;
using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Objects;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ReservationSystemBusinessLogic.Enums;

namespace ReservationSystemBusinessLogic.Services
{
    public class ReservationValidationService
    {
        public GenericStatusMessage ValidateRoomAvailability(long roomId, ReservationRequestPayload payload)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                Room room = context.Rooms.Include(x => x.WorkingHours).SingleOrDefault(x => x.Id == roomId && x.IsActive);
                return ValidateRoomAvailability(context, room, payload);
            }
        }

        public GenericStatusMessage ValidateRoomAvailability(ReservationDataContext context, Room room, ReservationRequestPayload payload)
        {
            if (room == null)
            {
                return new GenericStatusMessage(false, "Room not found or is not currently active.");
            }

            if (payload.RentStart.Date != payload.RentEnd.Date)
            {
                return new GenericStatusMessage(false, "Rent start and rent end should be on the same day.");
            }

            if (payload.RentStart >= payload.RentEnd)
            {
                return new GenericStatusMessage(false, "Rent start can't be after rent end.");
            }

            if (GetMatchingWorkingHours(payload, room) == null)
            {
                return new GenericStatusMessage(false, "Reservation is not in the rooms working hours.");
            }

            bool collision = context.ReservationRequests.Any(x => x.RoomId == room.Id
                && x.Status == ReservationStatus.Approved
                && DbFunctions.TruncateTime(x.RentStart) == DbFunctions.TruncateTime(payload.RentStart)
                && ((x.RentStart >= payload.RentStart && x.RentEnd <= payload.RentStart)
                    || (x.RentStart >= payload.RentEnd && x.RentEnd <= payload.RentEnd)));
            if (collision)
            {
                return new GenericStatusMessage(false, "Reservation collides with an already approved reservation.");
            }

            return new GenericStatusMessage(true);
        }

        public WorkingHours GetMatchingWorkingHours(ReservationRequestPayload payload, Room room)
        {
            int reservationStartTime = payload.RentStart.ToMinutes();
            int resevationEndTime = payload.RentEnd.ToMinutes();
            Days reservationDay = payload.RentStart.ToDaysEnum();
            return room.WorkingHours.FirstOrDefault(x => x.Day == reservationDay && x.TimeStart <= reservationStartTime && x.TimeEnd >= resevationEndTime);
        }

        public GenericStatusMessage ValidateRequest(long roomId, long requestId, long userId)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                ReservationRequest reservationRequest = context.ReservationRequests.Include(x => x.Room).SingleOrDefault(x => x.Id == requestId);
                if (reservationRequest == null)
                {
                    return new GenericStatusMessage(false, $"Reservation with ID {requestId} does not exist.");
                }

                if (reservationRequest.RoomId != roomId)
                {
                    return new GenericStatusMessage(false, $"Request {requestId} does not belong to room {roomId}.");
                }

                if (reservationRequest.Room.OwnerId != userId)
                {
                    return new GenericStatusMessage(false, $"Request {requestId} can not be verified by user.");
                }

                return new GenericStatusMessage(true);
            }
        }
    }
}
