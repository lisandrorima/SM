using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Bll
{
	public interface IBllRealEstate
	{
		Task<RealEstate> AddRealEstate(DTOAddRealEstate realEstatedto, string email);
		Task<DTOAddRealEstate> UpdateRealEstate(DTOAddRealEstate dto, string email);
		Task<IEnumerable<DTOShowRealEstate>> GetAll();
		Task<IEnumerable<DTOShowRealEstate>> GetRealEstatesByOwner(string email);
		Task<DTOShowRealEstate> GetRealEstateByID(int id);
		Task<IEnumerable<DTOShowRealEstate>> GetByMetros(int from, int to);
		Task<IEnumerable<DTOShowFullDetail>> GetFullDetailsByID(int id);
		Task<IEnumerable<DTOShowRealEstate>> GetRelacionado(string ciudad);
		Task<IEnumerable<DTOShowRealEstate>> Getfiltered(RealEstateFilter realEstateFilter);
		Task<DTOShowRealEstate> DeletePropiedad(int id, string email);

		public string validateImage(IFormFile file);
		Task<IEnumerable<DTOProvincia>> GetProvincias();
	}
}