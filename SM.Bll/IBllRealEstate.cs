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
		Task<RealEstate> AddRealEstate(DTOAddRealEstate realEstatedto);
		Task<RealEstate> UpdateRealEstate(DTOAddRealEstate dto, int realEstateID);
		Task<IEnumerable<DTOShowRealEstate>> GetAll();
		Task<IEnumerable<DTOShowRealEstate>> GetRealEstatesByOwner(string email);
		Task<DTOShowRealEstate> GetRealEstateByID(int id);
		Task<IEnumerable<DTOShowRealEstate>> GetByMetros(int from, int to);
		Task<IEnumerable<DTOShowFullDetail>> GetFullDetailsByID(int id);

	}
}