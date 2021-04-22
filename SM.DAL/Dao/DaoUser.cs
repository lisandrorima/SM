using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Dao
{
	public class DaoUser : IDaoUser
	{
		private readonly SmartPropDbContext _context;

		public DaoUser(SmartPropDbContext dbcontext)
		{
			_context = dbcontext;
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}
		/*
		public async Task<List<DTOUser>> GetAll()
		{
			var dtos = new List<DTOUser>();

			var users =  await _context.Users.ToListAsync();

			dtos.AddRange(users.Select(user => new DTOUser()
			{
				PersonalID =user.PersonalID,
				Email = user.Email,
				Name = user.Name,
				Surname = user.Surname,
				Password = user.Password,
				WalletAddress = user.WalletAddress,

			}).ToList());

			return dtos;
		}
	*/
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

		public async Task<IEnumerable<User>> GetAll()
		{
			return  await _context.Users.ToListAsync();

		}

	}
}
