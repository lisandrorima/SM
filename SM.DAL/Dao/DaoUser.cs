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


		
		public async Task<User> Register(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			return user;
		}

		public Task Update(DTOUser dto)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			return  await _context.Users.ToListAsync();

		}

		public  User GetByDNI(int id)
		{

			return  _context.Users.Where(u => u.PersonalID == id).FirstOrDefault();
			
		}

		public User GetByEmail(string email)
		{
			return _context.Users.Where(u => u.Email == email).FirstOrDefault();
		}
	}
}
