using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SM.Helper
{
	public static class JWTService
	{
		private static string SecureKey = "This is the private key used to encrypt JWT";

		public static string Generate(int id)
		{
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecureKey));
			var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
			var header = new JwtHeader(credentials);
			var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Now.AddMinutes(60));

			var securityToken = new JwtSecurityToken(header, payload);

			return new JwtSecurityTokenHandler().WriteToken(securityToken);
		}

		public static JwtSecurityToken Verify(string jwt)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.ASCII.GetBytes(SecureKey);
			tokenHandler.ValidateToken(jwt,
				new TokenValidationParameters
				{
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false
				},
				out SecurityToken validatedToken);

			return (JwtSecurityToken)validatedToken;
		}
	}
}
