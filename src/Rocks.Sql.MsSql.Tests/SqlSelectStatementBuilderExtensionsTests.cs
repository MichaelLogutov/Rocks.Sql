using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.Sql.Tests;

namespace Rocks.Sql.MsSql.Tests
{
	[TestClass]
	public class SqlSelectStatementBuilderExtensionsTests
	{
		[TestMethod]
		public void WithTop_Int_ByDefault_CreatesCorrectStatement ()
		{
			// arrange
			var sut = SqlBuilder.SelectFrom ("Table")
			                    .Columns ("Id", "Name")
			                    .Top (10, "@x")
			                    .Build ();


			// act
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().BeEquivalentToSql ("select top(@x) Id, Name from Table");
			parameters.ShouldAllBeEquivalentTo (new[]
			                                    {
				                                    new SqlParameter
				                                    {
					                                    ParameterName = "@x",
					                                    SqlDbType = SqlDbType.Int,
					                                    Value = 10
				                                    }
			                                    });
		}


		[TestMethod]
		public void WithTop_Int_Null_CreatesCorrectStatement ()
		{
			// arrange
			var sut = SqlBuilder.SelectFrom ("Table")
			                    .Columns ("Id", "Name")
			                    .Top (null)
			                    .Build ();


			// act
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().BeEquivalentToSql ("select Id, Name from Table");
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void WithTop_Long_ByDefault_CreatesCorrectStatement ()
		{
			// arrange
			var sut = SqlBuilder.SelectFrom ("Table")
			                    .Columns ("Id", "Name")
			                    .Top (10L, "@x")
			                    .Build ();


			// act
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().BeEquivalentToSql ("select top(@x) Id, Name from Table");
			parameters.ShouldAllBeEquivalentTo (new[]
			                                    {
				                                    new SqlParameter
				                                    {
					                                    ParameterName = "@x",
					                                    SqlDbType = SqlDbType.BigInt,
					                                    Value = 10
				                                    }
			                                    });
		}


		[TestMethod]
		public void WithTop_Long_Null_CreatesCorrectStatement ()
		{
			// arrange
			var sut = SqlBuilder.SelectFrom ("Table")
			                    .Columns ("Id", "Name")
			                    .Top ((long?) null)
			                    .Build ();


			// act
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().BeEquivalentToSql ("select Id, Name from Table");
			parameters.Should ().BeEmpty ();
		}
	}
}