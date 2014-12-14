using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace Rocks.Sql.MsSql.Tests
{
	[TestClass]
	public class MsSqlPredicatesExtensionsTests
	{
		[TestMethod]
		public void Int_AddEquals_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			int value = 1;
			

			// act
			sut.AddEquals ("Id", "@x", value);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id = @x");
			parameters.ShouldAllBeEquivalentTo (new[]
			                                    {
				                                    new SqlParameter
				                                    {
				                                    	ParameterName = "@x",
				                                    	SqlDbType = SqlDbType.Int,
				                                    	Value = value
				                                    }
			                                    });
		}


		[TestMethod]
		public void Int_AddEquals_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();
			int? value = null;


			// act
			sut.AddEquals ("Id", "@x", value);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}
	}
}