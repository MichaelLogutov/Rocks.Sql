using System;
using JetBrains.Annotations;

namespace Rocks.Sql
{
	public static class InitializersExtensions
	{
		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql clause of column names list.
		///		The format is:
		///		<code>
		///			<paramref name="statement"/>
		///				expression1,
		///				expression2,
		///				...
		///		</code>
		/// </summary>
		[NotNull]
		public static SqlClause AsColumnsListClause ([NotNull] this SqlClause sqlClause, string statement)
		{
			sqlClause.Prefix = statement + Environment.NewLine;
			sqlClause.ExpressionsPrefix = "\t";
			sqlClause.ExpressionsSeparator = "," + Environment.NewLine + "\t";
			sqlClause.ExpressionsSuffix = Environment.NewLine;
			sqlClause.Suffix = null;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql clause of predicates list.
		///		The format is:
		///		<code>
		///			<paramref name="statement"/>
		///				(expression1)
		///				<paramref name="logic"/> (expression2)
		///				<paramref name="logic"/> (expression3)
		///				...
		///		</code>
		/// </summary>
		[NotNull]
		public static SqlClause AsPredicatesClause ([NotNull] this SqlClause sqlClause, string statement, string logic = "and")
		{
			sqlClause.Prefix = statement + Environment.NewLine;
			sqlClause.ExpressionsPrefix = "\t(";
			sqlClause.ExpressionsSeparator = ")" + Environment.NewLine + "\t" + logic + " (";
			sqlClause.ExpressionsSuffix = ")" + Environment.NewLine;
			sqlClause.Suffix = null;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql clause of statements list.
		///		The format is:
		///		<code>
		///			<paramref name="statement"/>
		///				expression1
		///				expression2
		///				...
		///		</code>
		/// </summary>
		[NotNull]
		public static SqlClause AsStatementsClause ([NotNull] this SqlClause sqlClause, string statement, string indent = "\t")
		{
			sqlClause.Prefix = statement + Environment.NewLine;
			sqlClause.ExpressionsPrefix = indent;
			sqlClause.ExpressionsSeparator = Environment.NewLine + indent;
			sqlClause.ExpressionsSuffix = Environment.NewLine;
			sqlClause.Suffix = null;

			return sqlClause;
		}
	}
}