using AutoMapper;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Bll
{
	public class BllRealEstate : IBllRealEstate
	{

		private IDaoRealEstate _DaoRealEstate;
		private readonly SmartPropDbContext _db;
		private readonly IMapper _mapper;
		public BllRealEstate(IDaoRealEstate daoRealEstate, SmartPropDbContext db, IMapper mapper)
		{
			_DaoRealEstate = daoRealEstate;
			_db = db;
			_mapper = mapper;
		}

		public async Task<IEnumerable<DTOShowRealEstate>> GetAll()
		{
			var realEstates = await _DaoRealEstate.GetAllAsync();
			List<DTOShowRealEstate> dtos = new List<DTOShowRealEstate>();

			foreach (var realEstate in realEstates)
			{ 

				dtos.Add(_mapper.Map<DTOShowRealEstate>(realEstate));
			}

			return dtos;
		}

		public async Task<IEnumerable<DTOShowRealEstate>> GetRealEstatesByOwner(int userId)
		{
			var realEstates = await _DaoRealEstate.GetRealEstatesByOwnerAsync(userId);
			List<DTOShowRealEstate> dtos = new List<DTOShowRealEstate>();

			foreach (var realEstate in realEstates)
			{

				dtos.Add(_mapper.Map<DTOShowRealEstate>(realEstate));
			}

			return dtos;
		}

		public async Task<IEnumerable<DTOShowRealEstate>> GetByMetros(int from, int to)
		{
			var realEstates = await _DaoRealEstate.GetFilteredMetros(from,to);
			List<DTOShowRealEstate> dtos = new List<DTOShowRealEstate>();

			foreach (var realEstate in realEstates)
			{

				dtos.Add(_mapper.Map<DTOShowRealEstate>(realEstate));
			}

			return dtos;
		}

		public Task<RealEstate> AddRealEstate(DTOAddRealEstate realEstatedto)
		{
			throw new NotImplementedException();
		}

		public Task<RealEstate> UpdateRealEstate(DTOAddRealEstate dto, int realEstateID)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<DTOShowFullDetail>> GetFullDetailsByID(int id)
		{
			var realEstates = await _DaoRealEstate.GetPropertyDetails(id);
			List<DTOShowFullDetail> dtos = new List<DTOShowFullDetail>();

			foreach (var realEstate in realEstates)
			{

				dtos.Add(_mapper.Map<DTOShowFullDetail>(realEstate));
			}

			return dtos;
		}

	
	}
}
