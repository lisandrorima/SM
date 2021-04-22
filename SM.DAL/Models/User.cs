using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SM.DAL.Models
{
	public class User
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int? ID { get; set; }

		[Required]
		public int PersonalID { get; set; }
		public string Email { get; set; }

		[Required]
		[Column(TypeName = "NVARCHAR(100)")]
		public string Name { get; set; }

		[Required]
		[Column(TypeName = "NVARCHAR(100)")]
		public string Surname { get; set; }

		[Required]
		[Column(TypeName = "NVARCHAR(100)")]
		[JsonIgnore]
		public string Password { get; set; }

		[Required]
		[Column(TypeName = "NVARCHAR(42)")]
		public string WalletAddress { get; set; }
	}
}
