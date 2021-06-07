using Microsoft.EntityFrameworkCore;
using SM.DAL.Dao_interfaces;
using SM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Dao
{
	public class DaoRent : IDaoRent
	{

		private readonly SmartPropDbContext _context;

		public DaoRent(SmartPropDbContext dbcontext)
		{
			_context = dbcontext;
		}

		public async Task<IEnumerable<CuponDePago>> AddCupones(List<CuponDePago> cupones)
		{
			foreach (var cupon in cupones)
			{
				_context.CuponDePagos.Add(cupon);
			}
			await _context.SaveChangesAsync();
			return cupones;
		}

		public async Task<IEnumerable<CuponDePago>> GetCupones(User user)
		{
			var contratos= await _context.RentContracts.Where(c => c.Tenant == user).ToListAsync();


			return await _context.CuponDePagos.Where(r => contratos.Contains(r.rentContract)).ToListAsync();
		}

		public async Task<IEnumerable<RentContract>> GetContractWithCupons(User user)
		{
			return await _context.RentContracts.Where(c => c.Tenant == user).Include(r=>r.cupones).Include(m=>m.RealEstate).Include(o=>o.Owner).ToListAsync();
		}

		public async Task<IEnumerable<CuponDePago>> GetContractWithCuponsForPaymentVerification()
		{
			var contratos = await _context.RentContracts.Where(c => c.Isvalid).ToListAsync();


			return await _context.CuponDePagos.Where(r => contratos.Contains(r.rentContract)).Include(c=>c.rentContract).Include(r=>r.rentContract.RealEstate).Include(b=>b.rentContract.Owner).Include(v=>v.rentContract.Tenant).ToListAsync();
		}



		public async Task<RentContract> RentRealEstate(RentContract rent)
		{
			_context.RentContracts.Add(rent);
			await _context.SaveChangesAsync();
			return rent;
		}

		public async Task<int> UpdateCuponesValidity()
		{
			List<CuponDePago> cuponesVencidos = await GetCuponesVencidos();

			var contratosVencidos = cuponesVencidos.Select(x => x.rentContract).ToList();
			List<CuponDePago> AllCuponsOfExpiredContracts = await GetAllCuponesAVencer(contratosVencidos);
			ExpireCuponsAndContracts(contratosVencidos, AllCuponsOfExpiredContracts);
			SetRealEstateAvailable(contratosVencidos);
			return await _context.SaveChangesAsync();
		}

		private static void SetRealEstateAvailable(List<RentContract> contratosVencidos)
		{
			contratosVencidos.ForEach(c => c.RealEstate.Available = true);

		}

		private static void ExpireCuponsAndContracts(List<RentContract> contratosVencidos, List<CuponDePago> AllCuponsOfExpiredContracts)
		{
			
			contratosVencidos.ForEach(c => c.Isvalid = false);
			AllCuponsOfExpiredContracts.ForEach(c => c.Isvalid = false);

		}

		private async Task<List<CuponDePago>> GetAllCuponesAVencer(List<RentContract> contratosVencidos)
		{
			return await _context.CuponDePagos.Where(c => contratosVencidos.Contains(c.rentContract)).ToListAsync();
		}

		private async Task<List<CuponDePago>> GetCuponesVencidos()
		{
			return await _context.CuponDePagos.Where(c => c.FechaVencimiento < DateTime.Now & c.Isvalid==true & !c.IsPayed).Include(r => r.rentContract).Include(m=>m.rentContract.RealEstate).ToListAsync();
		}

		public async Task<int> UpdatePagados(List<string> cupones)
		{
				var cuponesAModificar = await _context.CuponDePagos.Where(c => cupones.Contains(c.HashCuponPago)).ToListAsync();
				cuponesAModificar.ForEach(c => c.IsPayed = true);
			return await _context.SaveChangesAsync();


			


		}
	}
}
