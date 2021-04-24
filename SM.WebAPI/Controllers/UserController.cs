using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.Bll;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
using SM.Helper;
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
		private IBllUser _repository;
		public UserController(IBllUser BllUser)
		{
			_repository = BllUser;
		}


		[HttpGet]
		[Route("GetAll")]
		public Task<IEnumerable<DTOUser>> GetAll()
		{
			return _repository.GetAll();
		}

		[HttpPost("register")]
		public async Task<ActionResult> Register(DTOUser userdto)
		{
			try
			{
				User user = await _repository.Register(userdto);
				return Ok(user);
			}
			catch (Exception e)
			{

				return BadRequest("Already registered");
			}


		}

		[HttpPost("logout")]
		public IActionResult Logout()
		{
			Response.Cookies.Delete("jwt");
			return Ok("Succesfully disconected");
		}

		[HttpPost("login")]
		public IActionResult Login(DTOLogin userdto)
		{
			var id = _repository.Login(userdto);
			if (id == -1)
			{
				return BadRequest();
			}
			else
			{
				var jwt = JWTService.Generate(id);
				Response.Cookies.Append("jwt", jwt, new CookieOptions
				{
					HttpOnly = true
				});
				return Ok("success");
			}


		}

	}
}
