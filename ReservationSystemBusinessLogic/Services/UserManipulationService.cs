using ReservationSystemApi.Objects;
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
        private User ConvertUserFromPayload(CreateUserPayload payload, UserRole role)
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

        private UserResponse ConvertUserToUserResponse(User user)
        {
            return new UserResponse
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Country = user.Country,
                City = user.City,
                Street = user.Street,
                BuildingNumber = user.BuildingNumber
            };
        }

        public GenericStatusMessage AddUser(CreateUserPayload payload, UserRole role)
        {
            try
            {
                Logger.Debug($"Attempting to create new user {payload.Username}");
                using (ReservationDataContext context = new ReservationDataContext())
                {
                    User user = ConvertUserFromPayload(payload, role);
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

        public GenericStatusMessage ChangePassword(PasswordChangePayload payload, long userId)
        {
            using (ReservationDataContext context = new())
            {
                User user = context.Users.Single(x => x.Id == userId);
                bool correctPassword = PasswordHasher.Validate(payload.CurrentPassword, user.PasswordHash);
                if (!correctPassword)
                {
                    Logger.Debug($"{user.Username} failed to change password due to incorrect password.");
                    return new GenericStatusMessage(false, "Password incorrect.");
                }
                else if (payload.NewPassword != payload.NewPasswordAgain)
                {
                    Logger.Debug($"{user.Username} failed to change password due to mismatching passwords.");
                    return new GenericStatusMessage(false, "Passwords do not match.");
                }

                user.PasswordHash = PasswordHasher.Create(payload.NewPassword);
                context.SaveChanges();
                return new GenericStatusMessage(true);
            }
        }

        public GenericObjectResponse<UserResponse> ChangeUserData(ChangeUserDataPayload payload, long userId)
        {
            UserValidationService service = new UserValidationService();
            GenericStatusMessage validationResponse = service.ValidateUserData(payload, false);
            if (validationResponse.Success)
            {
                using (ReservationDataContext context = new())
                {
                    User user = context.Users.Single(x => x.Id == userId);
                    user.FirstName = payload.FirstName ?? user.FirstName;
                    user.LastName = payload.LastName ?? user.LastName;
                    user.Country = payload.Country ?? user.Country;
                    user.City = payload.City ?? user.City;
                    user.Street = payload.Street ?? user.Street;
                    user.BuildingNumber = payload.BuildingNumber ?? user.BuildingNumber;
                    context.SaveChanges();
                    return new GenericObjectResponse<UserResponse>(ConvertUserToUserResponse(user));
                }
            }
            else
            {
                return new GenericObjectResponse<UserResponse>(validationResponse.Message);
            }
        }
    }
}
