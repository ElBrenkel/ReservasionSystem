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

        private GenericStatusMessage ValidateRoomAvailability(ReservationDataContext context, ReservationRequest request)
        {
            Room room = context.Rooms.Include(x => x.WorkingHours).SingleOrDefault(x => x.Id == request.RoomId);
            ReservationRequestPayload payload = new ReservationRequestPayload
            {
                RentStart = request.RentStart,
                RentEnd = request.RentEnd,
                Description = request.Description
            };

            return ValidateRoomAvailability(context, room, payload);
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

            var todaysReservations = context.ReservationRequests.Where(x => x.RoomId == room.Id
                && x.Status == ReservationStatus.Approved
                && DbFunctions.TruncateTime(x.RentStart) == DbFunctions.TruncateTime(payload.RentStart)).ToList();
            bool collision = todaysReservations.Any(x =>
            {
                bool startValidation = payload.RentStart > x.RentStart && payload.RentStart < x.RentEnd;
                bool endValidation = payload.RentEnd > x.RentStart && payload.RentEnd < x.RentEnd;
                bool sameTimeValidation = payload.RentStart == x.RentStart && payload.RentEnd == x.RentEnd;
                return startValidation || endValidation || sameTimeValidation;
            });

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

        public GenericStatusMessage ValidateRequest(long roomId, long requestId, long userId, bool validateDoubleBooking = false)
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

                if (validateDoubleBooking)
                {
                    return ValidateRoomAvailability(context, reservationRequest);
                }

                return new GenericStatusMessage(true);
            }
        }
    }
}
