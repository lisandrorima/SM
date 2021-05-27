using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
		private readonly IMapper _mapper;
		public BllRealEstate(IDaoRealEstate daoRealEstate,  IMapper mapper)
		{
			_DaoRealEstate = daoRealEstate;
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

		public async Task<IEnumerable<DTOShowRealEstate>> GetRealEstatesByOwner(string email)
		{

			var realEstates = await _DaoRealEstate.GetRealEstatesByOwnerAsync(email);
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

		public async Task<DTOShowRealEstate> GetRealEstateByID(int id)
		{
			DTOShowRealEstate dto = new();
			dto = _mapper.Map<DTOShowRealEstate>(await _DaoRealEstate.GetPropertyByIDAsync(id));
			return dto;
		}

		public async Task<IEnumerable<DTOShowRealEstate>> GetRelacionado(string localidad)
		{
			IEnumerable<RealEstate> realEstates;
			if (localidad == null)
			{
				realEstates = await _DaoRealEstate.GetRelated();
			}
			else
			{
				realEstates = await _DaoRealEstate.GetRelated(localidad);
			}
			

			List<DTOShowRealEstate> dtos = new List<DTOShowRealEstate>();

			foreach (var realEstate in realEstates)
			{

				dtos.Add(_mapper.Map<DTOShowRealEstate>(realEstate));
			}

			return dtos;


		}

		public async Task<IEnumerable<DTOShowRealEstate>> Getfiltered(RealEstateFilter realEstateFilter)
		{
		

			var realEstates = await _DaoRealEstate.Getfiltered(realEstateFilter);
			List<DTOShowRealEstate> dtos = new List<DTOShowRealEstate>();

			foreach (var realEstate in realEstates)
			{

				dtos.Add(_mapper.Map<DTOShowRealEstate>(realEstate));
			}

			return dtos;
		}




		public async Task<DTOShowRealEstate> DeletePropiedad(int id, string email)
		{
			var prop= await _DaoRealEstate.GetPropertyByIDAsync(id);
			DTOShowRealEstate dto = null ;
			try
			{
				if (ValidateProp(prop,email))
				{
						await _DaoRealEstate.DeleteProp(prop);
						dto = _mapper.Map<DTOShowRealEstate>(prop);
				}
				

			}
			catch (InvalidOperationException ){return dto;}


			return dto;
		}


		private bool ValidateProp(RealEstate prop, string email)
		{
			bool isValid = false;

			if (prop != null)
			{
				if (prop.User.Email == email && !prop.Available)
				{
					isValid = true;
				}
			}

			return isValid;
		}
	}
}
