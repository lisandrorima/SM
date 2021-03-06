using SM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Dao_interfaces
{
	public interface IDaoRent
	{
		Task<RentContract> RentRealEstate(RentContract rent);
		Task<IEnumerable<CuponDePago>> AddCupones(List<CuponDePago> cupones);
		Task<IEnumerable<CuponDePago>> GetCupones(User user);
		Task<IEnumerable<RentContract>> GetContractWithCupons(User user);
		Task<IEnumerable<RentContract>> GetContractWithCuponsOwner(User user);
		Task<int> UpdateCuponesValidity();
		Task<IEnumerable<CuponDePago>> GetContractWithCuponsForPaymentVerification();
		Task<int> UpdatePagados(List<CuponDePago> cupones);
	}
}
