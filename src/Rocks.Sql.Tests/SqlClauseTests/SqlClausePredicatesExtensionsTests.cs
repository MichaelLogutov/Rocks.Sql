﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddPredicate ("Id", " ? ", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id ? @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddPredicate_Null_AddsSqlAndParameters ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddPredicate ("Id", " ? ", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddEquals_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddEquals ("Id", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id = @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddEquals_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddEquals ("Id", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddNotEquals_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddNotEquals ("Id", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id <> @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddNotEquals_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddNotEquals ("Id", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddGreater_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddGreater ("Id", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id > @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddGreater_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddGreater ("Id", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddGreaterOrEquals_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddGreaterOrEquals ("Id", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id >= @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddGreaterOrEquals_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddGreaterOrEquals ("Id", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddLess_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddLess ("Id", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id < @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddLess_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddLess ("Id", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddLessOrEquals_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddLessOrEquals ("Id", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id <= @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddLessOrEquals_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddLessOrEquals ("Id", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddBetween_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };
			var parameter2 = new SqlParameter { ParameterName = "@x2" };


			// act
			sut.AddBetween ("Id", parameter, parameter2);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id between @x and @x2");
			parameters.Should ().Equal (parameter, parameter2);
		}


		[TestMethod]
		public void AddBetween_TheFirstParameterIsNull_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter2 = new SqlParameter { ParameterName = "@x2" };


			// act
			sut.AddBetween ("Id", null, parameter2);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id <= @x2");
			parameters.Should ().Equal (parameter2);
		}


		[TestMethod]
		public void AddBetween_TheSecondParameterIsNull_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddBetween ("Id", parameter, null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id >= @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddBetween_BothParametersAreNull_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddBetween ("Id", null, null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddNotBetween_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };
			var parameter2 = new SqlParameter { ParameterName = "@x2" };


			// act
			sut.AddNotBetween ("Id", parameter, parameter2);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id not between @x and @x2");
			parameters.Should ().Equal (parameter, parameter2);
		}


		[TestMethod]
		public void AddNotBetween_TheFirstParameterIsNull_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter2 = new SqlParameter { ParameterName = "@x2" };


			// act
			sut.AddNotBetween ("Id", null, parameter2);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id > @x2");
			parameters.Should ().Equal (parameter2);
		}


		[TestMethod]
		public void AddNotBetween_TheSecondParameterIsNull_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddNotBetween ("Id", parameter, null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id < @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddNotBetween_BothParametersAreNull_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddNotBetween ("Id", null, null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddLike_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddLike ("Id", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id like @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddLike_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddLike ("Id", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddNotLike_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddNotLike ("Id", parameter);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id not like @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddNotLike_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddNotLike ("Id", null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddIn_TwoParameters_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };
			var parameter2 = new SqlParameter { ParameterName = "@x2" };


			// act
			sut.AddIn ("Id", parameter, parameter2);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id in (@x, @x2)");
			parameters.Should ().Equal (parameter, parameter2);
		}


		[TestMethod]
		public void AddIn_NoParameters_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddIn ("Id");
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddIn_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddIn ("Id", (IEnumerable<IDbDataParameter>) null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddIn_OneParameter_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddIn ("Id", new[] { parameter }.Select (x => x));
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id = @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddNotIn_TwoParameters_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };
			var parameter2 = new SqlParameter { ParameterName = "@x2" };


			// act
			sut.AddNotIn ("Id", parameter, parameter2);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id not in (@x, @x2)");
			parameters.Should ().Equal (parameter, parameter2);
		}


		[TestMethod]
		public void AddNotIn_OneParameter_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();
			var parameter = new SqlParameter { ParameterName = "@x" };


			// act
			sut.AddNotIn ("Id", new[] { parameter }.Select (x => x));
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id <> @x");
			parameters.Should ().Equal (parameter);
		}


		[TestMethod]
		public void AddNotIn_NoParameters_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddNotIn ("Id");
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddNotIn_Null_AddsNothing ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddNotIn ("Id", (IEnumerable<IDbDataParameter>) null);
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be (string.Empty);
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddIsNull_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddIsNull ("Id");
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id is null");
			parameters.Should ().BeEmpty ();
		}


		[TestMethod]
		public void AddIsNotNull_ByDefault_AddsCorrectPredicate ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			sut.AddIsNotNull ("Id");
			var sql = sut.GetSql ();
			var parameters = sut.GetParameters ();


			// assert
			sql.Should ().Be ("Id is not null");
			parameters.Should ().BeEmpty ();
		}
	}
}