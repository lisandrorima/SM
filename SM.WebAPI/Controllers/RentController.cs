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
			try
			{
				await _Rentrepository.Rent(dto, email);
				return Ok();

			}
			catch
			{
				return BadRequest();
			}


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

		[Authorize]
		[HttpGet]
		[Route("MisCupones")]

		public async Task<IActionResult> GetCuponesAsync()
		{

			string email = GetEmailByHttpContext();
			try
			{
				return Ok(await _Rentrepository.MisCupones(email));
			
			}
			catch
			{
				return BadRequest();

			}


		}

		[Authorize]
		[HttpGet]
		[Route("MisCupones2")]

		public async Task<IActionResult> GetCuponesAsync2()
		{

			string email = GetEmailByHttpContext();
			try
			{
				return Ok(await _Rentrepository.GetDTOContractWithCupons(email));

			}
			catch
			{
				return BadRequest();

			}

		}

		[Authorize]
		[HttpGet]
		[Route("ownercoupons")]

		public async Task<IActionResult> GetOwnerCupons()
		{

			string email = GetEmailByHttpContext();
			try
			{
				return Ok(await _Rentrepository.GetDTOContractWithCuponsOwner(email));

			}
			catch
			{
				return BadRequest();

			}

		}

	}
}
