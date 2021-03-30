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
        public ReservationRequestResponse ConvertToResponse(ReservationRequest request, bool returnAllData = true)
        {
            return new ReservationRequestResponse()
            {
                Id = returnAllData ? request.Id : null,
                RoomId = returnAllData ? request.RoomId : null,
                UserId = returnAllData ? request.UserId : null,
                UserFullName = returnAllData ? $"{request.User?.FirstName ?? ""} {request.User?.LastName ?? ""}" : null,
                RentStart = request.RentStart,
                RentEnd = request.RentEnd,
                Description = returnAllData ? request.Description : null,
                Status = request.Status,
                FinalPrice = returnAllData ? request.FinalPrice : null
            };
        }

        public GenericObjectResponse<ReservationRequestResponse> AddReservation(long roomId, long requesterId, ReservationRequestPayload payload)
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
                    FinalPrice = PriceHelper.CalculatePrice(matchedWorkingHours.PriceForHour, payload.RentStart, payload.RentEnd),
                    RoomId = roomId,
                    UserId = requesterId

                };
                context.ReservationRequests.Add(reservationRequest);
                context.SaveChanges();

                return new GenericObjectResponse<ReservationRequestResponse>(ConvertToResponse(reservationRequest));
            }
        }

        public GenericStatusMessage ChangeReservationApproval(long requestId, ReservationStatus status)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                ReservationRequest reservationRequest = context.ReservationRequests.Single(x => x.Id == requestId);
                reservationRequest.Status = status;
                context.SaveChanges();

                return new GenericStatusMessage(true);
            }
        }
    }
}
