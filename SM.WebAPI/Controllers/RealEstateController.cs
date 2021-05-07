﻿using Microsoft.AspNetCore.Mvc;
using SM.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SM.Entities;
using SM.Helper;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SM.WebAPI.Controllers
{

	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class RealEstateController : ControllerBase
	{
		private IBllRealEstate _repository;
		public RealEstateController(IBllRealEstate BllRealEstate)
		{
			_repository = BllRealEstate;
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("getRealEstates")]
		public Task<IEnumerable<DTOShowRealEstate>> GetAll()
		{
			return _repository.GetAll();
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("areaFilter")]
		public Task<IEnumerable<DTOShowRealEstate>> GetbyMetros(int from, int to)
		{
			return _repository.GetByMetros(from,to);
		}

		
		[Authorize]
		[HttpGet]
		[Route("getMyRealEstates")]
		public Task<IEnumerable<DTOShowRealEstate>> GetMyRealEstates()
		{


			string email = "";
			var identity = HttpContext.User.Identity as ClaimsIdentity;
			if (identity != null)
			{
				email = identity.FindFirst("UserEmail").Value;
			}
			

			return _repository.GetRealEstatesByOwner(email);
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("GetPropertyDetail")]
		public  Task<IEnumerable<DTOShowFullDetail>> GetPropertyDetail(int id)
		{
			return _repository.GetFullDetailsByID(id);

		}

	}
}
