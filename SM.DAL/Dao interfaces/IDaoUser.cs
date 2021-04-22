using Microsoft.AspNetCore.Mvc;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SM.DAL.Dao_interfaces
{
	public interface IDaoUser
	{
		Task<DTOUser> Register(DTOUser userdto);
		IActionResult Logout();
		IActionResult Login(DTOUser userdto);
		Task Update(DTOUser dto);
		Task Delete(int id);
		Task<IEnumerable<User>>GetAll();

	}
}
