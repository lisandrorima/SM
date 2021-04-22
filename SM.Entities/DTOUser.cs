using System;

namespace SM.Entities
{
	public class DTOUser
	{
		public int PersonalID { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Password { get; set; }
		public string WalletAddress { get; set; }
	}
}
