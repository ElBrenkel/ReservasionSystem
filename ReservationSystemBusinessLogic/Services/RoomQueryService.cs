using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using ReservationSystemBusinessLogic.Common;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Services
{
    public class RoomQueryService
    {
        public GenericListResponse<RoomResponse> GetRoomsByLatLonAndRadius(decimal lat, decimal lon, int radius, int skip = 0, int take = 10)
        {
            RoomManipulationService roomManipulationService = new RoomManipulationService();
            using (ReservationDataContext context = new ReservationDataContext())
            {
                var roomsQuery = context.Rooms.Include(x => x.WorkingHours)
                    .Where(x => x.Lat.HasValue && x.Lon.HasValue)
                    .ToList()
                    .Select(x => new { Room = x, Distance = GetDistance(lon, lat, x.Lon.Value, x.Lat.Value) })
                    .Where(x => x.Distance <= radius);

                List<RoomResponse> rooms = roomsQuery
                    .Skip(skip)
                    .Take(take)
                    .Select(x => roomManipulationService.ConvertRoomToResponse(x.Room))
                    .ToList();

                int count = roomsQuery.Count();
                return new GenericListResponse<RoomResponse>(rooms, count);
            }
        }

        private double GetDistance(decimal longitude, decimal latitude, decimal otherLongitude, decimal otherLatitude)
        {
            var d1 = (double)latitude * (Math.PI / 180.0);
            var num1 = (double)longitude * (Math.PI / 180.0);
            var d2 = (double)otherLatitude * (Math.PI / 180.0);
            var num2 = (double)otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public GenericListResponse<RoomResponse> GetRoomsByNameOrCity(string city, string name, int skip, int take)
        {
            RoomManipulationService roomManipulationService = new RoomManipulationService();
            using (ReservationDataContext context = new ReservationDataContext())
            {
                var roomsQuery = context.Rooms.Include(x => x.WorkingHours)
                    .ToList()
                    .Where(x => CompareStrings(city, x.City) || CompareStrings(name, x.Name));
                List<RoomResponse> rooms = roomsQuery
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToList()
                    .Select(roomManipulationService.ConvertRoomToResponse)
                    .ToList();

                int count = roomsQuery.Count();
                return new GenericListResponse<RoomResponse>(rooms, count);
            }
        }

        private bool CompareStrings(string a, string b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            string lowerA = a.ToLower();
            string lowerB = b.ToLower();
            return (lowerA.LevenshteinDistance(lowerB) <= 3 || lowerB.Contains(lowerA));
        }

        public GenericObjectResponse<RoomResponse> GetRoomById(long roomId, bool expand)
        {
            RoomManipulationService roomManipulationService = new RoomManipulationService();
            using (ReservationDataContext context = new ReservationDataContext())
            {
                var roomQuery = expand ? context.Rooms.Include(x => x.WorkingHours).Include(x => x.Reservations) : context.Rooms;
                var room = roomQuery.SingleOrDefault(x => x.Id == roomId);
                return room != null
                    ? new GenericObjectResponse<RoomResponse>(roomManipulationService.ConvertRoomToResponse(room))
                    : new GenericObjectResponse<RoomResponse>("Room not found");
            }
        }
    }
}
