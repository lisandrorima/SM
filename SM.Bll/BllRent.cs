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
					ValidatedByBlockChain = false

				};
				await _DaoRent.RentRealEstate(ContractToAdd);
				await _daoRealEstate.DisableRealEstate(REFromDDBB);

			}
			

			 return dto;
		}
	}
}
