using Microsoft.AspNetCore.Mvc;
using ReservationSystemApi.Objects;
using ReservationSystemApi.Services;
using ReservationSystemBusinessLogic;
using ReservationSystemBusinessLogic.Enums;
using ReservationSystemBusinessLogic.Objects.Api;
using ReservationSystemBusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystemApi.Controllers
{
    [Route("api/user")]
    public class UsersController : Controller
    {
        [HttpPost("{role}")]
        public GenericStatusMessage CreateUser([FromBody] CreateUserPayload payload, UserRole role)
        {
            UserValidationService userValidationService = new UserValidationService();
            GenericStatusMessage genericStatusMessage = userValidationService.ValidateCreateUserData(payload);
            if (!genericStatusMessage.Success)
            {
                Response.StatusCode = 400;
                return genericStatusMessage;
            }

            UserManipulationService userManipulationService = new UserManipulationService();
            return userManipulationService.AddUser(payload, role);
        }

        [HttpPost("login")]
        public LoginResponse Login([FromBody] UserLoginPayload payload)
        {
            UserValidationService userValidationService = new UserValidationService();
            LoginResponse response = userValidationService.ValidateLogin(payload.Username, payload.Password);
            if (response == null)
            {
                Response.StatusCode = 401;
            }

            return response;
        }

        [HttpGet("poke")]
        public GenericStatusMessage Poke()
        {
            bool isAuthorized = AuthenticationService.IsAuthorized(Request, UserRole.Coach, UserRole.RoomOwner);
            return new GenericStatusMessage(isAuthorized);
        }
    }
}
