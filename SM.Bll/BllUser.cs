using Microsoft.AspNetCore.Mvc;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;

namespace SM.Bll
{
	public class BllUser : IBllUser
	{

		private IDaoUser _DaoUser;
		private readonly SmartPropDbContext _db;
		private readonly IMapper _mapper;
		public BllUser(IDaoUser DaoUser, SmartPropDbContext db, IMapper mapper)
		{
			_DaoUser = DaoUser;
			_db = db;
			_mapper = mapper;
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<DTOUser>> GetAll()
		{
			var users = await _DaoUser.GetAll();
			var dtos = new List<DTOUser>();

			dtos.AddRange(users.Select(user => new DTOUser()
			{
				PersonalID = user.PersonalID,
				Email = user.Email,
				Name = user.Name,
				Surname = user.Surname,
				Password = user.Password,
				WalletAddress = user.WalletAddress,

			}).ToList());

			return dtos;

		}

		public int Login(DTOLogin userdto)
		{
			var id = -1;
			try
			{
				var user = _DaoUser.GetByEmail(userdto.Email);
				

				if (user != null)
				{
					if (BCrypt.Net.BCrypt.Verify(userdto.Password, user.Password))
					{
						id = user.ID.Value;
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return id;




		}

		public async Task<User> Register(DTOUser userdto)
		{


			var user = _mapper.Map<User>(userdto);
			user.Password = BCrypt.Net.BCrypt.HashPassword(userdto.Password);
			try
			{

				user = await _DaoUser.Register(user);

			}
			catch (Exception)
			{
				throw;
			}

			return user;

		}

		public Task Update(DTOUser dto)
		{
			throw new NotImplementedException();
		}
	}
}
