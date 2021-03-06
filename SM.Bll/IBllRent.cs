using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.DAL.Models;
using SM.Entities;

namespace SM.Bll
{
	public interface IBllRent
	{
		Task<DTORentProperty> Rent(DTORentProperty dto, string Useremail);
		Task<IEnumerable<DTOCuponPago>> MisCupones(string email);
		Task<IEnumerable<DTOContractWithCupons>> GetDTOContractWithCupons(string email);
		Task<IEnumerable<DTOContractWithCupons>> GetDTOContractWithCuponsOwner(string email);
		Task<int> UpdateContractsAndCoupons();
		Task<IEnumerable<DTOpaymentVerification>> GetAllValidCouponsWithRealEstate();
		Task<int> ValidarCupones(List<DTOpaymentVerification> cuponesAModificar);
	}
}
