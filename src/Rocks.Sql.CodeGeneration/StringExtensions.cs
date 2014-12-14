using System;
using System.Linq;

namespace Rocks.Sql.CodeGeneration
{
	public static class StringExtensions
	{
		public static string CapitalizeInvariant (this string str)
		{
			if (string.IsNullOrEmpty (str))
				return str;

			return char.ToUpperInvariant (str[0]) + str.Substring (1);
		}
	}
}