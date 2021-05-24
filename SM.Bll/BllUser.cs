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
		private readonly IMapper _mapper;
		public BllUser(IDaoUser DaoUser, IMapper mapper)
		{
			_DaoUser = DaoUser;
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




		public async Task<DTOUser> Login(DTOLogin userdto)
		{

			var user = await _DaoUser.GetByEmail(userdto.Email);
			var dto = new DTOUser();

			if (user != null)
			{
				if (BCrypt.Net.BCrypt.Verify(userdto.Password, user.Password) && userdto.Email==user.Email)
				{
					dto = _mapper.Map<DTOUser>(user);
				}
			}


			return dto;




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

		public async Task<User> Update(DTOUser dto, string email)
		{
			var userFromDTO = _mapper.Map<User>(dto);
			User userFromDTOValidate = await _DaoUser.GetByEmail(userFromDTO.Email);
			User fromDDBBwithEMail = await _DaoUser.GetByEmail(dto.Email);
			

			
			if (fromDDBBwithEMail != null)
			{
				if (fromDDBBwithEMail.ID == userFromDTOValidate.ID)
				{
					userFromDTO.ID = userFromDTOValidate.ID;


					return await _DaoUser.Update(userFromDTO);
				}
			}

			return new User();
		}


		public  DTOUser GetByEmail(string email)
		{

			var user =  _DaoUser.GetByEmail(email);
			var dto = new DTOUser();

			if (user != null)
			{ 
				dto = _mapper.Map<DTOUser>(user);
			}


			return dto;

		}
	}
}
