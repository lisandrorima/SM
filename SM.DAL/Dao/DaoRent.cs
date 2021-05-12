using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Dao
{
	public class DaoRent : IDaoRent
	{

		private readonly SmartPropDbContext _context;

		public DaoRent(SmartPropDbContext dbcontext)
		{
			_context = dbcontext;
		}

		public  async Task<RentContract> RentRealEstate(RentContract rent)
		{
			_context.RentContracts.Add(rent);
			await _context.SaveChangesAsync();
			return rent;
		}
	}
}
