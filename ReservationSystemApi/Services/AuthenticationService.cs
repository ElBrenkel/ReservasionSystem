using Microsoft.AspNetCore.Http;
using ReservationSystemBusinessLogic.Enums;
using ReservationSystemBusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystemApi.Services
{
    public static class AuthenticationService
    {

        /// <summary>
        /// Returns the user's ID if token is valid, the user exists, and the role is correct.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requiredRoles"></param>
        /// <returns></returns>
        public static long? IsAuthorized(HttpRequest request, params UserRole[] requiredRoles)
        {
            if (request.Headers.ContainsKey(Constants.TokenHeaderKey) && request.Headers.ContainsKey(Constants.UserHeaderKey) && Guid.TryParse(request.Headers[Constants.TokenHeaderKey], out Guid token))
            {
                string username = request.Headers[Constants.UserHeaderKey];
                UserValidationService userValidationService = new UserValidationService();
                return userValidationService.ValidateUserToken(username, token, requiredRoles);
            }
            return null;
        }
    }
}
