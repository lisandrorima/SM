using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SM.Bll;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SM.WebAPI.Controllers
{

	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class RentController : ControllerBase
	{

		private IBllRent _Rentrepository;
		
		public RentController(IBllRent BllRent)
		{
			_Rentrepository = BllRent;

		}


		[Authorize]
		[HttpPost]
		[Route("Rent")]
		public async  Task<IActionResult> RentProperty(DTORentProperty dto)
		{

			string email = GetEmailByHttpContext();
			var a = await _Rentrepository.Rent(dto,email);
			
			return Ok();
		}

		private string GetEmailByHttpContext()
		{
			string email = "";
			var identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity != null)
			{
				email = identity.FindFirst("UserEmail").Value;
			}
			return email;
		}
	}
}
