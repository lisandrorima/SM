using SM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Dao_interfaces
{
	public interface IDaoRent
	{
		Task<RentContract> RentRealEstate(RentContract rent);
	}
}
