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
			sqlClause.Prefix = statement + Environment.NewLine + "\t";
			sqlClause.Separator = "," + Environment.NewLine + "\t";
			sqlClause.Suffix = Environment.NewLine;

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
			sqlClause.Prefix = statement + Environment.NewLine + "\t(";
			sqlClause.Separator = ")" + Environment.NewLine + "\t" + logic + " (";
			sqlClause.Suffix = ")" + Environment.NewLine;

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
			sqlClause.Prefix = statement + Environment.NewLine + indent;
			sqlClause.Separator = Environment.NewLine + indent;
			sqlClause.Suffix = Environment.NewLine;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "select" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsSelectClause ([NotNull] this SqlClause sqlClause)
		{
			return sqlClause.AsColumnsListClause ("select");
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "from" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsFromClause ([NotNull] this SqlClause sqlClause)
		{
			return sqlClause.AsStatementsClause ("from");
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "where" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsWhereClause ([NotNull] this SqlClause sqlClause, string logic = "and")
		{
			return sqlClause.AsPredicatesClause ("where", logic);
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "group by" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsGroupByClause ([NotNull] this SqlClause sqlClause)
		{
			return sqlClause.AsColumnsListClause ("group by");
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "having" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsHavingClause ([NotNull] this SqlClause sqlClause, string logic = "and")
		{
			return sqlClause.AsPredicatesClause ("having", logic);
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql CTE clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsCTEClause ([NotNull] this SqlClause sqlClause, string cteName)
		{
			sqlClause.Prefix = ";with " + cteName + " as (" + Environment.NewLine + "\t";
			sqlClause.Separator = null;
			sqlClause.Suffix = Environment.NewLine + ")" + Environment.NewLine;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "delete" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsDeleteClause ([NotNull] this SqlClause sqlClause, string tableName)
		{
			sqlClause.Prefix = "delete from " + tableName + Environment.NewLine;
			sqlClause.Separator = Environment.NewLine;
			sqlClause.Suffix = Environment.NewLine;
			sqlClause.RenderIfEmpty = true;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "update" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsUpdateClause ([NotNull] this SqlClause sqlClause, string tableName)
		{
			sqlClause.Prefix = "update " + tableName + Environment.NewLine + "set" + Environment.NewLine + "\t";
			sqlClause.Separator = "," + Environment.NewLine + "\t";
			sqlClause.Suffix = Environment.NewLine;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "insert" clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsInsertClause ([NotNull] this SqlClause sqlClause, string tableName)
		{
			sqlClause.AsStatementsClause ("insert into " + tableName, indent: null);
			sqlClause.RenderIfEmpty = true;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql insert columns list clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsInsertColumnsClause ([NotNull] this SqlClause sqlClause)
		{
			sqlClause.Prefix = "(" + Environment.NewLine + "\t";
			sqlClause.Separator = "," + Environment.NewLine + "\t";
			sqlClause.Suffix = Environment.NewLine + ")" + Environment.NewLine;

			return sqlClause;
		}


		/// <summary>
		///     Initialize <see cref="SqlClause" /> to represent the sql "values" (of insert) clause.
		/// </summary>
		[NotNull]
		public static SqlClause AsValuesClause ([NotNull] this SqlClause sqlClause)
		{
			sqlClause.Prefix = "values" + Environment.NewLine + "(" + Environment.NewLine + "\t";
			sqlClause.Separator = "," + Environment.NewLine + "\t";
			sqlClause.Suffix = Environment.NewLine + ")" + Environment.NewLine;

			return sqlClause;
		}
	}
}