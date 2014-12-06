using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.ExtensionsTests
{
	[TestClass]
	public class InitializersTests
	{
		[TestMethod]
		public void AsSelectClause_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsSelectClause ()
			                .AddExpression ("Id")
			                .AddExpression ("Name")
			                .GetSql ();


			// assert
			result.Should ().Be ("select" + Environment.NewLine +
			                     "\tId," + Environment.NewLine +
			                     "\tName" +
			                     Environment.NewLine);
		}


		[TestMethod]
		public void AsFromClause_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsFromClause ()
			                .AddExpression ("TableA")
			                .AddExpression ("inner join TableB")
			                .GetSql ();


			// assert
			result.Should ().Be ("from" + Environment.NewLine +
			                     "\tTableA" + Environment.NewLine +
			                     "\tinner join TableB" +
			                     Environment.NewLine);
		}
	}
}