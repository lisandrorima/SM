using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SM.DAL.Models
{
	[Serializable]
	public class RentContract
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		
		public int? ID { get; set; }
		public User Owner{ get; set; }
		public User Tenant { get; set; }
		public RealEstate RealEstate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Hash { get; set; }
		public bool ValidatedByBlockChain { get; set; }
		public List<CuponDePago> cupones { get; set; }

		public bool Isvalid { get; set; }
	}
}
