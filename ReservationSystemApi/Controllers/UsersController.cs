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

        [HttpPost("logout")]
        public GenericStatusMessage Logout()
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.Coach, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
            }
            else
            {
                UserManipulationService userManipulationService = new UserManipulationService();
                userManipulationService.ExpireToken(userId.Value);
            }

            return new GenericStatusMessage(userId != null);
        }

        [HttpGet("")]
        public GenericObjectResponse<UserResponse> GetUserData([FromQuery] string username)
        {
            UserResponse response = null;
            long? queryingUserId = AuthenticationService.IsAuthorized(Request, UserRole.Coach, UserRole.RoomOwner);
            if (queryingUserId == null)
            {
                Response.StatusCode = 401;
                return new GenericObjectResponse<UserResponse>($"Unauthorized request.");
            }
            else
            {
                UserQueryService userQueryService = new UserQueryService();
                if (username == null)
                {
                    response = userQueryService.GetUserById(queryingUserId.Value);
                }
                else
                {
                    response = userQueryService.FindUser(username, queryingUserId.Value);
                }
            }

            if (response == null)
            {
                Response.StatusCode = 404;
                return new GenericObjectResponse<UserResponse>($"Could not find user {username}.");
            }
            return new GenericObjectResponse<UserResponse>(response);
        }

        [HttpGet("ping")]
        public GenericStatusMessage PingUser()
        {
            long? queryingUserId = AuthenticationService.IsAuthorized(Request, UserRole.Coach, UserRole.RoomOwner);
            if (queryingUserId == null)
            {
                Response.StatusCode = 401;
                return new GenericStatusMessage(false, $"Unauthorized request.");
            }

            return new GenericStatusMessage(true);
        }

        [HttpGet("isRoomOwner")]
        public GenericStatusMessage IsRoomOwner()
        {
            long? queryingUserId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner);
            if (queryingUserId == null)
            {
                Response.StatusCode = 401;
                return new GenericStatusMessage(false, $"Unauthorized request.");
            }

            return new GenericStatusMessage(true);
        }

        [HttpGet("rooms")]
        public GenericListResponse<RoomResponse> GetOwnerRooms()
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericListResponse<RoomResponse>("");
            }

            RoomQueryService queryService = new RoomQueryService();
            return queryService.GetRoomsByOwner(userId.Value);
        }

        [HttpGet("reservations")]
        public GenericListResponse<ReservationRequestResponse> GetUserReservations()
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.Coach, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericListResponse<ReservationRequestResponse>("");
            }

            ReservationQueryService queryService = new ReservationQueryService();
            return queryService.GetUserReservations(userId.Value);
        }

        [HttpPost("changePassword")]
        public GenericStatusMessage ChangePassword([FromBody] PasswordChangePayload payload)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.Coach, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericStatusMessage(false);
            }
            else
            {
                UserManipulationService userManipulationService = new UserManipulationService();
                GenericStatusMessage message = userManipulationService.ChangePassword(payload, userId.Value);
                Response.StatusCode = message.Success ? 200 : 401;
                return message;
            }
        }

        [HttpPatch("")]
        public GenericObjectResponse<UserResponse> ChangeUserData([FromBody] ChangeUserDataPayload payload)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.Coach, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericObjectResponse<UserResponse>("");
            }
            else
            {
                UserManipulationService userManipulationService = new UserManipulationService();
                return userManipulationService.ChangeUserData(payload, userId.Value);
            }
        }
    }
}
