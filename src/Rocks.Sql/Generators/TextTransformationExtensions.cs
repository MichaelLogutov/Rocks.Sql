using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TextTemplating;

namespace Rocks.Sql.Generators
{
	/// <summary>
	/// Extensions for <see cref="TextTransformation"/> class.
	/// </summary>
	public static class TextTransformationExtensions
	{
		/// <summary>
		///     Writes an empty line.
		/// </summary>
		public static void WriteLine ([NotNull] this TextTransformation tt)
		{
			tt.WriteLine (string.Empty);
		}


		/// <summary>
		///     Writes <paramref name="count"/> new lines.
		/// </summary>
		public static void WriteNewLines ([NotNull] this TextTransformation tt, int count)
		{
			for (var k = 0; k < count; k++)
				tt.WriteLine (string.Empty);
		}


		/// <summary>
		///     Writes formatted string (with <see cref="TextTransformation.Write(string)" /> method)
		///     and pushes the indent of string containing the same amount of spaces
		///     as the source formatted string.
		/// </summary>
		public static void WriteAndPushIndent ([NotNull] this TextTransformation tt, string format, params object[] args)
		{
			var str = string.Format (format, args);

			tt.Write (str);
			tt.PushIndent (new string (' ', str.Length));
		}


		/// <summary>
		///     Writes several new lines for separating methods declarations.
		/// </summary>
		public static void WriteMethodsSeparator ([NotNull] this TextTransformation tt)
		{
			tt.WriteLine (string.Empty);
			tt.WriteLine (string.Empty);
		}
	}
}