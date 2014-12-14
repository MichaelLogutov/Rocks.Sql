using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        public void AddIn_TwoParameters_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            var value1 = fixture.Create<string> ();
            var value2 = fixture.Create<string> ();


            // act
            sut.AddIn ("Id", "@x", value1, value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id in (@x1, @x2)");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x1",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value1
                                                    },
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x2",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value2
                                                    }
                                                });
        }


        [TestMethod]
        public void AddIn_NoParameters_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();


            // act
            sut.AddIn ("Id", "@x", new string[0]);
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
            sut.AddIn ("Id", "@x", (IEnumerable<string>) null);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<string> ();


            // act
            sut.AddIn ("Id", "@x", new[] { value }.Select (x => x));
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id in (@x1)");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x1",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotIn_TwoParameters_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            var value1 = fixture.Create<string> ();
            var value2 = fixture.Create<string> ();


            // act
            sut.AddNotIn ("Id", "@x", value1, value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id not in (@x1, @x2)");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x1",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value1
                                                    },
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x2",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value2
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotIn_NoParameters_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();


            // act
            sut.AddNotIn ("Id", "@x", new string[0]);
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
            sut.AddNotIn ("Id", "@x", (IEnumerable<string>) null);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }


        [TestMethod]
        public void AddNotIn_OneParameter_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<string> ();


            // act
            sut.AddNotIn ("Id", "@x", new[] { value }.Select (x => x));
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id not in (@x1)");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x1",
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value
                                                    }
                                                });
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


        [TestMethod]
        public void AddStartsWith_ByDefault_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();


            // act
            sut.AddStartsWith ("Id", "@x", "a%b");
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
                                                        Value = "a\\%b%"
                                                    }
                                                });
        }


        [TestMethod]
        public void AddStartsWith_EmptyString_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            var value = string.Empty;


            // act
            sut.AddStartsWith ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }


        [TestMethod]
        public void AddStartsWith_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            string value = null;


            // act
            sut.AddStartsWith ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }


        [TestMethod]
        public void AddNotStartsWith_ByDefault_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();


            // act
            sut.AddNotStartsWith ("Id", "@x", "a%b");
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
                                                        Value = "a\\%b%"
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotStartsWith_EmptyString_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            var value = string.Empty;


            // act
            sut.AddNotStartsWith ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }


        [TestMethod]
        public void AddNotStartsWith_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            string value = null;


            // act
            sut.AddNotStartsWith ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }


        [TestMethod]
        public void AddEndsWith_ByDefault_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();


            // act
            sut.AddEndsWith ("Id", "@x", "a%b");
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
                                                        Value = "%a\\%b"
                                                    }
                                                });
        }


        [TestMethod]
        public void AddEndsWith_EmptyString_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            var value = string.Empty;


            // act
            sut.AddEndsWith ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }


        [TestMethod]
        public void AddEndsWith_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            string value = null;


            // act
            sut.AddEndsWith ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }


        [TestMethod]
        public void AddNotEndsWith_ByDefault_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();


            // act
            sut.AddNotEndsWith ("Id", "@x", "a%b");
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
                                                        Value = "%a\\%b"
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotEndsWith_EmptyString_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            var value = string.Empty;


            // act
            sut.AddNotEndsWith ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }


        [TestMethod]
        public void AddNotEndsWith_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            string value = null;


            // act
            sut.AddNotEndsWith ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }
    }
}