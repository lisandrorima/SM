using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using SM.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SM.Entities;
using System.Numerics;

namespace SM.SmartContractInteraction
{
	public class EventListenerWeb3 : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public EventListenerWeb3(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();


            while (!stoppingToken.IsCancellationRequested)
            {
                  

                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var bllRent = scope.ServiceProvider.GetRequiredService<IBllRent>();
                        var result = await bllRent.GetAllValidCouponsWithRealEstate();
                        var pagos =await GetAllEvents();
                        var CuponesAModificar = ContrastacionDePagos(result, pagos);
                        await bllRent.ValidarCupones(CuponesAModificar);

                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    }
                    await Task.Delay(Convert.ToInt32(TimeSpan.FromMinutes(20).TotalMilliseconds), stoppingToken);
                }
            }


        }

        private static async Task<List<EventoPago>> GetAllEvents()
		{
			List<EventoPago> pagos = new List<EventoPago>();


			//url proyecto, en este caso localhost ganache
			var url = "HTTP://127.0.0.1:7545";

			//ABI del contrato, una vez que este deployado cambiar a la ultima version
			var abi = @"[
    {
      'inputs': [],
      'stateMutability': 'nonpayable',
      'type': 'constructor'
    },
    {
      'anonymous': false,
      'inputs': [
        {
          'indexed': true,
          'internalType': 'address',
          'name': 'inquilino',
          'type': 'address'
        },
        {
          'indexed': true,
          'internalType': 'address',
          'name': 'propietario',
          'type': 'address'
        },
        {
          'indexed': true,
          'internalType': 'bytes32',
          'name': 'cupon',
          'type': 'bytes32'
        },
        {
          'indexed': false,
          'internalType': 'uint256',
          'name': 'monto',
          'type': 'uint256'
        }
      ],
      'name': 'pagoRealizado',
      'type': 'event'
    },
    {
      'inputs': [
        {
          'internalType': 'address',
          'name': 'propietario',
          'type': 'address'
        },
        {
          'internalType': 'bytes32',
          'name': 'cupon',
          'type': 'bytes32'
        }
      ],
      'name': 'PayRent',
      'outputs': [],
      'stateMutability': 'payable',
      'type': 'function',
      'payable': true
    },
    {
      'inputs': [
        {
          'internalType': 'address',
          'name': 'propietario',
          'type': 'address'
        }
      ],
      'name': 'sendViaCall',
      'outputs': [],
      'stateMutability': 'payable',
      'type': 'function',
      'payable': true
    }
  ]";

			//address del contrato, una vez que este deployado cambiar a la ultima version
			var contractAddress = "0x4Cf5D2c09a14B45a1050C9536991d0460DccCE75";

			//crea instncia de web3 con esa red
			var web3 = new Web3(url);

			//genera una instancia de contrato
			var contract = web3.Eth.GetContract(abi, contractAddress);


			//Busca los eventos que coincidan con la Key esta key esta definida en el smartcontract y es el nombre del evento
			var generacionContratoEventLog = web3.Eth.GetEvent<PagoRealizadoEventDTO>(contractAddress);


			//Busca los eventos desde el primer bloque al ultimo
			var filterInput = generacionContratoEventLog.CreateFilterInput(BlockParameter.CreateEarliest(), BlockParameter.CreateLatest());

			//filtra todos los cambios que se produjeron
			var logs = await generacionContratoEventLog.GetAllChanges(filterInput);

			DecodeLog(pagos, logs);

			return pagos;
		}

		private static void DecodeLog(List<EventoPago> pagos, List<Nethereum.Contracts.EventLog<PagoRealizadoEventDTO>> logs)
		{
			foreach (var item in logs)
			{
				EventoPago pago = new EventoPago();

				pago.cupon = ByteArrayToString(item.Event.cupon);
				pago.inquilino = item.Event.inquilino;
				pago.propietario = item.Event.propietario;
				pago.monto = item.Event.monto;

				pagos.Add(pago);
			}
		}

        private static List<DTOpaymentVerification> ContrastacionDePagos(IEnumerable<DTOpaymentVerification> result, List<EventoPago> pagos)
		{
            //Contrastacion en milli eth
            List<DTOpaymentVerification> contrastados = new List<DTOpaymentVerification>();

            foreach (var cupon in result)
            {
                var pagoCupon= pagos.Find(x => x.cupon == cupon.Cupon);
				if (pagoCupon!=null)
				{
					if (pagoCupon.inquilino.ToUpper()==cupon.Inquilino.ToUpper() && pagoCupon.propietario.ToUpper()==cupon.Propietario.ToUpper() && pagoCupon.monto== (BigInteger.Parse(cupon.Monto.ToString())*1000000000000000))
					{
                        contrastados.Add(cupon);
					}
				}
			}


            return contrastados;

        }
		private static string ByteArrayToString(byte[] array_datos)
        {
            StringBuilder cadena = new StringBuilder(array_datos.Length * 2);
            foreach (byte b in array_datos)
            {
                cadena.AppendFormat("{0:x2}", b);
            }
            return "0x" + cadena.ToString();
        }
    }
}

