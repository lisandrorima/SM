using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using AutoMapper;
using SM.Helper;
using System.Text.Json;
using System.Security.Cryptography;

namespace SM.Bll
{
	public class BllRent : IBllRent
	{
		private IDaoRent _DaoRent;
		private IDaoUser _DaoUser;
		private readonly IMapper _mapper;
		private IDaoRealEstate _daoRealEstate;
		public BllRent(IDaoRent daoRent, IMapper mapper, IDaoRealEstate daoRealEstate, IDaoUser daoUser)
		{
			_DaoRent = daoRent;
			_mapper = mapper;
			_daoRealEstate = daoRealEstate;
			_DaoUser = daoUser;
		}
		public async Task<DTORentProperty> Rent(DTORentProperty dto, string userEmail)
		{
			User tenant =  await _DaoUser.GetByEmail(userEmail);
			var REFromDDBB = await _daoRealEstate.GetPropertyByIDAsync(dto.ID);
			
			DTOShowRealEstate DTOfromDDBB = _mapper.Map<DTOShowRealEstate>(REFromDDBB);

			if (RentVerify.VerifyRent(dto, DTOfromDDBB))
			{

				var owner =await _DaoUser.GetUserByID(REFromDDBB.User.ID.Value);

				RentContract ContractToAdd = new()
				{
					Owner = owner,
					Tenant = tenant,
					RealEstate = REFromDDBB,
					StartDate = DateTime.Now,
					EndDate = DateTime.Now.AddDays(REFromDDBB.RentDurationDays),
					ValidatedByBlockChain = false,
					Hash = ""

				};
				ContractToAdd.Hash = GenerateHash(ContractToAdd);

				await _DaoRent.RentRealEstate(ContractToAdd);
				await _daoRealEstate.DisableRealEstate(REFromDDBB);

			}
			

			 return dto;
		}

		private string GenerateHash(RentContract contract)
		{
			

			using (SHA256 sha256Hash = SHA256.Create())
			{
				ASCIIEncoding encoding = new ASCIIEncoding();
				StringBuilder sb = new StringBuilder();
				sb.Append(contract.Owner);
				sb.Append(contract.RealEstate);
				sb.Append(contract.Tenant);
				sb.Append(contract.StartDate);
				sb.Append(contract.EndDate);



				// ComputeHash - returns byte array  
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));

				// Convert byte array to a string   
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}


			
		}
	}
}
