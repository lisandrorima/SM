using Microsoft.AspNetCore.Mvc;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Bll
{
	public interface IBllUser
	{
		Task<DTOUser> Register(DTOUser userdto);
		IActionResult Logout();
		IActionResult Login(DTOUser userdto);
		Task Update(DTOUser dto);
		Task Delete(int id);
		Task<IEnumerable<DTOUser>> GetAll();
	}
}
