using ReservationSystemBusinessLogic.Enums;
using ReservationSystemBusinessLogic.Objects;
using ReservationSystemBusinessLogic.Objects.Api;
using ReservationSystemBusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Context.Migrations
{
    public class AddInitialData : DbMigration
    {
        public override void Up()
        {
            UserManipulationService userManipulationService = new UserManipulationService();
            CreateUserPayload owner = new CreateUserPayload
            {
                FirstName = "Ron",
                LastName = "Shachar",
                Country = "Israel",
                City = "Tel Aviv",
                Street = "Rambam",
                BuildingNumber = 6,
                Password = "password123",
                ConfirmPassword = "password123",
                Username = "ron@shachar.com"
            };

            userManipulationService.AddUser(owner, UserRole.RoomOwner);
            long ownerId = 0;
            using (ReservationDataContext context = new ReservationDataContext())
            {
                ownerId = context.Users.Single(x => x.Username == "ron@shachar.com").Id;
            }

            RoomManipulationService roomManipulationService = new RoomManipulationService();
            CreateRoomPayload roomPayload = new CreateRoomPayload
            {
                Name = "Globo Gym",
                Country = "Israel",
                City = "Givatayim",
                Street = "Borochov",
                BuildingNumber = 5,
                IsActive = true,
                WorkingHours = CreateDefaultWorkingHours(Days. Sunday, Days.Tuesday, Days.Thursday)
            };

            roomManipulationService.AddRoom(roomPayload, ownerId);
        }

        private List<WorkingHoursPayload> CreateDefaultWorkingHours(params Days[] days)
        {
            return days.Select(x => new WorkingHoursPayload
            {
                Day = x,
                PriceForHour = 100,
                TimeStart = 480,
                TimeEnd = 1080
            }).ToList();
        }
    }
}
