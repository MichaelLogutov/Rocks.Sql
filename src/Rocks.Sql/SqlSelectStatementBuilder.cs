using System;
using System.Linq;
using JetBrains.Annotations;

namespace Rocks.Sql
{
	/// <summary>
	///     Represents a builder for sql select statement.
	/// </summary>
	public class SqlSelectStatementBuilder
	{
		#region Private fields

		[NotNull]
		private readonly SqlClause select;

		[NotNull]
		private readonly SqlClause from;

		[CanBeNull]
		private SqlClause where;

		[CanBeNull]
		private SqlClause groupBy;

		[CanBeNull]
		private SqlClause having;

		[CanBeNull]
		private SqlClause orderBy;

		#endregion

		#region Construct

		/// <summary>
		///     Initializes a new instance of the <see cref="SqlSelectStatementBuilder" /> class.
		/// </summary>
		public SqlSelectStatementBuilder ([NotNull] string tableName)
		{
			this.select = SqlClauseBuilder.Select ();
			this.from = SqlClauseBuilder.From (tableName);
		}

		#endregion

		#region Public properties

		/// <summary>
		///     A "select" clause.
		/// </summary>
		[NotNull]
		public SqlClause Select { get { return this.select; } }

		/// <summary>
		///     A "from" clause.
		/// </summary>
		[NotNull]
		public SqlClause From { get { return this.from; } }

		/// <summary>
		///     A "where" clause.
		/// </summary>
		[NotNull]
		public SqlClause Where
		{
			get
			{
				if (this.where == null)
					this.where = SqlClauseBuilder.Where ();
				return this.where;
			}
		}

		/// <summary>
		///     A "group by" clause.
		/// </summary>
		[NotNull]
		public SqlClause GroupBy
		{
			get
			{
				if (this.groupBy == null)
					return this.groupBy = SqlClauseBuilder.GroupBy ();

				return this.groupBy;
			}
		}

		/// <summary>
		///     A "having" clause.
		/// </summary>
		[NotNull]
		public SqlClause Having
		{
			get
			{
				if (this.having == null)
					return this.having = SqlClauseBuilder.Having ();

				return this.having;
			}
		}


		/// <summary>
		///     An "order by" clause.
		/// </summary>
		[NotNull]
		public SqlClause OrderBy
		{
			get
			{
				if (this.orderBy == null)
					return this.orderBy = SqlClauseBuilder.OrderBy ();

				return this.orderBy;
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		///     Adds new column to select statement.
		///		If the same column (case sensitive) hase been already added, ignores it.
		/// </summary>
		public SqlSelectStatementBuilder Column (string columnName)
		{
			this.select.Add (columnName, columnName);
			return this;
		}


		/// <summary>
		///     Adds new columns to select statement.
		///		If the same column (case sensitive) hase been already added, ignores it.
		/// </summary>
		public SqlSelectStatementBuilder Columns (params string[] columns)
		{
			foreach (var column in columns)
				this.Column (column);

			return this;
		}


		/// <summary>
		///     Builds the sql statement based on current information.
		/// </summary>
		/// <returns></returns>
		public SqlClause Build ()
		{
			if (this.Select.IsEmpty)
				this.Select.Add ("*");

			var result = new SqlClause ();

			result.Add (this.Select);
			result.Add (this.From);

			if (this.where != null)
				result.Add (this.where);

			if (this.groupBy != null)
				result.Add (this.groupBy);

			if (this.having != null)
				result.Add (this.having);

			if (this.orderBy != null)
				result.Add (this.orderBy);

			return result;
		}


		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString ()
		{
			return this.Build ().GetSql ();
		}

		#endregion
	}
}