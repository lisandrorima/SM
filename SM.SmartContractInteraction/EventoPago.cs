using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SM.SmartContractInteraction
{
	public class EventoPago
	{
		public string inquilino { get; set; }

		public string propietario { get; set; }

		public string cupon { get; set; }
		public string TXHash { get; set; }
		public string MinedOnBlock { get; set; }

		public BigInteger monto { get; set; }
		public BigInteger montoPropietario { get; set; }
		public BigInteger montoComision { get; set; }
	}
}
