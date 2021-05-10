using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Entities
{
	public class DTORentProperty
	{
		public int ID { get; set; }
		public string Address { get; set; }
		public int RentFee { get; set; }
		public int RentDurationDays { get; set; }
		public int RentPaymentSchedule { get; set; }
	}
}
