using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	[TestClass]
	public class ExpressionsTests
	{
		[TestMethod]
		public void GetSql_NoPrefix_NoSuffix_NoSeparator_NoExpressions_ReturnsEmptyString ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void GetSql_NoPrefix_NoSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.AddExpression ("a");
			sut.AddExpression ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEquivalentToSql ("ab");
		}


		[TestMethod]
		public void GetSql_WithPrefix_NoSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Prefix = "prefix ";
			sut.AddExpression ("a");
			sut.AddExpression ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEquivalentToSql ("prefix ab");
		}


		[TestMethod]
		public void GetSql_NoPrefix_WithSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Suffix = " suffix";
			sut.AddExpression ("a");
			sut.AddExpression ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEquivalentToSql ("ab suffix");
		}


		[TestMethod]
		public void GetSql_NoPrefix_NoSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Separator = " and ";
			sut.AddExpression ("a");
			sut.AddExpression ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEquivalentToSql ("a and b");
		}


		[TestMethod]
		public void GetSql_WithPrefix_WithSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Prefix = "prefix ";
			sut.Separator = " and ";
			sut.Suffix = " suffix";
			sut.AddExpression ("a");
			sut.AddExpression ("b");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEquivalentToSql ("prefix a and b suffix");
		}


		[TestMethod]
		public void ToString_WithPrefix_WithSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Prefix = "prefix ";
			sut.Separator = " and ";
			sut.Suffix = " suffix";
			sut.AddExpression ("a");
			sut.AddExpression ("b");


			// act
			var result = sut.ToString ();


			// arrange
			result.Should ().BeEquivalentToSql ("prefix a and b suffix");
		}


		[TestMethod]
		public void AddKeyed_NoPreviousExpressions_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.AddExpression ("key", "a");


			// act
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEquivalentToSql ("a");
		}


		[TestMethod]
		public void AddKeyed_WithPreviousExpressionWithTheSameKey_ByDefault_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.AddExpression ("key", "a");


			// act
			sut.AddExpression ("key", "b");
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEquivalentToSql ("a");
		}


		[TestMethod]
		public void AddKeyed_WithPreviousExpressionWithTheSameKey_WithOverwrite_OverwritesTheExpression ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.AddExpression ("key", "a");


			// act
			sut.AddExpression ("key", "b", overwrite: true);
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEquivalentToSql ("b");
		}
	}
}