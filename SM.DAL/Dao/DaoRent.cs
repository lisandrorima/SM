using Microsoft.EntityFrameworkCore;
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

		public async Task<IEnumerable<CuponDePago>> AddCupones(List<CuponDePago> cupones)
		{
			foreach (var cupon in cupones)
			{
				_context.CuponDePagos.Add(cupon);
			}
			await _context.SaveChangesAsync();
			return cupones;
		}

		public async Task<IEnumerable<CuponDePago>> GetCupones(User user)
		{
			var contratos= await _context.RentContracts.Where(c => c.Tenant == user).ToListAsync();


			return await _context.CuponDePagos.Where(r => contratos.Contains(r.rentContract)).ToListAsync();
		}

		public async Task<IEnumerable<RentContract>> GetContractWithCupons(User user)
		{
			return await _context.RentContracts.Where(c => c.Tenant == user).Include(r=>r.cupones).Include(m=>m.RealEstate).ToListAsync();
		}


		public  async Task<RentContract> RentRealEstate(RentContract rent)
		{
			_context.RentContracts.Add(rent);
			await _context.SaveChangesAsync();
			return rent;
		}
	}
}
