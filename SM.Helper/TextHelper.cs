using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Helper
{
	public static class TextHelper
	{
		public static string RemoveWhitespace(string input)
		{ 
			return new string(input.ToCharArray()
				.Where(c => !Char.IsWhiteSpace(c))
				.ToArray());
		}
	}
}
