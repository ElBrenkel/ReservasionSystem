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
    public class WorkingHoursManipulationService
    {
        private WorkingHours ConvertWorkingHoursFromPayload(long roomId, WorkingHoursPayload payload)
        {
            return new WorkingHours
            {
                RoomId = roomId,
                Day = payload.Day,
                TimeStart = payload.TimeStart,
                TimeEnd = payload.TimeEnd,
                PriceForHour = payload.PriceForHour
            };
        }

        public WorkingHoursPayload ConvertWorkingHoursToPayload(WorkingHours workingHours)
        {
            return new WorkingHoursPayload
            {
                Day = workingHours.Day,
                TimeStart = workingHours.TimeStart,
                TimeEnd = workingHours.TimeEnd,
                PriceForHour = workingHours.PriceForHour
            };
        }

        public GenericObjectResponse<List<WorkingHoursPayload>> ChangeWorkingHoursForRoom(long roomId, List<WorkingHoursPayload> workingHours, bool shouldValidate = true)
        {
            if (shouldValidate)
            {
                WorkingHoursValidationService workingHoursValidationService = new WorkingHoursValidationService();
                GenericStatusMessage validationMessage = workingHoursValidationService.ValidateWorkingHoursData(workingHours);
                if (!validationMessage.Success)
                {
                    return new GenericObjectResponse<List<WorkingHoursPayload>>(validationMessage.Message);
                }
            }

            try
            {
                using (ReservationDataContext context = new ReservationDataContext())
                {
                    List<WorkingHours> workingHoursToRemove = context.WorkingHours.Where(x => x.RoomId == roomId).ToList();
                    context.WorkingHours.RemoveRange(workingHoursToRemove);
                    List<WorkingHours> workingHoursToAdd = workingHours.Select(x => ConvertWorkingHoursFromPayload(roomId, x)).ToList();
                    context.WorkingHours.AddRange(workingHoursToAdd);
                    context.SaveChanges();
                    List<WorkingHoursPayload> savedWorkingHours = context.WorkingHours
                        .Where(x => x.RoomId == roomId)
                        .Select(ConvertWorkingHoursToPayload)
                        .ToList();
                    return new GenericObjectResponse<List<WorkingHoursPayload>>(savedWorkingHours);
                }
            }
            catch (DbEntityValidationException e)
            {
                string exceptionMessage = e.EntityValidationErrors.FirstOrDefault()?.ValidationErrors.FirstOrDefault()?.ErrorMessage;
                Logger.Error($"Failed to create working hours. Error: '{exceptionMessage}'");
                return new GenericObjectResponse<List<WorkingHoursPayload>>("Failed to add working hours, please contact support.");
            }
        }
    }
}
