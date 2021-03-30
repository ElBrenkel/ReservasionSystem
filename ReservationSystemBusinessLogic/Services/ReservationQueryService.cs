using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ReservationSystemBusinessLogic.Objects;
using ReservationSystemBusinessLogic.Enums;

namespace ReservationSystemBusinessLogic.Services
{
    public class ReservationQueryService
    {
        public GenericListResponse<ReservationRequestResponse> GetReservationsByDate(long roomId, DateTime startDate, DateTime endDate, long userId, int skip, int take)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                Room room = context.Rooms.Include(x => x.Reservations).Single(x => x.Id == roomId);
                IEnumerable<ReservationRequest> reservationsQuery = room.Reservations.Where(x => x.RentStart >= startDate && x.RentStart <= endDate
                    && (x.Status != ReservationStatus.Rejected || (room.OwnerId == userId || x.UserId == userId)));
                List<ReservationRequest> reservationList = reservationsQuery.Skip(skip).Take(take).ToList();
                int totalCount = reservationsQuery.Count();
                long[] reservationIds = reservationList.Select(x => x.Id).ToArray();
                List<ReservationRequest> expandedReservationList = context.ReservationRequests.Include(x => x.User).Where(x => reservationIds.Contains(x.Id)).ToList();
                ReservationManipulationService reservationManipulationService = new ReservationManipulationService();
                List<ReservationRequestResponse> reservationResponses = expandedReservationList.Select(x =>
                {
                    bool returnAllData = room.OwnerId == userId || x.UserId == userId;
                    return reservationManipulationService.ConvertToResponse(x, returnAllData);
                }).OrderBy(x => x.RentStart).ToList();

                return new GenericListResponse<ReservationRequestResponse>(reservationResponses, totalCount);
            }
        }
    }
}
