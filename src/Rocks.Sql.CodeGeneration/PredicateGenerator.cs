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
				this.GenerateAddEqualsMethod ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddNotEqualsMethod ();
			}

			if (this.configuration.GenerateGreaterOrLessMethods)
			{
				if (this.configuration.GenerateEqualsMethods)
					this.tt.WriteMethodsSeparator ();

				this.GenerateAddGreaterMethod ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddGreaterOrEqualsMethod ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddLessMethod ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddLessOrEqualsMethod ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddBetweenMethod (false);

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddBetweenMethod (true);
			}

			if (this.configuration.GenerateLikeMethods)
			{
				if (this.configuration.GenerateEqualsMethods || this.configuration.GenerateGreaterOrLessMethods)
					this.tt.WriteMethodsSeparator ();

				this.GenerateAddLikeMethod ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddNotLikeMethod ();
			}
		}

		#endregion

		#region Private methods

		private void GeneratePredicateMethod (string commentOperand, string methodName)
		{
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
				this.tt.WriteLine ("{0} value)", this.configuration.GetTypeDeclaration ());
			}
			this.tt.PopIndent ();

			this.tt.WriteLine ("{");
			this.tt.PushIndent ("    ");
			{
				this.tt.WriteLine ("if (value == null)");
				this.tt.WriteLine ("    return sqlClause;");
				this.tt.WriteLine (string.Empty);

				this.tt.WriteAndPushIndent ("return sqlClause.{0} (columnName, ", methodName);
				this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("parameterName", "value") + ");");
				this.tt.PopIndent ();
			}
			this.tt.PopIndent ();
			this.tt.WriteLine ("}");
		}


		private void GenerateAddEqualsMethod ()
		{
			this.GeneratePredicateMethod ("=", "AddEquals");
		}


		private void GenerateAddNotEqualsMethod ()
		{
			this.GeneratePredicateMethod ("&lt;&gt;", "AddNotEquals");
		}


		private void GenerateAddGreaterMethod ()
		{
			this.GeneratePredicateMethod ("&gt;", "AddGreater");
		}


		private void GenerateAddGreaterOrEqualsMethod ()
		{
			this.GeneratePredicateMethod ("&gt;=", "AddGreaterOrEquals");
		}


		private void GenerateAddLessMethod ()
		{
			this.GeneratePredicateMethod ("&gt;", "AddLess");
		}


		private void GenerateAddLessOrEqualsMethod ()
		{
			this.GeneratePredicateMethod ("&gt;=", "AddLessOrEquals");
		}


		private void GenerateAddBetweenMethod (bool not)
		{
			var comment_sql_operand = !not ? "between" : "not between";
			var comment_sql_operand_if_value2_null = !not ? "&gt;=" : "&lt;";
			var comment_sql_operand_if_value_null = !not ? "&lt;=" : "&gt;";
			var method_name = !not ? "AddBetween" : "AddNotBetween";

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
				this.tt.WriteLine ("{0} value,", this.configuration.GetTypeDeclaration ());
				this.tt.WriteLine ("[NotNull] string parameterName2,");
				this.tt.WriteLine ("{0} value2)", this.configuration.GetTypeDeclaration ());
			}
			this.tt.PopIndent ();

			this.tt.WriteLine ("{");
			this.tt.PushIndent ("    ");
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
			this.tt.PopIndent ();
			this.tt.WriteLine ("}");
		}


		private void GenerateAddLikeMethod ()
		{
			this.GeneratePredicateMethod ("like", "AddLike");
		}


		private void GenerateAddNotLikeMethod ()
		{
			this.GeneratePredicateMethod ("not like", "AddNotLike");
		}

		#endregion
	}
}