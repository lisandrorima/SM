using System;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Web3
{
	[Event("pagoRealizado")]
	public class PagoRealizadoEventDTO : IEventDTO
	{

		[Parameter("address", "inquilino", 1, true)]
		public string inquilino { get; set; }

		[Parameter("address", "propietario", 2, true)]
		public string propietario { get; set; }
		
		[Parameter("bytes32", "cupon", 3, true)]
		public string cupon { get; set; }

		[Parameter("uint", "monto", 4, false)]
		public int monto { get; set; }
	}
}
