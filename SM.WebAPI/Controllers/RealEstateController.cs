using Microsoft.AspNetCore.Mvc;
using SM.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SM.Entities;
using SM.Helper;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

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


		[HttpGet]
		[Route("getRealEstates")]
		public Task<IEnumerable<DTOShowRealEstate>> GetAll()
		{
			return _repository.GetAll();
		}

		[HttpGet]
		[Route("areaFilter")]
		public Task<IEnumerable<DTOShowRealEstate>> GetbyMetros(int from, int to)
		{
			return _repository.GetByMetros(from,to);
		}



		[HttpGet]
		[Route("getMyRealEstates")]
		public Task<IEnumerable<DTOShowRealEstate>> GetMyRealEstates()
		{
			int userId = Convert.ToInt32(ValidateJWT().Issuer);
			return _repository.GetRealEstatesByOwner(userId);
		}

		[HttpGet]
		[Route("GetPropertyDetail")]
		public  Task<IEnumerable<DTOShowFullDetail>> GetPropertyDetail(int id)
		{
			return _repository.GetFullDetailsByID(id);

		}

		private JwtSecurityToken ValidateJWT()
		{
			var jwt = Request.Cookies["jwt"];
			var token = new JwtSecurityToken();

			token = JWTService.Verify(jwt);

			return token;
		}

	}
}
