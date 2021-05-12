using Microsoft.AspNetCore.Mvc;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Dao_interfaces
{
	public interface IDaoUser
	{
		Task<User> Register(User user);
		User GetByDNI(int id);
		User GetByEmail(string email);
		Task<User> Update(User dto);
		Task Delete(int id);
		Task<IEnumerable<User>>GetAll();
		Task<User> GetUserByID(int id);

	}
}
