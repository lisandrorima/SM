using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using Nethereum.ABI.FunctionEncoding.Attributes;


namespace SM.SmartContractInteraction
{
	[Event("pagoRealizado")]
	class PagoRealizadoEventDTO : IEventDTO
	{

		[Parameter("address", "inquilino", 1, true)]
		public string inquilino { get; set; }

		[Parameter("address", "propietario", 2, true)]
		public string propietario { get; set; }

		[Parameter("bytes32", "cupon", 3, true)]
		public virtual byte[] cupon { get; set; }

		[Parameter("uint", "monto", 4, false)]
		public BigInteger monto { get; set; }
	}
}
