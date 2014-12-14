using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace Rocks.Sql.MsSql.Tests
{
    [TestClass]
    public class MsSqlPredicatesStringExtensionsTests
    {
        [TestMethod]
        public void AddEquals_ByDefault_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<string> ();


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
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddEquals_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            string value = null;


            // act
            sut.AddEquals ("Id", "@x", value);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<string> ();


            // act
            sut.AddNotEquals ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id <> @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotEquals_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            string value = null;


            // act
            sut.AddNotEquals ("Id", "@x", value);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<string> ();


            // act
            sut.AddLike ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id like @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddLike_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            string value = null;


            // act
            sut.AddLike ("Id", "@x", value);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<string> ();


            // act
            sut.AddNotLike ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id not like @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotLike_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            string value = null;


            // act
            sut.AddNotLike ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }
    }
}