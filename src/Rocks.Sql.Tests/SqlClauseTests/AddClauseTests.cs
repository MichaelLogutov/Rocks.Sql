using System;
using System.Data.SqlClient;
using FluentAssertions;
using Xunit;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	public class AddClauseTests
	{
		[Fact]
		public void ToEmptyClause_AddingEmptyClause_ReturnsEmptyString ()
		{
			// arrange
			var sut = new SqlClause ();

			var clause = new SqlClause ();


			// act
			sut.Add (clause);
			var result = sut.GetSql ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[Fact]
		public void ToEmptyClause_AddingNotEmptyClause_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			var clause = new SqlClause ();
			clause.Add ("a");


			// act
			sut.Add (clause);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a");
		}


		[Fact]
		public void ToNotEmptyClause_AddingNotEmptyClause_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();
			sut.Add ("a");

			var clause = new SqlClause ();
			clause.Add ("b");


			// act
			sut.Add (clause);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("ab");
		}


		[Fact]
		public void WithKey_HasExpressionWithTheSameKey_ByDefault_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();
			sut.Add ("key", "a");

			var clause = new SqlClause ();
			clause.Add ("b");


			// act
			sut.Add ("key", clause);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a");
		}


		[Fact]
		public void WithKey_HasExpressionWithTheSameKey_WithOverwrite_OverwritesTheExpression ()
		{
			// arrange
			var sut = new SqlClause ();
			sut.Add ("key", "a");

			var clause = new SqlClause ();
			clause.Add ("b");


			// act
			sut.Add ("key", clause, overwrite: true);
			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("b");
		}


		[Fact]
		public void ToEmptyClause_AddingEmptyClause_ReturnsEmptyParametersList ()
		{
			// arrange
			var sut = new SqlClause ();

			var clause = new SqlClause ();


			// act
			sut.Add (clause);
			var result = sut.GetParameters ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[Fact]
		public void ToEmptyClause_AddingNotEmptyClause_ReturnsCorrectParametersList ()
		{
			// arrange
			var sut = new SqlClause ();


			var parameter = new SqlParameter { ParameterName = "@name" };

			var clause = new SqlClause ();
			clause.Add (parameter);


			// act
			sut.Add (clause);
			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter);
		}


		[Fact]
		public void ToNotEmptyClause_AddingNotEmptyClause_ReturnsCorrectParametersList ()
		{
			// arrange
			var sut = new SqlClause ();


			var parameter = new SqlParameter { ParameterName = "@name" };
			var parameter2 = new SqlParameter { ParameterName = "@name2" };

			sut.Add (parameter);

			var clause = new SqlClause ();
			clause.Add (parameter2);


			// act
			sut.Add (clause);
			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter, parameter2);
		}


		[Fact]
		public void HasTheParameterWithTheSameName_OverwritesIt ()
		{
			// arrange
			var sut = new SqlClause ();


			var parameter = new SqlParameter { ParameterName = "@name" };
			var parameter2 = new SqlParameter { ParameterName = "@name" };

			sut.Add (parameter);

			var clause = new SqlClause ();
			clause.Add (parameter2);


			// act
			sut.Add (clause);
			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter2);
		}
	}
}


