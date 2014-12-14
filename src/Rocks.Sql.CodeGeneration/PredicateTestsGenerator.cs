using System;
using System.Linq;
using Microsoft.VisualStudio.TextTemplating;

namespace Rocks.Sql.CodeGeneration
{
	public class PredicateTestsGenerator
	{
		#region Private fields

		private readonly TextTransformation tt;
		private readonly PredicateGeneratorConfiguration configuration;

		#endregion

		#region Construct

		public PredicateTestsGenerator (TextTransformation tt, PredicateGeneratorConfiguration configuration)
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
				this.GenerateAddEqualsTests ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddInTests ();
			}

			if (this.configuration.GenerateGreaterOrLessMethods)
			{
				if (this.configuration.GenerateEqualsMethods)
					this.tt.WriteMethodsSeparator ();

				this.GenerateAddGreaterTests ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddLessTests ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddBetweenTests ();
			}

			if (this.configuration.GenerateLikeMethods)
			{
				if (this.configuration.GenerateEqualsMethods || this.configuration.GenerateGreaterOrLessMethods)
					this.tt.WriteMethodsSeparator ();

				this.GenerateAddLikeTests ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddStartsWithTests ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddEndsWithTests ();
			}
		}

		#endregion

		#region Private methods

		private void GenerateTest (string methodName, Action arrange, Action act, Action assert)
		{
			this.tt.WriteLine ("[TestMethod]");
			this.tt.WriteLine ("public void {0} ()", methodName);
			this.tt.WriteLine ("{");
			this.tt.PushIndent ("    ");
			{
				this.tt.WriteLine ("// arrange");
				arrange ();

				this.tt.WriteMethodsSeparator ();
				this.tt.WriteLine ("// act");
				act ();

				this.tt.WriteMethodsSeparator ();
				this.tt.WriteLine ("// assert");
				assert ();
			}
			this.tt.PopIndent ();
			this.tt.WriteLine ("}");
		}


		private void GeneratePredicateMethodTests (string testedMethodName, string expectedSqlOperand)
		{
			this.GenerateTest
				(string.Format ("{0}_ByDefault_AddsCorrectPredicate", testedMethodName),
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("var fixture = new FixtureBuilder ().Build ();");
					 this.tt.WriteLine ("var value = fixture.Create<{0}> ();", this.configuration.GetTypeDeclaration ());
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value);", testedMethodName);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (\"Id {0} @x\");", expectedSqlOperand);
					 this.tt.WriteAndPushIndent ("parameters.ShouldAllBeEquivalentTo (");
					 {
						 this.tt.WriteLine ("new[]");
						 this.tt.WriteLine ("{");
						 this.tt.PushIndent ("    ");
						 {
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x\"", "value"));
						 }
						 this.tt.PopIndent ();
						 this.tt.WriteLine ("});");
					 }
					 this.tt.PopIndent ();
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(string.Format ("{0}_Null_AddsNothing", testedMethodName),
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("{0} value = null;", this.configuration.GetTypeDeclaration ());
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value);", testedMethodName);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (string.Empty);");
					 this.tt.WriteLine ("parameters.Should ().BeEmpty ();");
				 });
		}


		private void GenerateAddEqualsTests ()
		{
			this.GeneratePredicateMethodTests ("AddEquals", "=");
			this.tt.WriteMethodsSeparator ();
			this.GeneratePredicateMethodTests ("AddNotEquals", "<>");
		}


		private void GenerateAddInTests ()
		{
			this.GenerateAddInTests (false);
			this.tt.WriteMethodsSeparator ();
			this.GenerateAddInTests (true);
		}


		private void GenerateAddInTests (bool not)
		{
			var tested_method_name = !not ? "AddIn" : "AddNotIn";
			var expected_sql_operand = !not ? "in" : "not in";

			this.GenerateTest
				(string.Format ("{0}_TwoParameters_AddsCorrectPredicate", tested_method_name),
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("var fixture = new FixtureBuilder ().Build ();");
					 this.tt.WriteLine ("var value1 = fixture.Create<{0}> ();", this.configuration.Type);
					 this.tt.WriteLine ("var value2 = fixture.Create<{0}> ();", this.configuration.Type);
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value1, value2);", tested_method_name);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (\"Id {0} (@x1, @x2)\");", expected_sql_operand);
					 this.tt.WriteAndPushIndent ("parameters.ShouldAllBeEquivalentTo (");
					 {
						 this.tt.WriteLine ("new[]");
						 this.tt.WriteLine ("{");
						 this.tt.PushIndent ("    ");
						 {
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x1\"", "value1") + ",");
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x2\"", "value2"));
						 }
						 this.tt.PopIndent ();
						 this.tt.WriteLine ("});");
					 }
					 this.tt.PopIndent ();
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(string.Format ("{0}_NoParameters_AddsNothing", tested_method_name),
				 arrange: () => { this.tt.WriteLine ("var sut = new SqlClause ();"); },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", new {1}[0]);", tested_method_name, this.configuration.Type);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (string.Empty);");
					 this.tt.WriteLine ("parameters.Should ().BeEmpty ();");
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(string.Format ("{0}_Null_AddsNothing", tested_method_name),
				 arrange: () => { this.tt.WriteLine ("var sut = new SqlClause ();"); },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", (IEnumerable<{1}>) null);", tested_method_name, this.configuration.Type);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (string.Empty);");
					 this.tt.WriteLine ("parameters.Should ().BeEmpty ();");
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(string.Format ("{0}_OneParameter_AddsCorrectPredicate", tested_method_name),
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("var fixture = new FixtureBuilder ().Build ();");
					 this.tt.WriteLine ("var value = fixture.Create<{0}> ();", this.configuration.Type);
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", new[] {{ value }}.Select (x => x));", tested_method_name);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (\"Id {0} (@x1)\");", expected_sql_operand);
					 this.tt.WriteAndPushIndent ("parameters.ShouldAllBeEquivalentTo (");
					 {
						 this.tt.WriteLine ("new[]");
						 this.tt.WriteLine ("{");
						 this.tt.PushIndent ("    ");
						 {
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x1\"", "value"));
						 }
						 this.tt.PopIndent ();
						 this.tt.WriteLine ("});");
					 }
					 this.tt.PopIndent ();
				 });
		}


		private void GenerateAddGreaterTests ()
		{
			this.GeneratePredicateMethodTests ("AddGreater", ">");
			this.tt.WriteMethodsSeparator ();
			this.GeneratePredicateMethodTests ("AddGreaterOrEquals", ">=");
		}


		private void GenerateAddLessTests ()
		{
			this.GeneratePredicateMethodTests ("AddLess", "<");
			this.tt.WriteMethodsSeparator ();
			this.GeneratePredicateMethodTests ("AddLessOrEquals", "<=");
		}


		private void GenerateAddBetweenTests ()
		{
			this.GenerateAddBetweenTests (false);
			this.tt.WriteMethodsSeparator ();
			this.GenerateAddBetweenTests (true);
		}


		private void GenerateAddBetweenTests (bool not)
		{
			var method_name = !not ? "AddBetween" : "AddNotBetween";
			var expected_sql_operand = !not ? "between" : "not between";
			var expected_sql_operand_value_is_null = !not ? "<=" : ">";
			var expected_sql_operand_value2_is_null = !not ? ">=" : "<";

			this.GenerateTest
				(method_name + "_ByDefault_AddsCorrectPredicate",
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("var fixture = new FixtureBuilder ().Build ();");
					 this.tt.WriteLine ("var value = fixture.Create<{0}> ();", this.configuration.GetTypeDeclaration ());
					 this.tt.WriteLine ("var value2 = fixture.Create<{0}> ();", this.configuration.GetTypeDeclaration ());
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value, \"@x2\", value2);", method_name);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (\"Id {0} @x and @x2\");", expected_sql_operand);
					 this.tt.WriteAndPushIndent ("parameters.ShouldAllBeEquivalentTo (");
					 {
						 this.tt.WriteLine ("new[]");
						 this.tt.WriteLine ("{");
						 this.tt.PushIndent ("    ");
						 {
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x\"", "value") + ",");
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x2\"", "value2"));
						 }
						 this.tt.PopIndent ();
						 this.tt.WriteLine ("});");
					 }
					 this.tt.PopIndent ();
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(method_name + "_TheFirstParameterIsNull_AddsCorrectPredicate",
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("var fixture = new FixtureBuilder ().Build ();");
					 this.tt.WriteLine ("{0} value = null;", this.configuration.GetTypeDeclaration ());
					 this.tt.WriteLine ("var value2 = fixture.Create<{0}> ();", this.configuration.GetTypeDeclaration ());
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value, \"@x2\", value2);", method_name);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (\"Id {0} @x2\");", expected_sql_operand_value_is_null);
					 this.tt.WriteAndPushIndent ("parameters.ShouldAllBeEquivalentTo (");
					 {
						 this.tt.WriteLine ("new[]");
						 this.tt.WriteLine ("{");
						 this.tt.PushIndent ("    ");
						 {
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x2\"", "value2"));
						 }
						 this.tt.PopIndent ();
						 this.tt.WriteLine ("});");
					 }
					 this.tt.PopIndent ();
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(method_name + "_TheSecondParameterIsNull_AddsCorrectPredicate",
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("var fixture = new FixtureBuilder ().Build ();");
					 this.tt.WriteLine ("var value = fixture.Create<{0}> ();", this.configuration.GetTypeDeclaration ());
					 this.tt.WriteLine ("{0} value2 = null;", this.configuration.GetTypeDeclaration ());
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value, \"@x2\", value2);", method_name);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (\"Id {0} @x\");", expected_sql_operand_value2_is_null);
					 this.tt.WriteAndPushIndent ("parameters.ShouldAllBeEquivalentTo (");
					 {
						 this.tt.WriteLine ("new[]");
						 this.tt.WriteLine ("{");
						 this.tt.PushIndent ("    ");
						 {
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x\"", "value"));
						 }
						 this.tt.PopIndent ();
						 this.tt.WriteLine ("});");
					 }
					 this.tt.PopIndent ();
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(method_name + "_BothParametersAreNull_AddsNothing",
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("{0} value = null;", this.configuration.GetTypeDeclaration ());
					 this.tt.WriteLine ("{0} value2 = null;", this.configuration.GetTypeDeclaration ());
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value, \"@x2\", value2);", method_name);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (string.Empty);");
					 this.tt.WriteLine ("parameters.Should ().BeEmpty ();");
				 });
		}


		private void GenerateAddLikeTests ()
		{
			this.GeneratePredicateMethodTests ("AddLike", "like");
			this.tt.WriteMethodsSeparator ();
			this.GeneratePredicateMethodTests ("AddNotLike", "not like");
		}


		private void GenerateAddStartsWithTests ()
		{
			this.GenerateAddStartsEndsWithTests ("AddStartsWith", "a%b", "like", "a\\\\%b%");
			this.tt.WriteMethodsSeparator ();
			this.GenerateAddStartsEndsWithTests ("AddNotStartsWith", "a%b", "not like", "a\\\\%b%");
		}


		private void GenerateAddEndsWithTests ()
		{
			this.GenerateAddStartsEndsWithTests ("AddEndsWith", "a%b", "like", "%a\\\\%b");
			this.tt.WriteMethodsSeparator ();
			this.GenerateAddStartsEndsWithTests ("AddNotEndsWith", "a%b", "not like", "%a\\\\%b");
		}


		private void GenerateAddStartsEndsWithTests (string testedMethodName, string value, string expectedSqlOperand, string expectedValue)
		{
			this.GenerateTest
				(string.Format ("{0}_ByDefault_AddsCorrectPredicate", testedMethodName),
				 arrange: () => { this.tt.WriteLine ("var sut = new SqlClause ();"); },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", \"{1}\");", testedMethodName, value);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (\"Id {0} @x\");", expectedSqlOperand);
					 this.tt.WriteAndPushIndent ("parameters.ShouldAllBeEquivalentTo (");
					 {
						 this.tt.WriteLine ("new[]");
						 this.tt.WriteLine ("{");
						 this.tt.PushIndent ("    ");
						 {
							 this.tt.WriteLine (this.configuration.GetCreateDbParameterCode ("\"@x\"", "\"" + expectedValue + "\""));
						 }
						 this.tt.PopIndent ();
						 this.tt.WriteLine ("});");
					 }
					 this.tt.PopIndent ();
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(string.Format ("{0}_EmptyString_AddsNothing", testedMethodName),
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("var value = string.Empty;");
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value);", testedMethodName);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (string.Empty);");
					 this.tt.WriteLine ("parameters.Should ().BeEmpty ();");
				 });

			this.tt.WriteMethodsSeparator ();

			this.GenerateTest
				(string.Format ("{0}_Null_AddsNothing", testedMethodName),
				 arrange: () =>
				 {
					 this.tt.WriteLine ("var sut = new SqlClause ();");
					 this.tt.WriteLine ("string value = null;");
				 },
				 act: () =>
				 {
					 this.tt.WriteLine ("sut.{0} (\"Id\", \"@x\", value);", testedMethodName);
					 this.tt.WriteLine ("var sql = sut.GetSql ();");
					 this.tt.WriteLine ("var parameters = sut.GetParameters ();");
				 },
				 assert: () =>
				 {
					 this.tt.WriteLine ("sql.Should ().Be (string.Empty);");
					 this.tt.WriteLine ("parameters.Should ().BeEmpty ();");
				 });
		}

		#endregion
	}
}