using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	[TestClass]
	public class ExpressionsTests
	{
		[TestMethod]
		public void GetSql_NoPrefix_NoSuffix_NoSeparator_NoExpressions_ByDefault_ReturnsEmptyString ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void GetSql_WithPrefix_WithSuffix_WithSeparator_NoExpressions_ByDefault_ReturnsEmptyString ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Prefix = "prefix ";
			sut.ExpressionsSeparator = " and ";
			sut.Suffix = " suffix";


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void GetSql_WithPrefix_WithSuffix_WithSeparator_NoExpressions_WithRenderIfEmpty_ReturnsEmptyString ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Prefix = "prefix ";
			sut.ExpressionsSeparator = " and ";
			sut.Suffix = " suffix";
			sut.RenderIfEmpty = true;


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("prefix  suffix");
		}


		[TestMethod]
		public void GetSql_NoPrefix_NoSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("a");
			sut.Add ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("ab");
		}


		[TestMethod]
		public void GetSql_WithPrefix_NoSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Prefix = "prefix ";
			sut.Add ("a");
			sut.Add ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("prefix ab");
		}


		[TestMethod]
		public void GetSql_NoPrefix_WithSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Suffix = " suffix";
			sut.Add ("a");
			sut.Add ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("ab suffix");
		}


		[TestMethod]
		public void GetSql_NoPrefix_NoSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.ExpressionsSeparator = " and ";
			sut.Add ("a");
			sut.Add ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a and b");
		}


		[TestMethod]
		public void GetSql_WithPrefix_WithSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Prefix = "prefix ";
			sut.ExpressionsSeparator = " and ";
			sut.Suffix = " suffix";
			sut.Add ("a");
			sut.Add ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("prefix a and b suffix");
		}


		[TestMethod]
		public void ToString_WithPrefix_WithSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Prefix = "prefix ";
			sut.ExpressionsSeparator = " and ";
			sut.Suffix = " suffix";
			sut.Add ("a");
			sut.Add ("b");


			// act
			var result = sut.ToString ();


			// arrange
			result.Should ().Be ("prefix a and b suffix");
		}


		[TestMethod]
		public void AddKeyed_NoPreviousExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("key", "a");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a");
		}


		[TestMethod]
		public void AddKeyed_WithPreviousExpressionWithTheSameKey_ByDefault_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("key", "a");


			// act
			sut.Add ("key", "b");
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a");
		}


		[TestMethod]
		public void AddKeyed_WithPreviousExpressionWithTheSameKey_WithOverwrite_OverwritesTheExpression ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("key", "a");


			// act
			sut.Add ("key", "b", overwrite: true);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("b");
		}
	}
}