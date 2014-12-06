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
		public SqlClause Where { get { return this.where ?? (this.where = SqlClauseBuilder.Where ()); } }

		/// <summary>
		///     A "group by" clause.
		/// </summary>
		[NotNull]
		public SqlClause GroupBy { get { return this.groupBy ?? (this.groupBy = SqlClauseBuilder.GroupBy ()); } }

		/// <summary>
		///     A "having" clause.
		/// </summary>
		[NotNull]
		public SqlClause Having { get { return this.having ?? (this.having = SqlClauseBuilder.Having ()); } }


		/// <summary>
		///     An "order by" clause.
		/// </summary>
		[NotNull]
		public SqlClause OrderBy { get { return this.orderBy ?? (this.orderBy = SqlClauseBuilder.OrderBy ()); } }

		#endregion

		#region Public methods

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