using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.BuildersTests
{
	[TestClass]
	public class SqlSelectStatementBuilderTests
	{
		[TestMethod]
		public void ByDefault_CreatesCorrectStatement ()
		{
			// arrange
			var sut = SqlBuilder.SelectFrom ("Table").Build ();


			// act
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().BeEquivalentToSql ("select * from Table");
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void WithTop_CreatesCorrectStatement ()
		{
			// arrange
			var parameter = new SqlParameter { ParameterName = "@top" };

			var sut = SqlBuilder.SelectFrom ("Table")
			                    .Columns ("Id", "Name")
			                    .Top (parameter)
			                    .Build ();


			// act
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().BeEquivalentToSql ("select top(@top) Id, Name from Table");
			parameters.Should ().Equal (parameter);
		}


        [TestMethod]
		public void _CreatesCorrectStatement ()
		{
			// arrange
			var sut = SqlBuilder.SelectFrom ("Table")
                .CTE (SqlBuilder.SelectFrom ("CTE"))
                .Build ();


			// act
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().BeEquivalentToSql (";with X as ( select * from CTE ) select * from Table");
			parameters.Should ().BeEmpty ();
		}
	}
}