using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	[TestClass]
	public class SqlClausePredicatesExtensionsTests
	{
		[TestMethod]
		public void AddPredicate_ByDefault_AddsSqlAndParameters ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter
			                {
				                ParameterName = "@x",
				                SqlDbType = SqlDbType.Int,
				                Value = 10
			                };


			// act
			sut.AddPredicate ("Id", " = ", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id = @x");
			parameters.Should ().Equal (parameter);
		}
	}
}