using System;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	[TestClass]
	public class AddClauseTests
	{
		[TestMethod]
		public void ToEmptyClause_AddingEmptyClause_ReturnsEmptyString ()
		{
			// arrange
			var sut = new SqlClause ();

			var clause = new SqlClause ();


			// act
			sut.AddClause (clause);
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void ToEmptyClause_AddingNotEmptyClause_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			var clause = new SqlClause ();
			clause.AddExpression ("a");


			// act
			sut.AddClause (clause);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a");
		}


		[TestMethod]
		public void ToNotEmptyClause_AddingNotEmptyClause_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();
			sut.AddExpression ("a");

			var clause = new SqlClause ();
			clause.AddExpression ("b");


			// act
			sut.AddClause (clause);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("ab");
		}


		[TestMethod]
		public void WithKey_HasExpressionWithTheSameKey_ByDefault_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();
			sut.AddExpression ("key", "a");

			var clause = new SqlClause ();
			clause.AddExpression ("b");


			// act
			sut.AddClause ("key", clause);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a");
		}


		[TestMethod]
		public void WithKey_HasExpressionWithTheSameKey_WithOverwrite_OverwritesTheExpression ()
		{
			// arrange
			var sut = new SqlClause ();
			sut.AddExpression ("key", "a");

			var clause = new SqlClause ();
			clause.AddExpression ("b");


			// act
			sut.AddClause ("key", clause, overwrite: true);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("b");
		}


		[TestMethod]
		public void ToEmptyClause_AddingEmptyClause_ReturnsEmptyParametersList ()
		{
			// arrange
			var sut = new SqlClause ();

			var clause = new SqlClause ();


			// act
			sut.AddClause (clause);
			var result = sut.GetParameters ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void ToEmptyClause_AddingNotEmptyClause_ReturnsCorrectParametersList ()
		{
			// arrange
			var sut = new SqlClause ();


			var parameter = new SqlParameter { ParameterName = "@name" };

			var clause = new SqlClause ();
			clause.AddParameter (parameter);


			// act
			sut.AddClause (clause);
			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter);
		}


		[TestMethod]
		public void ToNotEmptyClause_AddingNotEmptyClause_ReturnsCorrectParametersList ()
		{
			// arrange
			var sut = new SqlClause ();


			var parameter = new SqlParameter { ParameterName = "@name" };
			var parameter2 = new SqlParameter { ParameterName = "@name2" };

			sut.AddParameter (parameter);

			var clause = new SqlClause ();
			clause.AddParameter (parameter2);


			// act
			sut.AddClause (clause);
			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter, parameter2);
		}


		[TestMethod]
		public void HasTheParameterWithTheSameName_OverwritesIt ()
		{
			// arrange
			var sut = new SqlClause ();


			var parameter = new SqlParameter { ParameterName = "@name" };
			var parameter2 = new SqlParameter { ParameterName = "@name" };

			sut.AddParameter (parameter);

			var clause = new SqlClause ();
			clause.AddParameter (parameter2);


			// act
			sut.AddClause (clause);
			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter2);
		}
	}
}