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
        public const string TokenHeaderKey = "RS_TOKEN";
        public const string UserHeaderKey = "RS_USER";
        public static bool IsAuthorized(HttpRequest request, params UserRole[] requiredRoles)
        {
            if (request.Headers.ContainsKey(TokenHeaderKey) && request.Headers.ContainsKey(UserHeaderKey) && Guid.TryParse(request.Headers[TokenHeaderKey], out Guid token))
            {
                string username = request.Headers[UserHeaderKey];
                UserValidationService userValidationService = new UserValidationService();
                return userValidationService.ValidateUserToken(username, token, requiredRoles);
            }
            return false;
        }
    }
}
