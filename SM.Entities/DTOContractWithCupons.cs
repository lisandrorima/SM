using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Entities
{
	public class DTOContractWithCupons
	{

		public DTOContractWithCupons()
		{
			
			CuponesDePago = new List<DTOCuponPago>();
		}
		public int ContractId { get; set; }
		public string Address { get; set; }
		public List<DTOCuponPago> CuponesDePago { get; set; }
	}
}
