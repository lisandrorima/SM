using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Entities
{
	public class DTOAddRealEstate
	{
		public string Address { get; set; }
		public int RentFee { get; set; }
		public int RentDurationDays { get; set; }
		public int RentPaymentSchedule { get; set; }
		public float SqMtrs { get; set; }
		public string Description { get; set; }
		public int Rooms { get; set; }
		public int BedRoomQty { get; set; }
		public int BathRoomQty { get; set; }
		public bool Garage { get; set; }
		public bool Available { get; set; }
	}
}
