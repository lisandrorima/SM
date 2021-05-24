using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SM.Entities;
using System.Text.Json;



namespace SM.Helper
{
	public static class RentVerify
	{
		public static bool VerifyRent(DTORentProperty fromFront, DTOShowRealEstate fromDDBB)
		{
			bool isValid = false;

		if(fromFront.ID==fromDDBB.ID && fromFront.Address==fromDDBB.Address
                && fromFront.RentDurationDays == fromDDBB.RentDurationDays && fromFront.RentFee== fromDDBB.RentFee &&
                fromFront.RentPaymentSchedule == fromDDBB.RentPaymentSchedule && fromDDBB.Available)
			{
                isValid=true;
			}


            return isValid;
		}


       
	}
}
