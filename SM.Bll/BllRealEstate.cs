using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winista.Mime;

namespace SM.Bll
{
	public class BllRealEstate : IBllRealEstate
	{

		private IDaoRealEstate _DaoRealEstate;
		private readonly IMapper _mapper;
		public BllRealEstate(IDaoRealEstate daoRealEstate, IMapper mapper)
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
			var realEstates = await _DaoRealEstate.GetFilteredMetros(from, to);
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

		public async Task<DTOAddRealEstate> UpdateRealEstate(DTOAddRealEstate dto, string email)
		{
			var propDDBB = await _DaoRealEstate.GetPropertyByIDAsync(dto.ID.Value);
			RealEstate updatedProp = null;


			if (ValidateProp(propDDBB, email))
			{
				updatedProp = _mapper.Map<RealEstate>(dto);
				updatedProp.Address = propDDBB.Address;
				updatedProp.Localidad = propDDBB.Localidad;
				updatedProp.Provincia = propDDBB.Provincia;
				await _DaoRealEstate.UpdateRealEstate(updatedProp);

			}

			return dto;
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
			var prop = await _DaoRealEstate.GetPropertyByIDAsync(id);
			DTOShowRealEstate dto = null;
			try
			{
				if (ValidateProp(prop, email))
				{
					await _DaoRealEstate.DeleteProp(prop);
					dto = _mapper.Map<DTOShowRealEstate>(prop);
				}


			}
			catch (InvalidOperationException) { return dto; }


			return dto;
		}


		private bool ValidateProp(RealEstate prop, string email)
		{
			bool isValid = false;

			if (prop != null)
			{
				if (prop.User.Email == email && prop.Available)
				{
					isValid = true;
				}
			}

			return isValid;
		}


		public string validateImage(IFormFile file)
		{

			var mimeTypes = new MimeTypes();
			var mimeType1 = mimeTypes.GetMimeType(ConvertToBytes(file));

			if (mimeType1==null)
			{
				return "Archivo no valido";
			}

			if (file.Length > 10485760)
				return "El archivo supera los 10MB";

			if (mimeType1.SubType != "jpg" && mimeType1.SubType != "png" && mimeType1.SubType != "jpeg")
				return "Solo se permiten archivos JPG,JPEG y PNG";



			return null;
		}

		private byte[] ConvertToBytes(IFormFile image)
		{
			byte[] CoverImageBytes = null;
			BinaryReader reader = new BinaryReader(image.OpenReadStream());
			CoverImageBytes = reader.ReadBytes((int)image.Length);
			return CoverImageBytes;
		}
	}
}
