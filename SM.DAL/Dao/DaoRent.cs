using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Dao
{
	public class DaoRent : IDaoRent
	{
		public  Task<RentContract> RentRealEstate(RentContract rent)
		{
			throw new NotImplementedException();
		}
	}
}
