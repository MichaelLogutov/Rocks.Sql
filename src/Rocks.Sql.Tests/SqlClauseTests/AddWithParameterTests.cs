﻿using System;
using System.Data.SqlClient;
using FluentAssertions;
using Xunit;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	public class AddWithParameterTests
	{
		[Fact]
		public void GetSql_ToEmptyClause_ReturnsCorrectSql ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter { ParameterName = "@test" };


			// act
			sut.Add ("a", parameter);

			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a");
		}


		[Fact]
		public void GetParameters_ToEmptyClause_ReturnsCorrectParametersList ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter { ParameterName = "@test" };


			// act
			sut.Add ("a", parameter);

			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter);
		}


		[Fact]
		public void GetSql_WithKey_HasExpressionWithTheSameKey_ByDefault_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("key", "a");

			var parameter = new SqlParameter { ParameterName = "@test" };


			// act
			sut.Add ("key", "b", parameter);

			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("a");
		}


		[Fact]
		public void GetParameters_WithKey_HasExpressionWithTheSameKey_ByDefault_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("key", "a");

			var parameter = new SqlParameter { ParameterName = "@test" };


			// act
			sut.Add ("key", "b", parameter);

			var result = sut.GetParameters ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[Fact]
		public void GetSql_WithKey_HasExpressionWithTheSameKey_WithOverwrite_OverwritesTheExpression ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("key", "a");

			var parameter = new SqlParameter { ParameterName = "@test" };


			// act
			sut.Add ("key", "b", parameter, overwrite: true);

			var result = sut.GetSql ();


			// arrange
			result.Should ().Be ("b");
		}


		[Fact]
		public void GetParameters_WithKey_HasParameterWithTheSameKey_WithOverwrite_OverwritesTheParameter ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("key", "a");

			var parameter = new SqlParameter { ParameterName = "@test" };


			// act
			sut.Add ("key", "b", parameter, overwrite: true);

			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter);
		}


		[Fact]
		public void GetParameters_WithKey_NoParameterWithTheSameKey_WithOverwrite_AddsTheParameter ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter { ParameterName = "@test" };
			sut.Add ("key", "a");
			sut.Add (parameter);

			var parameter2 = new SqlParameter { ParameterName = "@test2" };


			// act
			sut.Add ("key", "b", parameter2, overwrite: true);

			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter, parameter2);
		}
	}
}


