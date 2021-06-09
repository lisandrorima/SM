using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Entities
{
	public class DTOCuponPago
	{
		public int ID { get; set; }
		public string HashCuponPago { get; set; }
		public DateTime FechaVencimiento { get; set; }
		public bool IsPayed { get; set; }
		public int RentContractID { get; set; }
		public int Monto { get; set; }
		public string OwnerWallet { get; set; }
		public string TXHash { get; set; }
		public string MinedOnBlock { get; set; }
	}
}
