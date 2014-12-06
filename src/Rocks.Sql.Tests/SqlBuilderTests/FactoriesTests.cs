using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.SqlBuilderTests
{
	[TestClass]
	public class FactoriesTests
	{
		[TestMethod]
		public void Select_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Select ()
			                       .AddExpression ("Id")
			                       .AddExpression ("Name")
			                       .GetSql ();


			// assert
			result.Should ().Be ("select" + Environment.NewLine +
			                     "\tId," + Environment.NewLine +
			                     "\tName" + Environment.NewLine);
		}


		[TestMethod]
		public void Select_WithColumns_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Select ("Id", "Name")
			                       .GetSql ();


			// assert
			result.Should ().Be ("select" + Environment.NewLine +
			                     "\tId," + Environment.NewLine +
			                     "\tName" + Environment.NewLine);
		}


		[TestMethod]
		public void From_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.From ("TableA")
			                       .AddExpression ("inner join TableB")
			                       .GetSql ();


			// assert
			result.Should ().Be ("from" + Environment.NewLine +
			                     "\tTableA" + Environment.NewLine +
			                     "\tinner join TableB" + Environment.NewLine);
		}


		[TestMethod]
		public void Where_ByDefault_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Where ()
			                       .AddExpression ("a = 1")
			                       .AddExpression ("b = 1")
			                       .GetSql ();


			// assert
			result.Should ().Be ("where" + Environment.NewLine +
			                     "\t(a = 1)" + Environment.NewLine +
			                     "\tand (b = 1)" + Environment.NewLine);
		}


		[TestMethod]
		public void Where_WithOrLogic_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Where ("or")
			                       .AddExpression ("a = 1")
			                       .AddExpression ("b = 1")
			                       .GetSql ();


			// assert
			result.Should ().Be ("where" + Environment.NewLine +
			                     "\t(a = 1)" + Environment.NewLine +
			                     "\tor (b = 1)" + Environment.NewLine);
		}


		[TestMethod]
		public void GroupBy_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.GroupBy ()
			                       .AddExpression ("a")
			                       .AddExpression ("b")
			                       .GetSql ();


			// assert
			result.Should ().Be ("group by" + Environment.NewLine +
			                     "\ta," + Environment.NewLine +
			                     "\tb" + Environment.NewLine);
		}


		[TestMethod]
		public void GroupBy_WithColumns_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.GroupBy ("a", "b")
			                       .GetSql ();


			// assert
			result.Should ().Be ("group by" + Environment.NewLine +
			                     "\ta," + Environment.NewLine +
			                     "\tb" + Environment.NewLine);
		}


		[TestMethod]
		public void Having_ByDefault_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Having ()
			                       .AddExpression ("a = 1")
			                       .AddExpression ("b = 1")
			                       .GetSql ();


			// assert
			result.Should ().Be ("having" + Environment.NewLine +
			                     "\t(a = 1)" + Environment.NewLine +
			                     "\tand (b = 1)" + Environment.NewLine);
		}


		[TestMethod]
		public void Having_WithOrLogic_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Having ("or")
			                       .AddExpression ("a = 1")
			                       .AddExpression ("b = 1")
			                       .GetSql ();


			// assert
			result.Should ().Be ("having" + Environment.NewLine +
			                     "\t(a = 1)" + Environment.NewLine +
			                     "\tor (b = 1)" + Environment.NewLine);
		}


		[TestMethod]
		public void OrderBy_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.OrderBy ()
			                       .AddExpression ("a")
			                       .AddExpression ("b")
			                       .GetSql ();


			// assert
			result.Should ().Be ("order by" + Environment.NewLine +
			                     "\ta," + Environment.NewLine +
			                     "\tb" + Environment.NewLine);
		}


		[TestMethod]
		public void OrderBy_WithColumns_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.OrderBy ("a", "b")
			                       .GetSql ();


			// assert
			result.Should ().Be ("order by" + Environment.NewLine +
			                     "\ta," + Environment.NewLine +
			                     "\tb" + Environment.NewLine);
		}


		[TestMethod]
		public void CTE_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.CTE ("X")
			                       .AddExpression ("select * from Table")
			                       .GetSql ();


			// assert
			result.Should ().Be (";with X as (" + Environment.NewLine +
			                     "\tselect * from Table" + Environment.NewLine +
			                     ")" + Environment.NewLine);
		}


		[TestMethod]
		public void Delete_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Delete ("Table")
			                       .AddExpression ("a")
			                       .AddExpression ("b")
			                       .GetSql ();


			// assert
			result.Should ().Be ("delete from Table" + Environment.NewLine +
			                     "a" + Environment.NewLine +
			                     "b" + Environment.NewLine);
		}


		[TestMethod]
		public void Delete_NoExpressions_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Delete ("Table")
			                       .GetSql ();


			// assert
			result.Should ().Be ("delete from Table" + Environment.NewLine +
			                     Environment.NewLine);
		}


		[TestMethod]
		public void Update_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Update ("Table")
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
		public void Insert_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Insert ("Table")
			                       .GetSql ();


			// assert
			result.Should ().Be ("insert into Table" + Environment.NewLine +
			                     Environment.NewLine);
		}


		[TestMethod]
		public void Insert_WithColumns_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Insert ("Table", "Id", "Name")
			                       .GetSql ();


			// assert
			result.Should ().Be ("insert into Table" + Environment.NewLine +
			                     "(" + Environment.NewLine +
			                     "\tId," + Environment.NewLine +
			                     "\tName" + Environment.NewLine +
			                     ")" + Environment.NewLine +
			                     Environment.NewLine);
		}


		[TestMethod]
		public void InsertColumns_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.InsertColumns ()
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
		public void Values_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlBuilder.Values ()
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