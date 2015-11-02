using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;

namespace Rocks.Sql
{
    /// <summary>
    ///     Represents a builder with the list of expressions and predicates
    ///     that can be converted to sql clause string and a list of parameters.
    /// </summary>
    public class SqlClause
    {
        #region Private fields

        private readonly OrderedHybridCollection<string, string> expressions;
        private readonly Dictionary<string, IDbDataParameter> parameters;

        #endregion

        #region Construct

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlClause" /> class.
        /// </summary>
        public SqlClause (string initialExpression = null)
        {
            this.expressions = new OrderedHybridCollection<string, string> (StringComparer.Ordinal);
            this.parameters = new Dictionary<string, IDbDataParameter> (StringComparer.Ordinal);

            if (!string.IsNullOrEmpty (initialExpression))
                this.Add (initialExpression);
        }

        #endregion

        #region Public properties

        /// <summary>
        ///     Prefix string that will be added before the clause content.
        /// </summary>
        [CanBeNull]
        public string Prefix { get; set; }


        /// <summary>
        ///     Suffix string that will be added after the clause content.
        /// </summary>
        [CanBeNull]
        public string Suffix { get; set; }


        /// <summary>
        ///     Prefix string that will be added before the list of clause expressions (if there are any).
        /// </summary>
        [CanBeNull]
        public string ExpressionsPrefix { get; set; }


        /// <summary>
        ///     Suffix string that will be added after the list of clause expressions (if there are any).
        /// </summary>
        [CanBeNull]
        public string ExpressionsSuffix { get; set; }


        /// <summary>
        ///     A separator that will be inserted between clause expressions (if there are any).
        /// </summary>
        [CanBeNull]
        public string ExpressionsSeparator { get; set; }


        /// <summary>
        ///     Forces rending of the clause <see cref="Suffix"/> and <see cref="Prefix"/> to string
        ///		even if it contains no expressions.
        ///     Default value is false (skip rendering if no expressions addedd).
        /// </summary>
        public bool RenderIfEmpty { get; set; }


        /// <summary>
        ///     Returns true if there is no sql expressions in the clause.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.expressions.Count == 0; }
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Returns true if there is an expression with the specified key.
        /// </summary>
        public bool ContainsExpression ([NotNull] string key)
        {
            return this.expressions.Contains (key);
        }


        /// <summary>
        ///     Returns true if there is a parameter with the specified name.
        /// </summary>
        public bool ContainsParameter ([NotNull] string parameterName)
        {
            return this.parameters.ContainsKey (parameterName);
        }


        /// <summary>
        ///     Adds specified parameter to the parameters list.
        ///     If there is a parameter with the same name present (case sensitive), it will be overwritten.
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        public SqlClause Add ([NotNull] IDbDataParameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException ("parameter");

            if (string.IsNullOrEmpty (parameter.ParameterName))
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException ("parameter.ParameterName");

            this.parameters[parameter.ParameterName] = parameter;

            return this;
        }


        /// <summary>
        ///     Adds the specified <paramref name="sql"/> to the list of sql expressions.
        ///		If <paramref name="sql"/> is null or empty - nothing will be added.
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        public SqlClause Add ([CanBeNull] string sql)
        {
            if (!string.IsNullOrEmpty (sql))
                this.expressions.AddSequenced (sql);

            return this;
        }


        /// <summary>
        ///     <para>
        ///         Adds the specified <paramref name="sql"/> to the list of sql expressions.
        ///         If <paramref name="sql" /> is null or empty - nothing will be added.
        ///     </para>
        ///     <para>
        ///         If there is an sql expression with the same <paramref name="key" /> present (case sensitive),
        ///         it will be overwritten (if <paramref name="overwrite" /> = true),
        ///         or nothing will be added (if <paramref name="overwrite" /> = false).
        ///     </para>
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        public SqlClause Add ([NotNull] string key, [CanBeNull] string sql, bool overwrite = false)
        {
            if (!string.IsNullOrEmpty (sql))
                this.expressions.AddKeyed (key, sql, overwrite);

            return this;
        }


        /// <summary>
        ///     <para>
        ///         Adds the specified <paramref name="sql"/> and <paramref name="parameter"/> to the list of sql expressions.
        ///         If <paramref name="sql" /> is null or empty - nothing will be added.
        ///     </para>
        ///     <para>
        ///         If there is an sql expression with the same <paramref name="key" /> present (case sensitive),
        ///         it will be overwritten (if <paramref name="overwrite" /> = true),
        ///         or nothing will be added (if <paramref name="overwrite" /> = false).
        ///     </para>
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        public SqlClause Add ([NotNull] string key, [CanBeNull] string sql, [NotNull] IDbDataParameter parameter, bool overwrite = false)
        {
            if (string.IsNullOrEmpty (sql))
                return this;

            if (this.ContainsExpression (key) && !overwrite)
                return this;

            this.expressions.AddKeyed (key, sql, overwrite);
            this.Add (parameter);

            return this;
        }


        /// <summary>
        ///     <para>
        ///         Adds specified <paramref name="sqlClause" /> by appending
        ///         it sql representation as new expression and adding
        ///         all parameters (overwriting existing with the same name).
        ///     </para>
        ///     <para>
        ///         If there is an sql expression with the same <paramref name="key" /> present (case sensitive),
        ///         it will be overwritten (if <paramref name="overwrite" /> = true),
        ///         or not the whole clause will be not added (if <paramref name="overwrite" /> = false).
        ///     </para>
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        public SqlClause Add ([NotNull] string key, [NotNull] SqlClause sqlClause, bool overwrite = false)
        {
            if (sqlClause == null)
                throw new ArgumentNullException ("sqlClause");

            if (this.ContainsExpression (key) && !overwrite)
                return this;

            this.Add (key, sqlClause.GetSql (), overwrite);

            foreach (var parameter in sqlClause.GetParameters ())
                this.Add (parameter);

            return this;
        }


        /// <summary>
        ///     Renders the content of the clause to the string.
        ///     If the clause is empty returns <see cref="string.Empty" />.
        /// </summary>
        [NotNull]
        public string GetSql ()
        {
            var is_empty = this.IsEmpty;

            if (is_empty && !this.RenderIfEmpty)
                return string.Empty;

            var result = this.Prefix;

            if (!is_empty)
                result += this.ExpressionsPrefix + string.Join (this.ExpressionsSeparator, this.expressions) + this.ExpressionsSuffix;

            if (this.Suffix != null)
                result += this.Suffix;

            return result ?? string.Empty;
        }


        /// <summary>
        ///     Returns a new list of sql parameters of the clause.
        /// </summary>
        [NotNull]
        public IList<IDbDataParameter> GetParameters ()
        {
            return this.parameters.Values.ToList ();
        }


        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString ()
        {
            return this.GetSql ();
        }

        #endregion
    }
}