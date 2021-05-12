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


        private static string HashProp(DTORentProperty model)
		{
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)));

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
