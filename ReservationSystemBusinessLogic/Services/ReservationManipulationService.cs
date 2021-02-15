using ReservationSystemApi.Objects;
using ReservationSystemBusinessLogic.ApiClients;
using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Enums;
using ReservationSystemBusinessLogic.Common;
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
    public class ReservationManipulationService
    {
        private ReservationRequesrResponse ConvertToResponse(ReservationRequest request)
        {
            return new ReservationRequesrResponse()
            {
                RoomId = request.RoomId,
                UserId = request.UserId,
                RentStart = request.RentStart,
                RentEnd = request.RentEnd,
                Description = request.Description,
                Status = request.Status,
                FinalPrice = request.FinalPrice
            };
        }

        public GenericObjectResponse<ReservationRequesrResponse> AddReservation(long roomId, long requesterId, ReservationRequestPayload payload)
        {
            ReservationValidationService reservationValidationService = new ReservationValidationService();
            using (ReservationDataContext context = new ReservationDataContext())
            {
                Room room = context.Rooms.Include(x => x.WorkingHours).Single(x => x.Id == roomId);
                WorkingHours matchedWorkingHours = reservationValidationService.GetMatchingWorkingHours(payload, room);
                ReservationRequest reservationRequest = new ReservationRequest()
                {
                    RentStart = payload.RentStart,
                    RentEnd = payload.RentEnd,
                    Status = ReservationStatus.Pending,
                    Description = payload.Description,
                    FinalPrice = CalculatePrice(matchedWorkingHours.PriceForHour, payload.RentStart, payload.RentEnd),
                    RoomId = roomId,
                    UserId = requesterId

                };
                context.ReservationRequests.Add(reservationRequest);
                context.SaveChanges();

                return new GenericObjectResponse<ReservationRequesrResponse>(ConvertToResponse(reservationRequest));
            }
        }

        private decimal CalculatePrice(decimal priceForHour, DateTime rentStart, DateTime rentEnd)
        {
            decimal priceForMinute = priceForHour / 60;
            decimal timeInMinutes = rentEnd.ToMinutes() - rentStart.ToMinutes();
            return priceForMinute * timeInMinutes;
        }

        public GenericStatusMessage ChangeReservationApproval(long roomId, long requestId, ReservationStatus status)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                Room room = context.Rooms.Include(x => x.WorkingHours).Single(x => x.Id == roomId);
                ReservationRequest reservationRequest = context.ReservationRequests.Single(x => x.Id == requestId);
                reservationRequest.Status = status;
                context.SaveChanges();

                return new GenericStatusMessage(true);
            }
        }
    }
}
