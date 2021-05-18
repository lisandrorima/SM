using SM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Dao_interfaces
{
	public interface IDaoRealEstate
	{
		Task<IEnumerable<RealEstate>> GetAllAsync();

		Task<IEnumerable<RealEstate>> GetRealEstatesByOwnerAsync(string email);

		Task<RealEstate> AddRealEstate(RealEstate realEstate);
		Task<IEnumerable<RealEstate>> GetFilteredMetros(int from, int to);
		Task<IEnumerable<RealEstate>> GetPropertyDetails(int id);

		Task<RealEstate> GetPropertyByIDAsync(int id);

		Task<bool> DisableRealEstate(RealEstate realEstate);

		Task<IEnumerable<RealEstate>> GetRelated(string localidad);
		Task<IEnumerable<RealEstate>> GetRelated();

		

	}
}
