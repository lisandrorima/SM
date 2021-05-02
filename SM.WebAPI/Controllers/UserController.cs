using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SM.Bll;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using SM.Entities;
using SM.Helper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
		public IActionResult Register(DTOUser userdto)
		{
			try
			{
				User user =  _repository.Register(userdto).Result;
				return Ok("User " + user.PersonalID + " was created");
			}
			catch (Exception)
			{

				return BadRequest("Already registered");
			}


		}
		[HttpPost("logout")]
		public IActionResult Logout()
		{
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
					HttpOnly = true,
					SameSite= SameSiteMode.None
				});

				return Ok("success");
			}

		}

		[HttpPut("modificardatos")] 
			public IActionResult Update(DTOUser userdto)
		{
			try
			{
				
				int userId = Convert.ToInt32(ValidateJWT().Issuer);
				if(_repository.Update(userdto, userId).Result.ID != null)
				{
					return Ok("success");
				}
				else
				{
					return Unauthorized("You don't have the correct privileges to perform this action");
				}
					
			}
			catch(Exception)
			{
				return BadRequest("Exception");

			}


		}

		private JwtSecurityToken ValidateJWT()
		{
			var jwt = Request.Cookies["jwt"];
			var token = JWTService.Verify(jwt);
			return token;
		}

	}
}
