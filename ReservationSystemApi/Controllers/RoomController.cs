using Microsoft.AspNetCore.Mvc;
using ReservationSystemApi.Objects;
using ReservationSystemApi.Services;
using ReservationSystemBusinessLogic.Common;
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
    [Route("api/room")]
    public class RoomController : Controller
    {
        [HttpPost("")]
        public async Task<GenericObjectResponse<RoomResponse>> AddRoom([FromBody] CreateRoomPayload payload)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericObjectResponse<RoomResponse>("");
            }

            RoomManipulationService service = new RoomManipulationService();
            GenericObjectResponse<RoomResponse> response = await service.AddRoom(payload, userId);
            if (!response.Status.Success)
            {
                Response.StatusCode = 400;
            }

            return response;
        }

        [HttpPost("{roomId}/workingHours")]
        public GenericObjectResponse<List<WorkingHoursPayload>> ChangeWorkingHours(long roomId, [FromBody] List<WorkingHoursPayload> payload)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericObjectResponse<List<WorkingHoursPayload>>("");
            }

            RoomValidationService roomValidationService = new RoomValidationService();
            GenericStatusMessage roomExistsValidation = roomValidationService.ValidateRoomExistsAndOwnedByUser(roomId, userId.Value);
            if (!roomExistsValidation.Success)
            {
                Response.StatusCode = 404;
                return new GenericObjectResponse<List<WorkingHoursPayload>>("Not found.");
            }

            WorkingHoursManipulationService service = new WorkingHoursManipulationService();
            GenericObjectResponse<List<WorkingHoursPayload>> response = service.ChangeWorkingHoursForRoom(roomId, payload);
            if (!response.Status.Success)
            {
                Response.StatusCode = 400;
            }

            return response;
        }

        [HttpPatch("{roomId}")]
        public async Task<GenericObjectResponse<RoomResponse>> ChangeRoomData([FromBody] CreateRoomPayload payload, long roomId)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericObjectResponse<RoomResponse>("");
            }

            RoomValidationService roomValidationService = new RoomValidationService();
            GenericStatusMessage roomExistsValidation = roomValidationService.ValidateRoomExistsAndOwnedByUser(roomId, userId.Value);
            if (!roomExistsValidation.Success)
            {
                Response.StatusCode = 404;
                return new GenericObjectResponse<RoomResponse>("Not found.");
            }

            RoomManipulationService roomManipulationService = new RoomManipulationService();
            return await roomManipulationService.ChangeRoomData(payload, roomId);
        }

        [HttpGet("{roomId}")]
        public GenericObjectResponse<RoomResponse> GetRoomById(long roomId, [FromQuery] bool expand = false)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner, UserRole.Coach);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericObjectResponse<RoomResponse>("");
            }

            RoomQueryService queryService = new RoomQueryService();
            var room = queryService.GetRoomById(roomId, userId, expand);
            if (!room.Status.Success)
            {
                Response.StatusCode = 404;
            }

            return room;
        }

        [HttpGet("{roomId}/availableTimes")]
        public GenericListResponse<AvailableReservationTime> GetRoomAvailableTimes(long roomId, [FromQuery] DateTime? startDate, [FromQuery] int duration, int take = 10)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner, UserRole.Coach);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericListResponse<AvailableReservationTime>("");
            }

            RoomQueryService queryService = new RoomQueryService();
            var availableTimes = queryService.GetRoomAvailableTimes(roomId, startDate, duration, take);
            if (!availableTimes.Status.Success)
            {
                Response.StatusCode = 404;
            }

            return availableTimes;
        }

        [HttpGet("nearby")]
        public GenericListResponse<RoomResponse> GetNearbyRooms([FromQuery] decimal lat, [FromQuery] decimal lon, [FromQuery] int radius,
            [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner, UserRole.Coach);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericListResponse<RoomResponse>("");
            }

            RoomQueryService queryService = new RoomQueryService();
            return queryService.GetRoomsByLatLonAndRadius(userId, lat, lon, radius, skip, take);
        }

        [HttpGet("search")]
        public GenericListResponse<RoomResponse> GetNearbyCity([FromQuery] string city, [FromQuery] string name, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner, UserRole.Coach);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericListResponse<RoomResponse>("");
            }

            RoomQueryService queryService = new RoomQueryService();
            return queryService.GetRoomsByNameOrCity(userId, city, name, skip, take);
        }

        [HttpPost("{roomId}/request")]
        public GenericObjectResponse<ReservationRequestResponse> RequestReservation(long roomId, [FromBody] ReservationRequestPayload payload)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner, UserRole.Coach);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericObjectResponse<ReservationRequestResponse>("");
            }

            ReservationRequestPayload trimmedPayload = new ReservationRequestPayload()
            {
                Description = payload.Description,
                RentStart = payload.RentStart.TrimDate(DateTimePrecision.Minute).ToLocalDate(),
                RentEnd = payload.RentEnd.TrimDate(DateTimePrecision.Minute).ToLocalDate()
            };
            ReservationValidationService reservationValidationService = new ReservationValidationService();
            GenericStatusMessage roomAvailabilityValidation = reservationValidationService.ValidateRoomAvailability(roomId, trimmedPayload);
            if (!roomAvailabilityValidation.Success)
            {
                Response.StatusCode = 400;
                return new GenericObjectResponse<ReservationRequestResponse>(roomAvailabilityValidation.Message);
            }

            ReservationManipulationService reservationManipulationService = new ReservationManipulationService();
            return reservationManipulationService.AddReservation(roomId, userId.Value, trimmedPayload);
        }

        [HttpPost("{roomId}/request/{requestId}/accept")]
        public GenericStatusMessage ApproveReservation(long roomId, long requestId)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericStatusMessage(false, "");
            }

            ReservationValidationService reservationValidationService = new ReservationValidationService();
            GenericStatusMessage requestExistsValidation = reservationValidationService.ValidateRequest(roomId, requestId, userId.Value);
            if (!requestExistsValidation.Success)
            {
                Response.StatusCode = 400;
                return new GenericStatusMessage(false, requestExistsValidation.Message);
            }

            ReservationManipulationService reservationManipulationService = new ReservationManipulationService();
            return reservationManipulationService.ChangeReservationApproval(requestId, ReservationStatus.Approved);
        }

        [HttpPost("{roomId}/request/{requestId}/reject")]
        public GenericStatusMessage RejectReservation(long roomId, long requestId)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericStatusMessage(false, "");
            }

            ReservationValidationService reservationValidationService = new ReservationValidationService();
            GenericStatusMessage requestExistsValidation = reservationValidationService.ValidateRequest(roomId, requestId, userId.Value);
            if (!requestExistsValidation.Success)
            {
                Response.StatusCode = 400;
                return new GenericStatusMessage(false, requestExistsValidation.Message);
            }

            ReservationManipulationService reservationManipulationService = new ReservationManipulationService();
            return reservationManipulationService.ChangeReservationApproval(requestId, ReservationStatus.Rejected);
        }

        [HttpGet("{roomId}/request/list")]
        public GenericListResponse<ReservationRequestResponse> GetReservationsByDate(long roomId, [FromQuery] DateTime startDate, [FromQuery] DateTime EndDate,
            [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.Coach, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericListResponse<ReservationRequestResponse>("");
            }

            RoomValidationService roomValidationService = new RoomValidationService();
            GenericStatusMessage roomExistsValidation = roomValidationService.ValidateRoomExists(roomId);
            if (!roomExistsValidation.Success)
            {
                Response.StatusCode = 404;
                return new GenericListResponse<ReservationRequestResponse>("Not found.");
            }

            ReservationQueryService queryService = new ReservationQueryService();
            return queryService.GetReservationsByDate(roomId, startDate, EndDate, userId.Value, skip, take);
        }

        [HttpPost("{roomId}/deactivate")]
        public GenericStatusMessage DeactivateRoom(long roomId, [FromBody] bool force)
        {
            return ChangeRoomActivation(roomId, false, force);
        }

        [HttpPost("{roomId}/activate")]
        public GenericStatusMessage ActivateRoom(long roomId)
        {
            return ChangeRoomActivation(roomId, true);
        }

        private GenericStatusMessage ChangeRoomActivation(long roomId, bool activate, bool force = false)
        {
            long? userId = AuthenticationService.IsAuthorized(Request, UserRole.RoomOwner);
            if (userId == null)
            {
                Response.StatusCode = 401;
                return new GenericStatusMessage(false, "");
            }

            RoomValidationService roomValidationService = new RoomValidationService();
            GenericStatusMessage roomExistsValidation = roomValidationService.ValidateRoomExistsAndOwnedByUser(roomId, userId.Value);
            if (!roomExistsValidation.Success)
            {
                Response.StatusCode = 404;
                return new GenericStatusMessage(false, "Not found.");
            }

            RoomManipulationService roomManipulationService = new RoomManipulationService();
            return roomManipulationService.ChangeRoomActivation(roomId, activate, force);
        }
    }
}
