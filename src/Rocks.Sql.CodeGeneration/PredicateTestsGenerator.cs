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

				this.tt.WriteNewLines (2);
				this.GenerateAddNotEqualsTests ();
			}

			if (this.configuration.GenerateGreaterOrLessMethods)
			{
				if (this.configuration.GenerateEqualsMethods)
					this.tt.WriteNewLines (2);

				this.GenerateAddGreaterTests ();

				this.tt.WriteNewLines (2);
				this.GenerateAddGreaterOrEqualsTests ();

				this.tt.WriteNewLines (2);
				this.GenerateAddLessTests ();

				this.tt.WriteNewLines (2);
				this.GenerateAddLessOrEqualsTests ();

				this.tt.WriteNewLines (2);
				this.GenerateAddBetweenTests (false);

				this.tt.WriteNewLines (2);
				this.GenerateAddBetweenTests (true);
			}

			if (this.configuration.GenerateLikeMethods)
			{
				if (this.configuration.GenerateEqualsMethods || this.configuration.GenerateGreaterOrLessMethods)
					this.tt.WriteMethodsSeparator ();

				this.GenerateAddLikeTests ();

				this.tt.WriteMethodsSeparator ();
				this.GenerateAddNotLikeTests ();
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

				this.tt.WriteNewLines (2);
				this.tt.WriteLine ("// act");
				act ();

				this.tt.WriteNewLines (2);
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

			this.tt.WriteNewLines (2);

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
		}


		private void GenerateAddNotEqualsTests ()
		{
			this.GeneratePredicateMethodTests ("AddNotEquals", "<>");
		}


		private void GenerateAddGreaterTests ()
		{
			this.GeneratePredicateMethodTests ("AddGreater", ">");
		}


		private void GenerateAddGreaterOrEqualsTests ()
		{
			this.GeneratePredicateMethodTests ("AddGreaterOrEquals", ">=");
		}


		private void GenerateAddLessTests ()
		{
			this.GeneratePredicateMethodTests ("AddLess", "<");
		}


		private void GenerateAddLessOrEqualsTests ()
		{
			this.GeneratePredicateMethodTests ("AddLessOrEquals", "<=");
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

			this.tt.WriteNewLines (2);

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

			this.tt.WriteNewLines (2);

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

			this.tt.WriteNewLines (2);

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
		}


		private void GenerateAddNotLikeTests ()
		{
			this.GeneratePredicateMethodTests ("AddNotLike", "not like");
		}

		#endregion
	}
}