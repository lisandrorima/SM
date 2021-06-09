using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Entities
{
	public class DTOpaymentVerification
	{
		public string Cupon { get; set; }
		public string Inquilino { get; set; }
		public string Propietario { get; set; }
		public int Monto { get; set; }
		public string TXHash { get; set; }
		public string MinedOnBlock { get; set; }

	}
}
