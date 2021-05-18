using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
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
			return await _context.RealEstates.Where(R => R.Available).Include(r=>r.images).ToListAsync();

		}

		public async Task<IEnumerable<RealEstate>> GetFilteredMetros(int from, int to)
		{
			return await _context.RealEstates.Where(rs => rs.SqMtrs >=from & rs.SqMtrs<=to).Include(r => r.images).ToListAsync();
		}

		public async Task<RealEstate> GetPropertyByIDAsync(int id)
		{
			return await _context.RealEstates.FindAsync(id);

		}

		public async Task<IEnumerable<RealEstate>> GetPropertyDetails(int id)
		{
			return await _context.RealEstates.Where(c => c.ID == id).Include(r => r.images).Include(u=>u.User).ToListAsync();
		}

		public async Task<IEnumerable<RealEstate>> GetRealEstatesByOwnerAsync(string email)
		{
			return await _context.RealEstates.Where(c=> c.User.Email == email).Include(r => r.images).ToListAsync();
		}

		public async Task<bool> DisableRealEstate(RealEstate realEstate)
		{
			bool success = false;
			var modifRealEstate = await _context.RealEstates.FindAsync(realEstate.ID);
			modifRealEstate.Available = false;
			try
			{
				await _context.SaveChangesAsync();
				success = true;
;
			}
			catch { }
			return success;
			
		}

		public async Task<IEnumerable<RealEstate>> GetRelated(string localidad)
		{
			return await _context.RealEstates.Where(R => R.Available & R.Localidad==localidad).Include(r => r.images).OrderBy(r => Guid.NewGuid()).Take(3).ToListAsync();
		}

		public async Task<IEnumerable<RealEstate>> GetRelated()
		{
			return await _context.RealEstates.Where(R => R.Available).Include(r => r.images).OrderBy(r => Guid.NewGuid()).Take(3).ToListAsync();

		}
	}
}
