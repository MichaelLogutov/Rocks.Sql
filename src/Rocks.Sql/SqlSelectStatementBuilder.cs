using System;
using System.Data;
using JetBrains.Annotations;

namespace Rocks.Sql
{
    /// <summary>
    ///     Represents a builder for sql select statement.
    /// </summary>
    public class SqlSelectStatementBuilder
    {
        #region Private readonly fields

        [NotNull]
        private readonly SqlClause select;

        [NotNull]
        private readonly SqlClause from;

        #endregion

        #region Private fields

        [CanBeNull]
        private SqlClause prefix;

        [CanBeNull]
        private SqlClause where;

        [CanBeNull]
        private SqlClause groupBy;

        [CanBeNull]
        private SqlClause having;

        [CanBeNull]
        private SqlClause orderBy;

        [CanBeNull]
        private SqlClause suffix;

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
        ///     A starting sql clause. This sql will be prepended to the result.
        /// </summary>
        [NotNull]
        public SqlClause Prefix
        {
            get
            {
                if (this.prefix == null)
                {
                    this.prefix = new SqlClause ();
                    this.prefix.ExpressionsSeparator = Environment.NewLine;
                    this.prefix.Suffix = Environment.NewLine;
                }

                return this.prefix;
            }
        }

        /// <summary>
        ///     A "select" clause.
        /// </summary>
        [NotNull]
        public SqlClause Select
        {
            get { return this.select; }
        }

        /// <summary>
        ///     A "from" clause.
        /// </summary>
        [NotNull]
        public SqlClause From
        {
            get { return this.from; }
        }

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

        /// <summary>
        ///     An ending sql clause. This sql will be appended to the result.
        /// </summary>
        [NotNull]
        public SqlClause Suffix
        {
            get
            {
                if (this.suffix == null)
                {
                    this.suffix = new SqlClause ();
                    this.suffix.ExpressionsSeparator = Environment.NewLine;
                    this.suffix.Prefix = Environment.NewLine;
                }

                return this.suffix;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Adds new column to select statement.
        ///     If the same column (case sensitive) hase been already added, ignores it.
        /// </summary>
        public SqlSelectStatementBuilder Column (string columnName)
        {
            this.select.Add (columnName, columnName);
            return this;
        }


        /// <summary>
        ///     Adds new columns to select statement.
        ///     If the same column (case sensitive) hase been already added, ignores it.
        /// </summary>
        public SqlSelectStatementBuilder Columns (params string[] columns)
        {
            foreach (var column in columns)
                this.Column (column);

            return this;
        }


        /// <summary>
        ///     Changes the select clause to start with "select top(@topValueParameterName)" text.
        ///     Adds the <paramref name="topValueParameter" /> to the clause parameters list.
        /// </summary>
        public SqlSelectStatementBuilder Top ([NotNull] IDbDataParameter topValueParameter)
        {
            this.Select.Prefix = "select top(" + topValueParameter.ParameterName + ")" + Environment.NewLine;
            this.Select.Add (topValueParameter);

            return this;
        }


        /// <summary>
        ///     Adds CTE to the statement.
        /// </summary>
        public SqlSelectStatementBuilder CTE ([NotNull] SqlSelectStatementBuilder statement, [NotNull] string name = "X")
        {
            if (statement == null)
                throw new ArgumentNullException ("statement");

            if (string.IsNullOrEmpty (name))
                throw new ArgumentNullException ("name");

            var cte = SqlClauseBuilder.CTE (name, statement.Build ());
            this.Prefix.Add (cte);

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

            if (this.prefix != null)
                result.Add (this.prefix);

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

            if (this.suffix != null)
                result.Add (this.suffix);

            return result;
        }


        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString ()
        {
            return this.Build ().GetSql ();
        }

        #endregion
    }
}