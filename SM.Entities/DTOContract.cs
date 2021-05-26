using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Entities
{
	public class DTOContract
	{
		public int ID { get; set; }
		public DTOUser Owner { get; set; }
		public DTOUser Tenant { get; set; }
		public DTOShowRealEstate RealEstate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Hash { get; set; }
		public bool ValidatedByBlockChain { get; set; }
	}
}
