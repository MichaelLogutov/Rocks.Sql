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
			                     "\tName" + Environment.NewLine);
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
			                     "\tinner join TableB" + Environment.NewLine);
		}


		[TestMethod]
		public void AsWhereClause_ByDefault_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsWhereClause ()
			                .AddExpression ("a = 1")
			                .AddExpression ("b = 1")
			                .GetSql ();


			// assert
			result.Should ().Be ("where" + Environment.NewLine +
			                     "\t(a = 1)" + Environment.NewLine +
			                     "\tand (b = 1)" + Environment.NewLine);
		}


		[TestMethod]
		public void AsWhereClause_WithOrLogic_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsWhereClause ("or")
			                .AddExpression ("a = 1")
			                .AddExpression ("b = 1")
			                .GetSql ();


			// assert
			result.Should ().Be ("where" + Environment.NewLine +
			                     "\t(a = 1)" + Environment.NewLine +
			                     "\tor (b = 1)" + Environment.NewLine);
		}


		[TestMethod]
		public void AsGroupByClause_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsGroupByClause ()
			                .AddExpression ("a")
			                .AddExpression ("b")
			                .GetSql ();


			// assert
			result.Should ().Be ("group by" + Environment.NewLine +
			                     "\ta," + Environment.NewLine +
			                     "\tb" + Environment.NewLine);
		}


		[TestMethod]
		public void AsHavingClause_ByDefault_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsHavingClause ()
			                .AddExpression ("a = 1")
			                .AddExpression ("b = 1")
			                .GetSql ();


			// assert
			result.Should ().Be ("having" + Environment.NewLine +
			                     "\t(a = 1)" + Environment.NewLine +
			                     "\tand (b = 1)" + Environment.NewLine);
		}


		[TestMethod]
		public void AsHavingClause_WithOrLogic_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsHavingClause ("or")
			                .AddExpression ("a = 1")
			                .AddExpression ("b = 1")
			                .GetSql ();


			// assert
			result.Should ().Be ("having" + Environment.NewLine +
			                     "\t(a = 1)" + Environment.NewLine +
			                     "\tor (b = 1)" + Environment.NewLine);
		}


		[TestMethod]
		public void AsCTEClause_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsCTEClause ("X")
			                .AddExpression ("select * from Table")
			                .GetSql ();


			// assert
			result.Should ().Be (";with X as (" + Environment.NewLine +
			                     "\tselect * from Table" + Environment.NewLine +
			                     ")" + Environment.NewLine);
		}


		[TestMethod]
		public void AsDeleteClause_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsDeleteClause ("Table")
			                .AddExpression ("a")
			                .AddExpression ("b")
			                .GetSql ();


			// assert
			result.Should ().Be ("delete from Table" + Environment.NewLine +
			                     "a" + Environment.NewLine +
			                     "b" + Environment.NewLine);
		}


		[TestMethod]
		public void AsDeleteClause_NoExpressions_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsDeleteClause ("Table")
			                .GetSql ();


			// assert
			result.Should ().Be ("delete from Table" + Environment.NewLine +
			                     Environment.NewLine);
		}


		[TestMethod]
		public void AsUpdateClause_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsUpdateClause ("Table")
			                .AddExpression ("A = 1")
			                .AddExpression ("B = 2")
			                .GetSql ();


			// assert
			result.Should ().Be ("update Table" + Environment.NewLine +
			                     "set" + Environment.NewLine +
			                     "\tA = 1," + Environment.NewLine +
			                     "\tB = 2" + Environment.NewLine);
		}


		[TestMethod]
		public void AsInsertClause_NoColumns_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsInsertClause ("Table")
			                .GetSql ();


			// assert
			result.Should ().Be ("insert into Table" + Environment.NewLine +
			                     Environment.NewLine);
		}


		[TestMethod]
		public void AsInsertColumnsClause_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsInsertColumnsClause ()
			                .AddExpression ("Id")
			                .AddExpression ("Name")
			                .GetSql ();


			// assert
			result.Should ().Be ("(" + Environment.NewLine +
			                     "\tId," + Environment.NewLine +
			                     "\tName" + Environment.NewLine +
			                     ")" + Environment.NewLine);
		}


		[TestMethod]
		public void AsValuesClause_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.AsValuesClause ()
			                .AddExpression ("Id")
			                .AddExpression ("Name")
			                .GetSql ();


			// assert
			result.Should ().Be ("values" + Environment.NewLine +
			                     "(" + Environment.NewLine +
			                     "\tId," + Environment.NewLine +
			                     "\tName" + Environment.NewLine +
			                     ")" + Environment.NewLine);
		}
	}
}