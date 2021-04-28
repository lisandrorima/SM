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
	public class DaoRealEstate : IDaoRealEstate

	{
		private readonly SmartPropDbContext _context;

		public DaoRealEstate(SmartPropDbContext dbcontext)
		{
			_context = dbcontext;
		}

		public async Task<RealEstate> AddRealEstate(RealEstate realEstate)
		{
			await _context.RealEstates.AddAsync(realEstate);
			await _context.SaveChangesAsync();
			return realEstate;
		}

		public async Task<IEnumerable<RealEstate>> GetAllAsync()
		{
			return await _context.RealEstates.ToListAsync();

		}

		public async Task<IEnumerable<RealEstate>> GetRealEstatesByOwnerAsync(int id)
		{
			return await _context.RealEstates.Where(c=> c.User.ID ==id).ToListAsync();
		}
	}
}
