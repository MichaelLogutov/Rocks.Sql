using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.BuildersTests
{
	[TestClass]
	public class FactoriesTests
	{
		[TestMethod]
		public void Select_InitializesAsCorrectlyFormattedSql ()
		{
			// arrange


			// act
			var result = SqlClauseBuilder.Select ()
			                       .Add ("Id")
			                       .Add ("Name")
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
			var result = SqlClauseBuilder.Select ("Id", "Name")
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
			var result = SqlClauseBuilder.From ("TableA")
			                       .Add ("inner join TableB")
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
			var result = SqlClauseBuilder.Where ()
			                       .Add ("a = 1")
			                       .Add ("b = 1")
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
			var result = SqlClauseBuilder.Where ("or")
			                       .Add ("a = 1")
			                       .Add ("b = 1")
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
			var result = SqlClauseBuilder.GroupBy ()
			                       .Add ("a")
			                       .Add ("b")
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
			var result = SqlClauseBuilder.GroupBy ("a", "b")
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
			var result = SqlClauseBuilder.Having ()
			                       .Add ("a = 1")
			                       .Add ("b = 1")
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
			var result = SqlClauseBuilder.Having ("or")
			                       .Add ("a = 1")
			                       .Add ("b = 1")
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
			var result = SqlClauseBuilder.OrderBy ()
			                       .Add ("a")
			                       .Add ("b")
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
			var result = SqlClauseBuilder.OrderBy ("a", "b")
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
			var result = SqlClauseBuilder.CTE ("X")
			                       .Add ("select * from Table")
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
			var result = SqlClauseBuilder.Delete ("Table")
			                       .Add ("a")
			                       .Add ("b")
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
			var result = SqlClauseBuilder.Delete ("Table")
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
			var result = SqlClauseBuilder.Update ("Table")
			                       .Add ("A = 1")
			                       .Add ("B = 2")
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
			var result = SqlClauseBuilder.Insert ("Table")
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
			var result = SqlClauseBuilder.Insert ("Table", "Id", "Name")
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
			var result = SqlClauseBuilder.InsertColumns ()
			                       .Add ("Id")
			                       .Add ("Name")
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
			var result = SqlClauseBuilder.Values ()
			                       .Add ("Id")
			                       .Add ("Name")
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