using ReservationSystemBusinessLogic.Common;
using ReservationSystemBusinessLogic.Context;
using ReservationSystemBusinessLogic.Objects;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Services
{
    public class RoomValidationService
    {
        public GenericStatusMessage ValidateRoomData(CreateRoomPayload payload, bool required = true)
        {
            ValidationHelper helper = new ValidationHelper();
            List<GenericStatusMessage> validations = new List<GenericStatusMessage>
            {
                helper.ValidateValueThanZero(payload.Size, "Size", true, required),
                helper.ValidateValueThanZero(payload.MaxNumberOfPeople, "Maximum number of people", true, required),
                helper.ValidateStringValue(payload.Country, "Country", 2, 20, required),
                helper.ValidateStringValue(payload.City, "City", 2, 20, required),
                helper.ValidateStringValue(payload.Street, "Street", 2, 50, required),
                helper.ValidateValueThanZero(payload.BuildingNumber, "Building number", true, required)
            };

            return validations.FirstOrDefault(x => !x.Success) ?? new GenericStatusMessage(true);
        }

        public GenericStatusMessage ValidateRoomExistsAndOwnedByUser(long roomId, long userId)
        {
            using (ReservationDataContext context = new ReservationDataContext())
            {
                bool exists = context.Rooms.Any(x => x.Id == roomId && x.OwnerId == userId);
                return new GenericStatusMessage(exists);
            }
        }
    }
}
