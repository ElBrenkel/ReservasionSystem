using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Context.Attributes
{
	public class DecimalPrecisionAttribute : Attribute
	{
        public byte Precision { get; set; }

        public byte Scale { get; set; }

        public DecimalPrecisionAttribute(byte precision, byte scale)
		{
			Precision = precision;
			Scale = scale;
		}
	}

	public class DecimalPrecisionAttributeConvention : PrimitivePropertyAttributeConfigurationConvention<DecimalPrecisionAttribute>
	{
		public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DecimalPrecisionAttribute attribute)
		{
			configuration.HasPrecision(attribute.Precision, attribute.Scale);
		}
	}
}
