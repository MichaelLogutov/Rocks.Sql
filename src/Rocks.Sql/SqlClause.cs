using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;

namespace Rocks.Sql
{
	/// <summary>
	///     Represents a list of expressions/predicates that
	///     can be rendered as sql clause string with a list of parameters.
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
		public SqlClause ()
		{
			this.expressions = new OrderedHybridCollection<string, string> (StringComparer.Ordinal);
			this.parameters = new Dictionary<string, IDbDataParameter> (StringComparer.Ordinal);
		}

		#endregion

		#region Public properties

		/// <summary>
		///     Prefix string that will be added before the clause content if it's not empty.
		/// </summary>
		[CanBeNull]
		public string Prefix { get; set; }


		/// <summary>
		///     Suffix string that will be added after the clause content if it's not empty.
		/// </summary>
		[CanBeNull]
		public string Suffix { get; set; }


		/// <summary>
		///     A separator that will be inserted between clause expressions.
		/// </summary>
		[CanBeNull]
		public string Separator { get; set; }

		/// <summary>
		///     Returns true if there is no sql expressions in the clause.
		/// </summary>
		public bool IsEmpty { get { return this.expressions.Count == 0; } }

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
		///     Adds specified parameter the parameters list.
		///     If there is a parameter with the same name present (case sensitive), it will be overwritten.
		/// </summary>
		/// <returns>Current instance (for chain calls).</returns>
		[NotNull]
		public SqlClause AddParameter ([NotNull] IDbDataParameter parameter)
		{
			if (parameter == null)
				throw new ArgumentNullException ("parameter");

			if (string.IsNullOrEmpty (parameter.ParameterName))
				throw new ArgumentNullException ("ParameterName");

			this.parameters[parameter.ParameterName] = parameter;

			return this;
		}


		/// <summary>
		///     Adds the specified <paramref name="sql"/> to the list of sql expressions.
		///		If <paramref name="sql"/> is null or empty - nothing will be added.
		/// </summary>
		/// <returns>Current instance (for chain calls).</returns>
		[NotNull]
		public SqlClause AddExpression ([CanBeNull] string sql)
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
		public SqlClause AddExpression ([NotNull] string key, [CanBeNull] string sql, bool overwrite = false)
		{
			if (!string.IsNullOrEmpty (sql))
				this.expressions.AddKeyed (key, sql, overwrite);

			return this;
		}


		/// <summary>
		///     Adds the specified <paramref name="sql"/> and <paramref name="parameter"/> to the list of sql expressions.
		///		If <paramref name="sql"/> is null or empty - nothing will be added.
		/// </summary>
		/// <returns>Current instance (for chain calls).</returns>
		[NotNull]
		public SqlClause AddExpression ([CanBeNull] string sql, [NotNull] IDbDataParameter parameter)
		{
			if (string.IsNullOrEmpty (sql))
				return this;

			this.expressions.AddSequenced (sql);
			this.AddParameter (parameter);

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
		public SqlClause AddExpression ([NotNull] string key, [CanBeNull] string sql, [NotNull] IDbDataParameter parameter, bool overwrite = false)
		{
			if (string.IsNullOrEmpty (sql))
				return this;

			if (this.ContainsExpression (key) && !overwrite)
				return this;

			this.expressions.AddKeyed (key, sql, overwrite);
			this.AddParameter (parameter);

			return this;
		}


		/// <summary>
		///     Adds specified <paramref name="sqlClause" /> by appending
		///     it sql representation as new expression and adding
		///     all parameters (overwriting existing with the same name).
		/// </summary>
		/// <returns>Current instance (for chain calls).</returns>
		[NotNull]
		public SqlClause AddClause ([NotNull] SqlClause sqlClause)
		{
			if (sqlClause == null)
				throw new ArgumentNullException ("sqlClause");

			this.AddExpression (sqlClause.GetSql ());

			foreach (var parameter in sqlClause.GetParameters ())
				this.AddParameter (parameter);

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
		public SqlClause AddClause ([NotNull] string key, [NotNull] SqlClause sqlClause, bool overwrite = false)
		{
			if (sqlClause == null)
				throw new ArgumentNullException ("sqlClause");

			if (this.ContainsExpression (key) && !overwrite)
				return this;

			this.AddExpression (key, sqlClause.GetSql (), overwrite);

			foreach (var parameter in sqlClause.GetParameters ())
				this.AddParameter (parameter);

			return this;
		}


		/// <summary>
		///     Renders the content of the clause to the string.
		///     If the clause is empty returns <see cref="string.Empty" />.
		/// </summary>
		[NotNull]
		public string GetSql ()
		{
			if (this.IsEmpty)
				return string.Empty;

			var result = this.Prefix + string.Join (this.Separator, this.expressions) + this.Suffix;

			return result;
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