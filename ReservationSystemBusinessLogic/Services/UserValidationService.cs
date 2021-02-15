using ReservationSystemBusinessLogic.Common;
using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using ReservationSystemBusinessLogic.Objects;
using ReservationSystemBusinessLogic.Log;
using ReservationSystemBusinessLogic.Enums;

namespace ReservationSystemBusinessLogic.Services
{
    public class UserValidationService
    {
        private const string EmailRegexPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        private readonly TimeSpan TokenExpiryWindow = TimeSpan.FromHours(8);

        /// <summary>
        /// Returns the user's ID if token is valid, the user exists, and the role is correct.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <param name="requiredRoles"></params
        /// <returns></returns>
        public long? ValidateUserToken(string username, Guid token, UserRole[] requiredRoles)
        {
            using (ReservationDataContext context = new())
            {
                User user = context.Users.SingleOrDefault(x => x.Username == username && x.Token == token && requiredRoles.Contains(x.Role));
                if (user == null)
                {
                    Logger.Debug($"{username} does not exist.");
                    return null;
                }

                if (user.TokenExpiryDate < DateTime.UtcNow)
                {
                    Logger.Debug($"{username}'s token has expired.");
                    return null;
                }

                user.TokenExpiryDate = DateTime.UtcNow + TokenExpiryWindow;
                context.SaveChanges();
                return user.Id;
            }
        }

        /// <summary>
        /// User payload validations:
        /// Email matches ^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$ regex.
        /// Password matches confirm password.
        /// First and last name does not exceed 20 chars.
        /// Country and city does not exceed 20 chars.
        /// Street does not exceed 50 chars.
        /// Building number > 0.
        /// </summary>
        public GenericStatusMessage ValidateCreateUserData(CreateUserPayload payload)
        {
            if (!payload.Username.Matches(EmailRegexPattern))
            {
                return new GenericStatusMessage(false, "Username is not an email.");
            }

            using (ReservationDataContext context = new())
            {
                bool usernameExists = context.Users.Any(x => x.Username == payload.Username);
                if (usernameExists)
                {
                    return new GenericStatusMessage(false, "Username already exists.");
                }
            }

            if (!payload.Password.CheckLength(25, 8) || !payload.ConfirmPassword.CheckLength(25, 8))
            {
                return new GenericStatusMessage(false, "Passwords should be between 8 and 25 characters.");
            }

            if (payload.Password != payload.ConfirmPassword)
            {
                return new GenericStatusMessage(false, "Passwords do not match.");
            }

            return ValidateUserData(payload);
        }

        public LoginResponse ValidateLogin(string username, string password)
        {
            using (ReservationDataContext context = new())
            {
                User user = context.Users.FirstOrDefault(x => x.Username == username);
                if (user == null)
                {
                    Logger.Debug($"Username {username} does not exist.");
                    return null;
                }

                bool correctPassword = PasswordHasher.Validate(password, user.PasswordHash);
                if (!correctPassword)
                {
                    Logger.Debug($"{username} failed to login due to incorrect password.");
                    return null;
                }

                user.Token = Guid.NewGuid();
                user.TokenExpiryDate = DateTime.UtcNow + TokenExpiryWindow;
                context.SaveChanges();
                return new LoginResponse() { Token = user.Token };
            }
        }

        public GenericStatusMessage ValidateUserData(ChangeUserDataPayload payload, bool required = true)
        {
            ValidationHelper helper = new ValidationHelper();
            List<GenericStatusMessage> validations = new List<GenericStatusMessage>
            {
                helper.ValidateStringValue(payload.FirstName, "First name", 2, 20, required),
                helper.ValidateStringValue(payload.LastName, "Last name", 2, 20, required),
                helper.ValidateStringValue(payload.Country, "Country", 2, 20, required),
                helper.ValidateStringValue(payload.City, "City", 2, 20, required),
                helper.ValidateStringValue(payload.Street, "Street", 2, 50, required)
            };

            bool buildingValueExists = required || payload.BuildingNumber.HasValue;
            if (buildingValueExists && payload.BuildingNumber <= 0)
            {
                validations.Add(new GenericStatusMessage(false, "Building number cannot be non positive."));
            }

            if (validations.Any(x => !x.Success))
            {
                return validations.First(x => !x.Success);
            }
            return new GenericStatusMessage(true);
        }
    }
}
