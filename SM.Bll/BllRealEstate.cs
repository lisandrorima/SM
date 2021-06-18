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
		private IDaoUser _DaoUser;
		private readonly IMapper _mapper;
		public BllRealEstate(IDaoRealEstate daoRealEstate, IMapper mapper, IDaoUser daoUser)
		{
			_DaoRealEstate = daoRealEstate;
			_mapper = mapper;
			_DaoUser = daoUser;
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

		public async Task<RealEstate> AddRealEstate(DTOAddRealEstate dto, string email)
		{

			RealEstate ReToPersist = _mapper.Map<RealEstate>(dto);
			
			ReToPersist.User =  await _DaoUser.GetByEmail(email);
			var RealEstate= await _DaoRealEstate.AddRealEstate(ReToPersist);

			List<ImagesRealEstate> imagenes = new List<ImagesRealEstate>();

			foreach (var imagen in dto.ImgURL)
			{
				ImagesRealEstate img = new ImagesRealEstate();
				img.ImgURL = imagen.ImgURL;
				img.RealEstate = RealEstate;
				imagenes.Add(img);
			}

			await _DaoRealEstate.AddImages(imagenes);

			if (RealEstate!=null)
			{
				return RealEstate;
			}
			return null;

		}

		public async Task<DTOAddRealEstate> UpdateRealEstate(DTOAddRealEstate dto, string email)
		{
			var propDDBB = await _DaoRealEstate.GetPropertyByIDForValidationAsync(dto.ID.Value);
			RealEstate updatedProp = null;


			if (ValidateProp(propDDBB, email).Result)
			{
				updatedProp = _mapper.Map<RealEstate>(dto);
				updatedProp.Address = propDDBB.Address;
				updatedProp.Localidad = propDDBB.Localidad;
				updatedProp.Provincia = propDDBB.Provincia;

				List<ImagesRealEstate> imagenesFromDDBB = await _DaoRealEstate.GetImagesRealEstatesAsync(updatedProp.ID.Value);
				List<ImagesRealEstate> newImagenes = new List<ImagesRealEstate>();

				if (dto.ImgURL!=null)
				{
					foreach (var imagen in dto.ImgURL)
					{
						ImagesRealEstate img = new ImagesRealEstate();
						img.ImgURL = imagen.ImgURL;
						img.RealEstate = propDDBB;

						

						if (!imagenesFromDDBB.Any(x =>imagen.ImgURL ==x.ImgURL))
						{
							newImagenes.Add(img);
						}
					
						
					}
				}

				if (newImagenes.Count>0)
				{
					await _DaoRealEstate.AddImages(newImagenes);
				}
			
				await _DaoRealEstate.UpdateRealEstate(updatedProp);
				return dto;
			}

			return null;
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

		public async Task<IEnumerable<DTOShowRealEstate>> GetRelacionado(string provincia)
		{
			IEnumerable<RealEstate> realEstates;
			if (provincia == null)
			{
				realEstates = await _DaoRealEstate.GetRelated();
			}
			else
			{
				realEstates = await _DaoRealEstate.GetRelated(provincia);
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
				if (ValidateProp(prop, email).Result)
				{
					prop.IsDeleted = true;
					prop.Available = false;
					await _DaoRealEstate.DeleteProp(prop);
					dto = _mapper.Map<DTOShowRealEstate>(prop);
				}


			}
			catch (InvalidOperationException) { return dto; }


			return dto;
		}


		private async Task<bool> ValidateProp(RealEstate prop, string email)
		{
			bool isValid = false;

			if (prop != null)
			{
				if (prop.User.Email == email && !prop.IsDeleted)

				{
					var contratosValidos = await _DaoRealEstate.getValidContractsForProp(prop);
					if (!contratosValidos.Any())
					{
						isValid = true;
					}
					
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

		public async Task<IEnumerable<DTOProvincia>> GetProvincias()
		{
			var provincias= await _DaoRealEstate.GetProvincias();
			List<DTOProvincia> dtoProvincias = new List<DTOProvincia>();

			foreach (var item in provincias)
			{
				DTOProvincia a = new DTOProvincia();
				a.Nombre = item.Nombre;
				dtoProvincias.Add(a);
			}

			return dtoProvincias;
		}
	}
}
