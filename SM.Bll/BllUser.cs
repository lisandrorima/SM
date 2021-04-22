using Microsoft.AspNetCore.Mvc;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SM.Bll
{
	public class BllUser : IBllUser
	{

		private  IDaoUser _DaoUser;
		private readonly SmartPropDbContext _db;
		public BllUser(IDaoUser DaoUser, SmartPropDbContext db)
		{
			_DaoUser = DaoUser;
			_db = db;
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<DTOUser>> GetAll()
		{
			var users = await _DaoUser.GetAll();
			var dtos = new List<DTOUser>();

			dtos.AddRange(users. Select(user => new DTOUser()
			{
				PersonalID = user.PersonalID,
				Email = user.Email,
				Name = user.Name,
				Surname = user.Surname,
				Password = user.Password,
				WalletAddress = user.WalletAddress,

			}).ToList());

			return  dtos;

		}

		public IActionResult Login(DTOUser userdto)
		{
			throw new NotImplementedException();
		}

		public IActionResult Logout()
		{
			throw new NotImplementedException();
		}

		public Task<DTOUser> Register(DTOUser userdto)
		{
			throw new NotImplementedException();
		}

		public Task Update(DTOUser dto)
		{
			throw new NotImplementedException();
		}
	}
}
