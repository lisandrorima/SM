﻿using SM.Entities;
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
					EndDate = DateTime.Now.AddMonths(REFromDDBB.RentDurationDays),
					ValidatedByBlockChain = false,
					Hash = ""

				};
				ContractToAdd.Hash = GenerateHashContrato(ContractToAdd);

				await _DaoRent.AddCupones( GenerateCupones(await _DaoRent.RentRealEstate(ContractToAdd),REFromDDBB));

				await _daoRealEstate.DisableRealEstate(REFromDDBB);

			}
			

			 return dto;
		}

		public async Task<IEnumerable<DTOCuponPago>> MisCupones(string email)
		{
			User user = await _DaoUser.GetByEmail(email);
			DTOCuponPago cuponDTO = new DTOCuponPago();
			List<DTOCuponPago> listcuponDTO = new List<DTOCuponPago>();
			
			if (user!=null)
			{
				var cupones =await _DaoRent.GetCupones(user);
				foreach (var cupon in cupones)
				{
					listcuponDTO.Add(_mapper.Map<DTOCuponPago>(cupon));
				}
			}

			return listcuponDTO;
		}

		public async Task<IEnumerable<DTOContractWithCupons>> GetDTOContractWithCupons(string email)
		{
			User user = await _DaoUser.GetByEmail(email);
			List<DTOContractWithCupons> listcuponDTO = new List<DTOContractWithCupons>();

			if (user != null)
			{
				var ContractWithCoupon = await _DaoRent.GetContractWithCupons(user);
				foreach (var contract in ContractWithCoupon)
				{
					listcuponDTO.Add(_mapper.Map<DTOContractWithCupons>(contract));
				}
			}

			return listcuponDTO;
		}


		private string GenerateHashContrato(RentContract contract)
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

		private List<CuponDePago> GenerateCupones(RentContract contract, RealEstate realEstate)
		{
			List<CuponDePago> cupones = new List<CuponDePago>();


			for (int i = 0; i < realEstate.RentPaymentSchedule; i++)
			{
				CuponDePago cupon = new CuponDePago();

				cupon.IsPayed = false;
				cupon.rentContract = contract;
				cupon.FechaVencimiento = GenerateFechaVencimiento(cupon, i, contract);
				cupon.HashCuponPago = GenerateHashCupon(cupon);

				cupones.Add(cupon);
			}

			return cupones;
		}

		private string GenerateHashCupon(CuponDePago cupon)
		{

			using (SHA256 sha256Hash = SHA256.Create())
			{
				ASCIIEncoding encoding = new ASCIIEncoding();
				StringBuilder sb = new StringBuilder();
				sb.Append(cupon.ID);
				sb.Append(cupon.FechaVencimiento);
				sb.Append(cupon.IsPayed);
				sb.Append(cupon.rentContract);





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

		private DateTime GenerateFechaVencimiento(CuponDePago cupon, int i,RentContract contract)
		{
			if (i == 0)
			{
				cupon.FechaVencimiento = contract.StartDate.AddDays(1);
			}
			else
			{
				cupon.FechaVencimiento = contract.StartDate.AddMonths(i);
			}

			return cupon.FechaVencimiento;
		}

		
	}
}
