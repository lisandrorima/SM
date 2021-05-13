using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json;
using System.Security.Claims;
using SM.Entities;
using Microsoft.Extensions.Primitives;

namespace SM.Helper
{
	public  class JWTService
	{
		private static string SecureKey = "This is the private key used to encrypt JWT";


		public string Generate(DTOUser user)
		{
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecureKey));
			var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
			var header = new JwtHeader(credentials);

			var claims = new[] {
				
				new Claim("UserEmail",user.Email.ToString()),
				new Claim("UserPersonalID", user.PersonalID.ToString()),
				new Claim("UserName", user.Name.ToString()),
				new Claim("UserSurname", user.Surname.ToString()),
				new Claim("UserWallet", user.WalletAddress.ToString())
	
			};
			var payload = new JwtPayload("SmartProp.com", "SmartProp.com", claims, null, DateTime.Now.AddMinutes(5), DateTime.Now);

			var securityToken = new JwtSecurityToken(header, payload);

			return new JwtSecurityTokenHandler().WriteToken(securityToken);
		}

		public  JwtSecurityToken Verify(string jwt)
		{
			try
			{
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken empty = new JwtSecurityToken();
				var key = Encoding.ASCII.GetBytes(SecureKey);
				tokenHandler.ValidateToken(jwt,
					new TokenValidationParameters
					{
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidAudience= "SmartProp.com",
						ValidIssuer= "SmartProp.com"
					},
					out SecurityToken validatedToken);
				return (JwtSecurityToken)validatedToken;

			}
			catch (Exception)
			{
				return null;
			}
			


		}

		internal static StringValues RefreshToken(ClaimsIdentity claimsIdentity)
		{
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecureKey));
			var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
			var header = new JwtHeader(credentials);

			

				var claims = new[] {

				new Claim("UserEmail",claimsIdentity.FindFirst("UserEmail").Value),
				new Claim("UserPersonalID", claimsIdentity.FindFirst("UserPersonalID").Value),
				new Claim("UserName", claimsIdentity.FindFirst("UserName").Value),
				new Claim("UserSurname", claimsIdentity.FindFirst("UserSurname").Value),
				new Claim("UserWallet", claimsIdentity.FindFirst("UserWallet").Value)

			};

			var payload = new JwtPayload("SmartProp.com", "SmartProp.com", claims, null, DateTime.Now.AddMinutes(5), DateTime.Now);

			var securityToken = new JwtSecurityToken(header, payload);

			return new JwtSecurityTokenHandler().WriteToken(securityToken);
		}

	}
}
