﻿using AutoMapper.QueryableExtensions;
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
			return await _context.RealEstates.Where(rs => rs.ID==id).Include(r => r.images).FirstAsync();

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

		public async Task<IEnumerable<RealEstate>> Getfiltered(RealEstateFilter request)
		{
			return await GetWithFilterquery(request).Include(r => r.images).ToListAsync();

		}


		private IQueryable<RealEstate> GetWithFilterquery(RealEstateFilter request)
		{
			var query = _context.RealEstates.AsQueryable();

			query = AddAreaFiler(request, query);

			query = AddMaxPriceFiler(request, query);

			query = AddBathroomFilter(request, query);
			query = AddRoomsFilter(request, query);
			query = AddLocalidadFiter(request, query);

			query = AddGarageFilter(request, query);

			return query;
		}

		private static IQueryable<RealEstate> AddGarageFilter(RealEstateFilter request, IQueryable<RealEstate> query)
		{
			if (request.Garage != null)
			{
				query = query.Where(x => x.Localidad.Equals(request.Garage));
			}

			return query;
		}

		private static IQueryable<RealEstate> AddLocalidadFiter(RealEstateFilter request, IQueryable<RealEstate> query)
		{
			if (!string.IsNullOrEmpty(request.Localidad))
			{
				query = query.Where(x => x.Localidad.Equals(request.Localidad));
			}

			return query;
		}

		private static IQueryable<RealEstate> AddRoomsFilter(RealEstateFilter request, IQueryable<RealEstate> query)
		{
			if (request.MinRooms != null || request.MaxRooms != null)
			{
				if (request.MinRooms != null && request.MaxRooms != null)
				{
					query = query.Where(x => x.Rooms >= request.MinRooms && x.Rooms <= request.MaxRooms);
				}
				else
				{
					if (request.MinRooms == null && request.MaxRooms != null)
					{
						query = query.Where(x => x.Rooms <= request.MaxRooms);
					}
					else
					{
						if (request.MinRooms != null && request.MaxRooms == null)
						{
							query = query.Where(x => x.Rooms >= request.MinRooms);
						}
					}
				}

			}

			return query;
		}

		private static IQueryable<RealEstate> AddBathroomFilter(RealEstateFilter request, IQueryable<RealEstate> query)
		{
			if (request.Minbathrooms != null || request.MaxBathrooms != null)
			{
				if (request.Minbathrooms != null && request.MaxBathrooms != null)
				{
					query = query.Where(x => x.BathRoomQty >= request.Minbathrooms && x.BathRoomQty <= request.MaxBathrooms);
				}
				else
				{
					if (request.MinPrice == null && request.MaxPrice != null)
					{
						query = query.Where(x => x.BathRoomQty <= request.MaxBathrooms);
					}
					else
					{
						if (request.MinPrice != null && request.MaxPrice == null)
						{
							query = query.Where(x => x.BathRoomQty >= request.Minbathrooms);
						}
					}
				}

			}

			return query;
		}

		private static IQueryable<RealEstate> AddMaxPriceFiler(RealEstateFilter request, IQueryable<RealEstate> query)
		{
			if (request.MinPrice != null || request.MaxPrice != null)
			{
				if (request.MinPrice != null && request.MaxPrice != null)
				{
					query = query.Where(x => x.RentFee >= request.MinPrice && x.RentFee <= request.MaxPrice);
				}
				else
				{
					if (request.MinPrice == null && request.MaxPrice != null)
					{
						query = query.Where(x => x.RentFee <= request.MaxPrice);
					}
					else
					{
						if (request.MinPrice != null && request.MaxPrice == null)
						{
							query = query.Where(x => x.RentFee >= request.MinPrice);
						}
					}
				}

			}

			return query;
		}

		private static IQueryable<RealEstate> AddAreaFiler(RealEstateFilter request, IQueryable<RealEstate> query)
		{
			if (request.MinArea != null || request.MaxArea != null)
			{
				if (request.MinArea != null && request.MaxArea != null)
				{
					query = query.Where(x => x.SqMtrs >= request.MinArea && x.SqMtrs <= request.MaxArea);
				}
				else
				{
					if (request.MinArea == null && request.MaxArea != null)
					{
						query = query.Where(x => x.SqMtrs <= request.MaxArea);
					}
					else
					{
						if (request.MinArea != null && request.MaxArea == null)
						{
							query = query.Where(x => x.SqMtrs >= request.MinArea);
						}
					}
				}

			}

			return query;
		}
	}
}
