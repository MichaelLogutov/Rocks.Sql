using System;
using JetBrains.Annotations;

namespace Rocks.Sql
{
	public static class InitializersExtensions
	{
		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "select" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsSelectClause ([NotNull] this SqlClause sqlClause)
		{
			sqlClause.Prefix = "select" + Environment.NewLine + "\t";
			sqlClause.Separator = "," + Environment.NewLine + "\t";
			sqlClause.Suffix = Environment.NewLine;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "from" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsFromClause ([NotNull] this SqlClause sqlClause)
		{
			sqlClause.Prefix = "from" + Environment.NewLine + "\t";
			sqlClause.Separator = Environment.NewLine + "\t";
			sqlClause.Suffix = Environment.NewLine;

			return sqlClause;
		}
	}
}