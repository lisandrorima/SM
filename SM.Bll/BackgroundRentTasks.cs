using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Bll
{
	public class BackgroundRentTasks : BackgroundService
	{
		//private readonly IBllRent _bllRent;
		private readonly IServiceScopeFactory _scopeFactory;
		public BackgroundRentTasks(IServiceScopeFactory scopeFactory)
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
					await Task.Delay(Convert.ToInt32(TimeSpan.FromHours(12).TotalMilliseconds), stoppingToken);
				}
			}


		}
	}
}
