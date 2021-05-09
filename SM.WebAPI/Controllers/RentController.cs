using Microsoft.AspNetCore.Mvc;
using SM.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SM.WebAPI.Controllers
{

	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class RentController : ControllerBase
	{

		private IBllRent _repository;
		public RentController(IBllRent BllRent)
		{
			_repository = BllRent;
		}


		
	}
}
