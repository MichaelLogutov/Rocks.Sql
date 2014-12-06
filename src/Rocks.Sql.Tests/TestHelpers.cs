using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Rocks.Sql.Tests
{
	public static class TestHelpers
	{
		#region Private fields

		private static readonly Regex NormalizeSpacesRegex = new Regex (@"[\s\n\r]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		#endregion

		#region Static methods

		public static AndConstraint<StringAssertions> BeEquivalentToSql (this StringAssertions assertions,
		                                                                 string expectedSqlEquivalent,
		                                                                 string reason = null,
		                                                                 params object[] reasonArgs)
		{
			var subject = NormalizeSql (assertions.Subject);
			var expected = NormalizeSql (expectedSqlEquivalent);

			return subject.Should ().Be (expected);
		}

		#endregion

		#region Private methods

		private static string NormalizeSql (string subject)
		{
			NormalizeSpacesRegex.Replace (subject, " ");
			return subject;
		}

		#endregion
	}
}