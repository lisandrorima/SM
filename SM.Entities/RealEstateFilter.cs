using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Entities
{
	public class RealEstateFilter
	{
		public DTOProvincia Provincia { get; set; }
		public string Localidad { get; set; }
		public int? MinArea { get; set; }
		public int? MaxArea { get; set; }
		public int? MinPrice { get; set; }
		public int? MaxPrice { get; set; }
		public int? MinRooms { get; set; }
		public int? MaxRooms { get; set; }
		public int? Minbathrooms { get; set; }
		public int? MaxBathrooms { get; set; }
		public bool? Garage { get; set; }



	}
}
