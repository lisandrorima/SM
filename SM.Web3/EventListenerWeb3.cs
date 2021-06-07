using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nethereum.RPC.Eth.DTOs;
using SM.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Web3Proj
{
	public class EventListenerWeb3: BackgroundService
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
						var result = await bllRent.UpdateContractsAndCoupons();
					}
					catch (Exception)
					{
						await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
					}
					await Task.Delay(Convert.ToInt32(TimeSpan.FromHours(1).TotalMilliseconds), stoppingToken);
				}
			}


		}

		private async List<PagoRealizadoEventDTO> GetAllEvents()
		{
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
            var contractAddress = "0xB7b3CEcD52f1bA4ccD20F7C7dF53a8e5462a6bca";

            //crea instncia de web3 con esa red
            var web3 = new Web3(url);

            //genera una instancia de contrato
            var contract = web3.Eth.GetContract(abi, contractAddress);

            //Balance de una billetera
            var balance = await web3.Eth.GetBalance.SendRequestAsync("0x4Cf5D2c09a14B45a1050C9536991d0460DccCE75");
            Console.WriteLine(balance);


            //Busca los eventos que coincidan con la Key esta key esta definida en el smartcontract y es el nombre del evento
            var generacionContratoEventLog = web3.Eth.GetEvent<PagoRealizadoEventDTO>();


            //Busca los eventos desde el primer bloque al ultimo
            var filterInput = generacionContratoEventLog.CreateFilterInput(BlockParameter.CreateEarliest(), BlockParameter.CreateLatest());

            //filtra todos los cambios que se produjeron
            var logs = await generacionContratoEventLog.GetAllChanges(filterInput);
        }
	}
}
