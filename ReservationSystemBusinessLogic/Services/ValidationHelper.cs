using ReservationSystemBusinessLogic.Common;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Services
{
    public class ValidationHelper
    {
        public GenericStatusMessage ValidateStringValue(string value, string valueName, int minChars, int maxChars, bool required)
        {
            bool shouldBeValidated = required || !value.IsNullOrWhiteSpace();
            if (!value.CheckLength(maxChars, minChars, shouldBeValidated))
            {
                return new GenericStatusMessage(false, $"{valueName} should be between {minChars} and {maxChars} characters.");
            }
            return new GenericStatusMessage(true);
        }

        public GenericStatusMessage ValidateValueThanZero(int? value, string valueName, bool isGreater, bool required)
        {
            bool shouldBeValidated = required || (value != null);
            if (!value.CheckIntFromZero(isGreater, shouldBeValidated))
            {
                string failMessage = isGreater ? "greater" : "smaller";
                return new GenericStatusMessage(false, $"{valueName} should be {failMessage} than 0.");
            }
            return new GenericStatusMessage(true);
        }

        public GenericStatusMessage ValidateBetweenValues(int? value, string valueName, int minValue, int maxValue, bool required)
        {
            bool shouldBeValidated = required || (value != null);
            if (!value.CheckIntBetweenValues(minValue, maxValue, shouldBeValidated))
            {
                return new GenericStatusMessage(false, $"{valueName} should be between {minValue} and {minValue}.");
            }
            return new GenericStatusMessage(true);
        }
    }
}
