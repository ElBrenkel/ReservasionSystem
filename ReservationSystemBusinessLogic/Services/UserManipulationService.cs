using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Enums;
using ReservationSystemBusinessLogic.Log;
using ReservationSystemBusinessLogic.Objects;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Services
{
    public class UserManipulationService
    {
        private User ConvertFromPayload(CreateUserPayload payload, UserRole role)
        {
            return new User
            {
                Username = payload.Username,
                PasswordHash = PasswordHasher.Create(payload.Password),
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                Country = payload.Country,
                City = payload.City,
                Street = payload.Street,
                BuildingNumber = payload.BuildingNumber,
                Role = role
            };
        }

        public GenericStatusMessage AddUser(CreateUserPayload payload, UserRole role)
        {
            try
            {
                Logger.Debug($"Attempting to create new user {payload.Username}");
                using (ReservationDataContext context = new ReservationDataContext())
                {
                    User user = ConvertFromPayload(payload, role);
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                Logger.Debug($"{payload.Username} was created successfully.");
                return new GenericStatusMessage(true);
            }
            catch (DbEntityValidationException e)
            {
                string exceptionMessage = e.EntityValidationErrors.FirstOrDefault()?.ValidationErrors.FirstOrDefault()?.ErrorMessage;
                Logger.Error($"Failed to create user {payload.Username}. Error: '{exceptionMessage}'");
                return new GenericStatusMessage(false, "Failed to add user, please contact support.");
            }
        }

        public void ExpireToken(long userId)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                User user = context.Users.Single(x => x.Id == userId);
                user.TokenExpiryDate = DateTime.UtcNow;
                context.SaveChanges();
            }
        }
    }
}
