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
using System.Security.Claims;
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



		[AllowAnonymous]
		[HttpPost("register")]
		public IActionResult Register(DTOUser userdto)
		{
			try
			{
				User user = _repository.Register(userdto).Result;
				return Ok("User " + user.PersonalID + " was created");
			}
			catch (Exception)
			{

				return BadRequest("Already registered");
			}


		}

		//FALTA, Hay que hacerlo desde el browser o poner tiempo de expiracion mas corto y mandar refresh token
		[HttpPost("logout")]
		public IActionResult Logout()
		{

			return Ok("Succesfully disconected");
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public IActionResult Login(DTOLogin userdto)
		{
			var user = _repository.Login(userdto);
			
			if (user.Email == null)
			{
				return BadRequest();
			}
			else
			{
				
				var jwt = GenerateJWT(user);
				Response.Headers.Add("Authorization", jwt);
				return Ok(jwt);
			}

		}

		[Authorize]
		[HttpPut("modificardatos")] 
			public IActionResult Update(DTOUser userdto)
		{
			try
			{
				string email = "";
				var identity = HttpContext.User.Identity as ClaimsIdentity;
				if (identity != null)
				{
					email =identity.FindFirst("UserEmail").Value;
				}
			

				if(_repository.Update(userdto, email).Result.ID != null)
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


		private string GenerateJWT(DTOUser user)
		{
			JWTService validator = new JWTService();
			string securityToken = validator.Generate(user);

			return securityToken;
		}
	}
}
