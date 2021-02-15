using ReservationSystemBusinessLogic.Common;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationSystemBusinessLogic.Common;

namespace ReservationSystemBusinessLogic.Services
{
    public class WorkingHoursValidationService
    {
        public GenericStatusMessage ValidateWorkingHoursData(WorkingHoursPayload payload, bool required = true)
        {
            ValidationHelper helper = new ValidationHelper();
            List<GenericStatusMessage> validations = new List<GenericStatusMessage>
            {
                helper.ValidateBetweenValues(payload.TimeStart, "Start time", 0, 1439, required),
                helper.ValidateBetweenValues(payload.TimeEnd, "End time", 0, 1439, required),
                new GenericStatusMessage(payload.TimeStart < payload.TimeEnd, "End time can't be sooner than the start.")
            };

            return validations.FirstOrDefault(x => !x.Success) ?? new GenericStatusMessage(true);
        }

        public GenericStatusMessage ValidateWorkingHoursData(List<WorkingHoursPayload> list, bool required = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                WorkingHoursPayload current = list[i];
                GenericStatusMessage status = ValidateWorkingHoursData(current, required);
                if (!status.Success)
                {
                    status.Message = $"Working hours [{i}]: {status.Message}";
                    return status;
                }
            }

            List<GenericStatusMessage> overlapsValidations = list.GroupBy(x => x.Day).Select(x => CheckOverlapsForDay(x)).ToList();
            return overlapsValidations.FirstOrDefault(x => !x.Success) ?? new GenericStatusMessage(true);
        }

        private GenericStatusMessage CheckOverlapsForDay(IEnumerable<WorkingHoursPayload> timeSlotsForDay)
        {
            List<WorkingHoursPayload> listForDay = timeSlotsForDay.OrderBy(x => x.TimeStart).ToList();
            if (listForDay.Count <= 1)
            {
                return new GenericStatusMessage(true);
            }

            for (int i = 0; i < listForDay.Count - 1; i++)
            {
                WorkingHoursPayload current = listForDay[i];
                WorkingHoursPayload next = listForDay[i + 1];
                if (next.TimeStart <= current.TimeEnd)
                {
                    return new GenericStatusMessage(false, $"Found overlapping in day {current.Day}: [{current.TimeStart.ToTime()}]-[{current.TimeEnd.ToTime()}] and [{next.TimeStart.ToTime()}]-[{next.TimeEnd.ToTime()}]");
                }
            }

            return new GenericStatusMessage(true);
        }
    }
}
