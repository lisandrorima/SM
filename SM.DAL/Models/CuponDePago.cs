using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.DAL.Models
{
	public class CuponDePago
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int? ID { get; set; }
		public string HashCuponPago { get; set; }
		public DateTime FechaVencimiento { get; set; }
		public bool IsPayed { get; set; }
		public RentContract rentContract { get; set; }
		public bool Isvalid { get; set; }

		public int Monto { get; set; }
		public string TXHash { get; set; }
		public string MinedOnBlock { get; set; }

	}
}

