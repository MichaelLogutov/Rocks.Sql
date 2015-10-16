using System;
using JetBrains.Annotations;

namespace Rocks.Sql
{
	/// <summary>
	///     A helper static class for creation of <see cref="SqlClause" /> for common sql clauses.
	/// </summary>
	public static class SqlClauseBuilder
	{
		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql statement containing specified sql clauses.
		/// </summary>
		[NotNull]
		public static SqlClause Statement ([NotNull] params SqlClause[] clauses)
		{
			if (clauses == null)
				throw new ArgumentNullException ("clauses");

			var sql_clause = new SqlClause ();

			foreach (var clause in clauses)
				sql_clause.Add (clause);

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "select" clause.
		/// </summary>
		[NotNull]
		public static SqlClause Select (params string[] columns)
		{
			var sql_clause = new SqlClause ().AsColumnsListClause ("select");

			if (columns != null && columns.Length > 0)
			{
				foreach (var column in columns)
					sql_clause.Add (column, column);
			}

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "from" clause.
		/// </summary>
		[NotNull]
		public static SqlClause From (string tableName)
		{
			var sql_clause = new SqlClause ().AsStatementsClause ("from" + Environment.NewLine + "\t" + tableName);
			sql_clause.RenderIfEmpty = true;

			return sql_clause;
		}


        /// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "from" clause.
		/// </summary>
		[NotNull]
		public static SqlClause Join ()
		{
			var sql_clause = new SqlClause ().AsStatementsClause ();
            sql_clause.Prefix = null;

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "where" clause.
		/// </summary>
		[NotNull]
		public static SqlClause Where (string logic = "and")
		{
			return new SqlClause ().AsPredicatesClause ("where", logic);
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "group by" clause.
		/// </summary>
		[NotNull]
		public static SqlClause GroupBy (params string[] columns)
		{
			var sql_clause = new SqlClause ().AsColumnsListClause ("group by");

			if (columns != null && columns.Length > 0)
			{
				foreach (var column in columns)
					sql_clause.Add (column, column);
			}

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "having" clause.
		/// </summary>
		[NotNull]
		public static SqlClause Having (string logic = "and")
		{
			return new SqlClause ().AsPredicatesClause ("having", logic);
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "order by" clause.
		/// </summary>
		[NotNull]
		public static SqlClause OrderBy (params string[] columns)
		{
			var sql_clause = new SqlClause ().AsColumnsListClause ("order by");

			if (columns != null && columns.Length > 0)
			{
				foreach (var column in columns)
					sql_clause.Add (column, column);
			}

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql CTE clause.
		/// </summary>
		[NotNull]
		public static SqlClause CTE (string cteName, SqlClause innerStatement = null)
		{
			var sql_clause = new SqlClause ();

			sql_clause.Prefix = ";with " + cteName + " as (" + Environment.NewLine;
			sql_clause.ExpressionsPrefix = "\t";
			sql_clause.ExpressionsSeparator = Environment.NewLine + "\t";
			sql_clause.ExpressionsSuffix = Environment.NewLine;
			sql_clause.Suffix = ")" + Environment.NewLine;

			if (innerStatement != null)
				sql_clause.Add (innerStatement);

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "delete" clause.
		/// </summary>
		[NotNull]
		public static SqlClause Delete (string tableName)
		{
			var sql_clause = new SqlClause ();

			sql_clause.Prefix = "delete from " + tableName + Environment.NewLine;
			sql_clause.ExpressionsPrefix = null;
			sql_clause.ExpressionsSeparator = Environment.NewLine;
			sql_clause.ExpressionsSuffix = Environment.NewLine;
			sql_clause.Suffix = null;
			sql_clause.RenderIfEmpty = true;

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "update" clause.
		/// </summary>
		[NotNull]
		public static SqlClause Update (string tableName)
		{
			var sql_clause = new SqlClause ();

			sql_clause.Prefix = "update " + tableName + Environment.NewLine;
			sql_clause.ExpressionsPrefix = "set" + Environment.NewLine + "\t";
			sql_clause.ExpressionsSeparator = "," + Environment.NewLine + "\t";
			sql_clause.ExpressionsSuffix = Environment.NewLine;
			sql_clause.Suffix = null;

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "insert" clause.
		/// </summary>
		[NotNull]
		public static SqlClause Insert (string tableName, params string[] columns)
		{
			var sql_clause = new SqlClause ();

			sql_clause.AsStatementsClause ("insert into " + tableName, indent: null);
			sql_clause.RenderIfEmpty = true;

			if (columns != null && columns.Length > 0)
				sql_clause.Add (InsertColumns (columns));

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql insert columns list clause.
		/// </summary>
		[NotNull]
		public static SqlClause InsertColumns (params string[] columns)
		{
			var sql_clause = new SqlClause ();

			sql_clause.Prefix = "(" + Environment.NewLine;
			sql_clause.ExpressionsPrefix = "\t";
			sql_clause.ExpressionsSeparator = "," + Environment.NewLine + "\t";
			sql_clause.ExpressionsSuffix = Environment.NewLine;
			sql_clause.Suffix = ")" + Environment.NewLine;

			foreach (var column in columns)
				sql_clause.Add (column);

			return sql_clause;
		}


		/// <summary>
		///     Creates new <see cref="SqlClause" /> to represent the sql "values" (of insert) clause.
		/// </summary>
		[NotNull]
		public static SqlClause Values ()
		{
			var sql_clause = new SqlClause ();

			sql_clause.Prefix = "values" + Environment.NewLine + "(" + Environment.NewLine;
			sql_clause.ExpressionsPrefix = "\t";
			sql_clause.ExpressionsSeparator = "," + Environment.NewLine + "\t";
			sql_clause.ExpressionsSuffix = Environment.NewLine;
			sql_clause.Suffix = ")" + Environment.NewLine;

			return sql_clause;
		}
	}
}