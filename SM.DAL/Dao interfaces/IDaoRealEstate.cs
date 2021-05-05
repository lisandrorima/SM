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

		Task<IEnumerable<RealEstate>> GetRealEstatesByOwnerAsync(int id);

		Task<RealEstate> AddRealEstate(RealEstate realEstate);
		Task<IEnumerable<RealEstate>> GetFilteredMetros(int from, int to);
		Task<IEnumerable<RealEstate>> GetPropertyDetails(int id);
	}
}
