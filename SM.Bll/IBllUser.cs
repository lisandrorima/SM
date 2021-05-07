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
	public interface IBllUser
	{
		Task<User> Register(DTOUser userdto);
		DTOUser Login(DTOLogin userdto);
		Task<User> Update(DTOUser dto, string email);
		Task Delete(int id);
		Task<IEnumerable<DTOUser>> GetAll();
	}
}
