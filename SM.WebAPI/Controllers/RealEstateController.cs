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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Amazon.Runtime;
using Amazon.S3;
using System.IO;
using Amazon.S3.Transfer;
using Winista.Mime;

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
		[Route("provincias")]
		public Task<IEnumerable<DTOProvincia>> GetProvincias()
		{
			return _repository.GetProvincias();
		}



		[AllowAnonymous]
		[HttpGet]
		[Route("getRelacionados")]
		public Task<IEnumerable<DTOShowRealEstate>> GetByCiudad(string provincia)
		{
			return _repository.GetRelacionado(provincia);
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("areaFilter")]
		public Task<IEnumerable<DTOShowRealEstate>> GetbyMetros(int from, int to)
		{
			return _repository.GetByMetros(from, to);
		}


		[Authorize]
		[HttpGet]
		[Route("getMyRealEstates")]
		public Task<IEnumerable<DTOShowRealEstate>> GetMyRealEstates()
		{
			var email = GetEmailFromContext(HttpContext);
			return _repository.GetRealEstatesByOwner(email);
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("GetPropertyDetail")]
		public Task<IEnumerable<DTOShowFullDetail>> GetPropertyDetail(int id)
		{
			return _repository.GetFullDetailsByID(id);

		}


		[AllowAnonymous]
		[HttpGet]
		[Route("GetPropByID")]
		public async Task<IActionResult> GetPropByID(int id)
		{
			 var propiedad = await _repository.GetRealEstateByID(id);
			if (propiedad == null)
			{
				return NotFound();
			}
			return Ok(propiedad);
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("Getfiltered")]
		public async Task<IEnumerable<DTOShowRealEstate>> GerfilteredRealEstateList(RealEstateFilter realEstateFilter)
		{
			return await _repository.Getfiltered(realEstateFilter);

		}

		[Authorize]
		[HttpDelete]
		[Route("Borrarmipropiedad")]
		public async Task<IActionResult> BorrarMiPropiedad(int idProp)
		{
			var email = GetEmailFromContext(HttpContext);

			if (await _repository.DeletePropiedad(idProp, email) != null)
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}

		}

		[Authorize]
		[HttpPut]
		[HttpPost]
		[Route("propiedades")]
		public async Task<IActionResult> ModificarMiPropiedad(int? id, DTOAddRealEstate prop)
		{
			var email = GetEmailFromContext(HttpContext);
			if (prop.Provincia == null || prop.ImgURL.Count == 0)
				return BadRequest();

			if (id != null)
			{

				if (await _repository.UpdateRealEstate(prop, email) != null)
				{
					return Ok(prop);
				}
				return Ok();

			}
			else
			{
				if (await _repository.AddRealEstate(prop, email) != null)
				{
					return Ok(prop);
				}
				return Ok(); 
			}


		}

		private string GetEmailFromContext(HttpContext context)
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
		[HttpPost]
		[Route("altamipropiedad")]
		public async Task<IActionResult> AltaMiPropiedad(DTOAddRealEstate prop)
		{
			var email = GetEmailFromContext(HttpContext);

			await _repository.AddRealEstate(prop, email);

			return Ok();
		}



		[Authorize]
		[HttpPut]
		[Route("subirimagen")]
		public async Task<IActionResult> UploadImage([FromForm]ICollection<IFormFile> files)
		{
			List<Dictionary<string,string>> url = new List<Dictionary<string,string>>();
			foreach (var file in files)
			{
				
				var validated = _repository.validateImage(file);
				if (validated == null)
				{
					var credentials = new BasicAWSCredentials("AKIA5LZCIECLMXRCIGU4", "fieG1W90YQblzXLifAxOd36sjhNCc2EB71s8G0lj");
					var config = new AmazonS3Config
					{
						RegionEndpoint = Amazon.RegionEndpoint.SAEast1
					};
					using var client = new AmazonS3Client(credentials, config);
					await using var newMemoryStream = new MemoryStream();

					file.CopyTo(newMemoryStream);

					var uploadRequest = new TransferUtilityUploadRequest
					{
						InputStream = newMemoryStream,
						Key = Guid.NewGuid().ToString(),
						BucketName = "smartprop",
						CannedACL = S3CannedACL.PublicRead,
					};

					var fileTransferUtility = new TransferUtility(client);
					await fileTransferUtility.UploadAsync(uploadRequest);

					Dictionary<string, string> ObjImgUrl = new Dictionary<string, string>();

					ObjImgUrl.Add("ImgURL", string.Format("https://{0}.s3-{1}.amazonaws.com/{2}", uploadRequest.BucketName, config.RegionEndpoint.SystemName, uploadRequest.Key));
					url.Add(ObjImgUrl);
				}
					

				
				

			}
			return Ok(url);

		}




	}
}
