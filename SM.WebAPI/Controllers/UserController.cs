using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.Bll;
using SM.DAL.Dao_interfaces;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SM.WebAPI.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private   IBllUser _repository;
		public UserController(IBllUser BllUser)
		{
			_repository = BllUser;
		}


		[HttpGet]
		[Route("GetAll")]
		public  Task<IEnumerable<DTOUser>> GetAll()
		{
			return    _repository.GetAll();
		}

	}
}
