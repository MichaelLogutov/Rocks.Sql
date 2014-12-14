using System;
using System.Linq;
using Microsoft.VisualStudio.TextTemplating;

namespace Rocks.Sql.CodeGeneration
{
	public class PredicateGenerator
	{
		#region Private fields

		private readonly TextTransformation tt;
		private readonly PredicateGeneratorConfiguration configuration;

		#endregion

		#region Construct

		public PredicateGenerator (TextTransformation tt, PredicateGeneratorConfiguration configuration)
		{
			this.tt = tt;
			this.configuration = configuration;
		}

		#endregion

		#region Public methods

		/// <summary>
		///     Performs generation of code.
		/// </summary>
		public void Generate ()
		{
			if (this.configuration.GenerateEqualsMethods)
			{
				this.GenerateAddEqualsMethods ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddInMethods ();
			}

			if (this.configuration.GenerateGreaterOrLessMethods)
			{
				if (this.configuration.GenerateEqualsMethods)
					this.tt.WriteMethodsSeparator ();

				this.GenerateAddGreaterMethods ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddLessMethods ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddBetweenMethods ();
			}

			if (this.configuration.GenerateLikeMethods)
			{
				if (this.configuration.GenerateEqualsMethods || this.configuration.GenerateGreaterOrLessMethods)
					this.tt.WriteMethodsSeparator ();

				this.GenerateAddLikeMethods ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddStartsWithMethods ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddEndsWithMethods ();
			}
		}

		#endregion

		#region Private methods

		private void GeneratePredicateMethod (string commentOperand, string methodName)
		{
			var can_be_null_attribute = !this.configuration.IsNullable ? "[CanBeNull] " : string.Empty;

			this.tt.WriteLine ("/// <summary>");
			this.tt.WriteLine ("///     Adds \"<paramref name=\"columnName\" /> {0} <paramref name=\"parameterName\" />", commentOperand);
			this.tt.WriteLine ("///     expression to the clause.");
			this.tt.WriteLine ("///     If <paramref name=\"value\"/> is null then nothing will be added.");
			this.tt.WriteLine ("/// </summary>");
			this.tt.WriteLine ("[MethodImpl (MethodImplOptions.AggressiveInlining)]");

			this.tt.WriteAndPushIndent ("public static SqlClause {0} (", methodName);
			{
				this.tt.WriteLine ("this SqlClause sqlClause,");
				this.tt.WriteLine ("[NotNull] string columnName,");
				this.tt.WriteLine ("[NotNull] string parameterName,");
				this.tt.WriteLine ("{0}{1} value)", can_be_null_attribute, this.configuration.GetTypeDeclaration ());
			}
			this.tt.PopIndent ();

			this.tt.OpenScope ();
			{
				this.tt.WriteLine ("if (value == null)");
				this.tt.WriteLine ("    return sqlClause;");
				this.tt.WriteLine (string.Empty);

				this.tt.WriteAndPushIndent ("return sqlClause.{0} (columnName, ", methodName);
				this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("parameterName", "value") + ");");
				this.tt.PopIndent ();
			}
			this.tt.CloseScope ();
		}


		private void GenerateAddEqualsMethods ()
		{
			this.GeneratePredicateMethod ("=", "AddEquals");
			this.tt.WriteMethodsSeparator ();
			this.GeneratePredicateMethod ("&lt;&gt;", "AddNotEquals");
		}


		private void GenerateAddInMethods ()
		{
			this.GenerateAddInMethod (false);
			this.tt.WriteMethodsSeparator ();
			this.GenerateAddInMethod (true);
		}


		private void GenerateAddInMethod (bool not)
		{
			var method_name = !not ? "AddIn" : "AddNotIn";
			var comment_operand = !not ? "in" : "not in";

			Action write_comment = () =>
			{
				this.tt.WriteLine ("/// <summary>");
				this.tt.WriteLine ("///     Adds \"<paramref name=\"columnName\" /> {0} (<paramref name=\"parameterName\" />1, ", comment_operand);
				this.tt.WriteLine ("///     <paramref name=\"parameterName\" />2, ...) expression to the clause.");
				this.tt.WriteLine ("///     If <paramref name=\"values\"/> is null or contains no elements then nothing will be added.");
				this.tt.WriteLine ("/// </summary>");
			};

			write_comment ();
			this.tt.WriteLine ("[MethodImpl (MethodImplOptions.AggressiveInlining)]");
			this.tt.WriteAndPushIndent ("public static SqlClause {0} (", method_name);
			{
				this.tt.WriteLine ("this SqlClause sqlClause,");
				this.tt.WriteLine ("[NotNull] string columnName,");
				this.tt.WriteLine ("[NotNull] string parameterName,");
				this.tt.WriteLine ("[CanBeNull] IEnumerable<{0}> values)", this.configuration.Type);
			}
			this.tt.PopIndent ();

			this.tt.OpenScope ();
			{
				this.tt.WriteLine ("if (values == null)");
				this.tt.WriteLine ("    return sqlClause;");

				this.tt.WriteLine (string.Empty);
				this.tt.WriteAndPushIndent ("var parameters = values.Select ((value, index) => ");
				this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("parameterName + (index + 1)", "value") + ");");
				this.tt.PopIndent ();

				this.tt.WriteLine (string.Empty);
				this.tt.WriteLine ("return sqlClause.{0} (columnName, parameters);", method_name);
			}
			this.tt.CloseScope ();


			this.tt.WriteMethodsSeparator ();

			write_comment ();
			this.tt.WriteLine ("[MethodImpl (MethodImplOptions.AggressiveInlining)]");
			this.tt.WriteAndPushIndent ("public static SqlClause {0} (", method_name);
			{
				this.tt.WriteLine ("this SqlClause sqlClause,");
				this.tt.WriteLine ("[NotNull] string columnName,");
				this.tt.WriteLine ("[NotNull] string parameterName,");
				this.tt.WriteLine ("[CanBeNull] params {0}[] values)", this.configuration.Type);
			}
			this.tt.PopIndent ();
			this.tt.OpenScope ();
			{
				this.tt.WriteLine ("return sqlClause.{0} (columnName, parameterName, (IEnumerable<{1}>) values);",
				                   method_name,
				                   this.configuration.Type);
			}
			this.tt.CloseScope ();
		}


		private void GenerateAddGreaterMethods ()
		{
			this.GeneratePredicateMethod ("&gt;", "AddGreater");
			this.tt.WriteMethodsSeparator ();
			this.GeneratePredicateMethod ("&gt;=", "AddGreaterOrEquals");
		}


		private void GenerateAddLessMethods ()
		{
			this.GeneratePredicateMethod ("&gt;", "AddLess");
			this.tt.WriteMethodsSeparator ();
			this.GeneratePredicateMethod ("&gt;=", "AddLessOrEquals");
		}


		private void GenerateAddBetweenMethods ()
		{
			this.GenerateAddBetweenMethod (false);
			this.tt.WriteMethodsSeparator ();
			this.GenerateAddBetweenMethod (true);
		}


		private void GenerateAddBetweenMethod (bool not)
		{
			var comment_sql_operand = !not ? "between" : "not between";
			var comment_sql_operand_if_value2_null = !not ? "&gt;=" : "&lt;";
			var comment_sql_operand_if_value_null = !not ? "&lt;=" : "&gt;";
			var method_name = !not ? "AddBetween" : "AddNotBetween";
			var can_be_null_attribute = !this.configuration.IsNullable ? "[CanBeNull] " : string.Empty;

			this.tt.WriteLine ("/// <summary>");
			this.tt.WriteLine ("///     <para>");
			this.tt.WriteLine ("///         Adds \"<paramref name=\"columnName\" /> {0} <paramref name=\"parameterName\" />",
			                   comment_sql_operand);

			this.tt.WriteLine ("///         and <paramref name=\"parameterName2\" />\" expression to the clause.");
			this.tt.WriteLine ("///     </para>");
			this.tt.WriteLine ("///     <para>");
			this.tt.WriteLine ("///         If <paramref name=\"value2\" /> is null then adds \"<paramref name=\"columnName\" /> " +
			                   comment_sql_operand_if_value2_null);

			this.tt.WriteLine ("///         <paramref name=\"parameterName\" />\" expression to the clause.");
			this.tt.WriteLine ("///     </para>");
			this.tt.WriteLine ("///		<para>");
			this.tt.WriteLine ("///         If <paramref name=\"value\" /> is null then adds \"<paramref name=\"columnName\" /> " +
			                   comment_sql_operand_if_value_null);

			this.tt.WriteLine ("///         <paramref name=\"parameterName2\" />\" expression to the clause.");
			this.tt.WriteLine ("///     </para>");
			this.tt.WriteLine ("///     <para>");
			this.tt.WriteLine ("///         If both <paramref name=\"value\" /> and <paramref name=\"value2\" /> are null");
			this.tt.WriteLine ("///         then nothing will be added.");
			this.tt.WriteLine ("///     </para>");
			this.tt.WriteLine ("/// </summary>");
			this.tt.WriteLine ("[MethodImpl (MethodImplOptions.AggressiveInlining)]");

			this.tt.WriteAndPushIndent ("public static SqlClause {0} (", method_name);
			{
				this.tt.WriteLine ("this SqlClause sqlClause,");
				this.tt.WriteLine ("[NotNull] string columnName,");
				this.tt.WriteLine ("[NotNull] string parameterName,");
				this.tt.WriteLine ("{0}{1} value,", can_be_null_attribute, this.configuration.GetTypeDeclaration ());
				this.tt.WriteLine ("[NotNull] string parameterName2,");
				this.tt.WriteLine ("{0}{1} value2)", can_be_null_attribute, this.configuration.GetTypeDeclaration ());
			}
			this.tt.PopIndent ();

			this.tt.OpenScope ();
			{
				this.tt.WriteLine ("if (value == null && value2 == null)");
				this.tt.WriteLine ("    return sqlClause;");

				this.tt.WriteLine (string.Empty);
				this.tt.WriteAndPushIndent ("var parameter = value == null ? null : ");
				this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("parameterName", "value") + ";");
				this.tt.PopIndent ();

				this.tt.WriteLine (string.Empty);
				this.tt.WriteAndPushIndent ("var parameter2 = value2 == null ? null : ");
				this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("parameterName2", "value2") + ";");
				this.tt.PopIndent ();

				this.tt.WriteLine (string.Empty);
				this.tt.WriteLine ("return sqlClause.{0} (columnName, parameter, parameter2);", method_name);
			}
			this.tt.CloseScope ();
		}


		private void GenerateAddLikeMethods ()
		{
			this.GeneratePredicateMethod ("like", "AddLike");
			this.tt.WriteMethodsSeparator ();
			this.GeneratePredicateMethod ("not like", "AddNotLike");
		}


		private void GenerateAddStartsWithMethods ()
		{
			this.GenerateAddStartsWithMethods (false);
			this.tt.WriteMethodsSeparator ();
			this.GenerateAddStartsWithMethods (true);
		}


		private void GenerateAddStartsWithMethods (bool not)
		{
			var comment_operand = !not ? "like" : "not like";
			var method_name = !not ? "AddStartsWith" : "AddNotStartsWith";
			var add_method_name = !not ? "AddLike" : "AddNotLike";

			this.tt.WriteLine ("/// <summary>");
			this.tt.WriteLine ("///     Adds \"<paramref name=\"columnName\" /> {0} <paramref name=\"parameterName\" />", comment_operand);
			this.tt.WriteLine ("///     expression to the clause. The <paramref name=\"value\"/> parameter is escaped");
			this.tt.WriteLine ("///     and appended with the '%' sign.");
			this.tt.WriteLine ("///     If <paramref name=\"value\"/> is null or empty string then nothing will be added.");
			this.tt.WriteLine ("/// </summary>");
			this.tt.WriteLine ("[MethodImpl (MethodImplOptions.AggressiveInlining)]");

			this.tt.WriteAndPushIndent ("public static SqlClause {0} (", method_name);
			{
				this.tt.WriteLine ("this SqlClause sqlClause,");
				this.tt.WriteLine ("[NotNull] string columnName,");
				this.tt.WriteLine ("[NotNull] string parameterName,");
				this.tt.WriteLine ("[CanBeNull] string value)");
			}
			this.tt.PopIndent ();

			this.tt.OpenScope ();
			{
				this.tt.WriteLine ("if (string.IsNullOrEmpty (value))");
				this.tt.WriteLine ("    return sqlClause;");

				this.tt.WriteLine (string.Empty);

				this.tt.WriteAndPushIndent ("return sqlClause.{0} (columnName, ", add_method_name);
				this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("parameterName", "value.Replace (\"%\", \"\\\\%\") + \"%\"") + ");");
				this.tt.PopIndent ();
			}
			this.tt.CloseScope ();
		}


		private void GenerateAddEndsWithMethods ()
		{
			this.GenerateAddEndsWithMethods (false);
			this.tt.WriteMethodsSeparator ();
			this.GenerateAddEndsWithMethods (true);
		}


		private void GenerateAddEndsWithMethods (bool not)
		{
			var comment_operand = !not ? "like" : "not like";
			var method_name = !not ? "AddEndsWith" : "AddNotEndsWith";
			var add_method_name = !not ? "AddLike" : "AddNotLike";

			this.tt.WriteLine ("/// <summary>");
			this.tt.WriteLine ("///     Adds \"<paramref name=\"columnName\" /> {0} <paramref name=\"parameterName\" />", comment_operand);
			this.tt.WriteLine ("///     expression to the clause. The <paramref name=\"value\"/> parameter is escaped");
			this.tt.WriteLine ("///     and prepended with the '%' sign.");
			this.tt.WriteLine ("///     If <paramref name=\"value\"/> is null or empty string then nothing will be added.");
			this.tt.WriteLine ("/// </summary>");
			this.tt.WriteLine ("[MethodImpl (MethodImplOptions.AggressiveInlining)]");

			this.tt.WriteAndPushIndent ("public static SqlClause {0} (", method_name);
			{
				this.tt.WriteLine ("this SqlClause sqlClause,");
				this.tt.WriteLine ("[NotNull] string columnName,");
				this.tt.WriteLine ("[NotNull] string parameterName,");
				this.tt.WriteLine ("[CanBeNull] string value)");
			}
			this.tt.PopIndent ();

			this.tt.OpenScope ();
			{
				this.tt.WriteLine ("if (string.IsNullOrEmpty (value))");
				this.tt.WriteLine ("    return sqlClause;");

				this.tt.WriteLine (string.Empty);

				this.tt.WriteAndPushIndent ("return sqlClause.{0} (columnName, ", add_method_name);
				this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("parameterName", "\"%\" + value.Replace (\"%\", \"\\\\%\")") + ");");
				this.tt.PopIndent ();
			}
			this.tt.CloseScope ();
		}

		#endregion
	}
}