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
using Winista.Mime;


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
					Hash = "",
					Isvalid = true

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

		public async Task<int> UpdateContractsAndCoupons()
		{
			return await _DaoRent.UpdateCuponesValidity();
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



				return CreateHash(sha256Hash, sb);

			}



		}

		private List<CuponDePago> GenerateCupones(RentContract contract, RealEstate realEstate)
		{
			List<CuponDePago> cupones = new List<CuponDePago>();


			for (int i = 0; i < realEstate.RentDurationDays; i++)
			{
				CuponDePago cupon = new CuponDePago();

				cupon.IsPayed = false;
				cupon.rentContract = contract;
				cupon.FechaVencimiento = GenerateFechaVencimiento(cupon, i, contract);
				cupon.Isvalid = true;
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

				return CreateHash(sha256Hash, sb);
			}
		}

		private static string CreateHash(SHA256 sha256Hash, StringBuilder sb)
		{
			byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));

			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				builder.Append(bytes[i].ToString("x2"));
			}
			return "0x" + builder.ToString();
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

		public async Task<IEnumerable<DTOpaymentVerification>> GetAllValidCouponsWithRealEstate()
		{
			var cuponDePagos = await _DaoRent.GetContractWithCuponsForPaymentVerification();
			List<DTOpaymentVerification> cuponesValidos = new List<DTOpaymentVerification>();


			foreach (var item in cuponDePagos)
			{
				DTOpaymentVerification verifiacion = new DTOpaymentVerification();

				verifiacion.Inquilino = item.rentContract.Tenant.WalletAddress;
				verifiacion.Propietario = item.rentContract.Owner.WalletAddress;
				verifiacion.Monto = item.rentContract.RealEstate.RentFee;
				verifiacion.Cupon = item.HashCuponPago;

				cuponesValidos.Add(verifiacion);
			}


			return cuponesValidos;
		}

		public async Task<int> ValidarCupones(List<DTOpaymentVerification> cuponesAModificar)
		{
			List<string> cupones = new List<string>();
			CuponDePago cupon = new CuponDePago();


			foreach (var item in cuponesAModificar)
			{ 
				cupones.Add(item.Cupon);
			}

			return await _DaoRent.UpdatePagados(cupones);
		}
	}
}
