using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Enums;
using ReservationSystemBusinessLogic.Log;
using ReservationSystemBusinessLogic.Objects;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Services
{
    public class UserQueryService
    {
        public GetUserResponse FindUser(string usernameToFind, long queryingUserId)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                User queryingUser = context.Users.Single(x => x.Id == queryingUserId);
                if (usernameToFind == queryingUser.Username)
                {
                    Logger.Debug($"User {usernameToFind} found itself.");
                    return ConvertFromUser(queryingUser);
                }
                else if (queryingUser.Role == UserRole.RoomOwner)
                {
                    User userToFind = context.Users.SingleOrDefault(x => x.Username == usernameToFind);
                    if (userToFind != null && userToFind.Role == UserRole.Coach)
                    {
                        Logger.Debug($"User {queryingUser.Username} found {usernameToFind}.");
                        return ConvertFromUser(userToFind);
                    }
                }
                Logger.Debug($"No user {usernameToFind} was found.");
                return null;
            }
        }

        private GetUserResponse ConvertFromUser(User user)
        {
            return new GetUserResponse()
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
    }
}
