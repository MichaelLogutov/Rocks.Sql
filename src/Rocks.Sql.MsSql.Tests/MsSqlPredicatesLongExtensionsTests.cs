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
    public class MsSqlPredicatesLongExtensionsTests
    {
        [TestMethod]
        public void AddEquals_ByDefault_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();


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
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddEquals_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            long? value = null;


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
            var value = fixture.Create<long?> ();


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
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotEquals_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            long? value = null;


            // act
            sut.AddNotEquals ("Id", "@x", value);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();


            // act
            sut.AddGreater ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id > @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddGreater_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            long? value = null;


            // act
            sut.AddGreater ("Id", "@x", value);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();


            // act
            sut.AddGreaterOrEquals ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id >= @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddGreaterOrEquals_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            long? value = null;


            // act
            sut.AddGreaterOrEquals ("Id", "@x", value);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();


            // act
            sut.AddLess ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id < @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddLess_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            long? value = null;


            // act
            sut.AddLess ("Id", "@x", value);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();


            // act
            sut.AddLessOrEquals ("Id", "@x", value);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id <= @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddLessOrEquals_Null_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            long? value = null;


            // act
            sut.AddLessOrEquals ("Id", "@x", value);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();
            var value2 = fixture.Create<long?> ();


            // act
            sut.AddBetween ("Id", "@x", value, "@x2", value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id between @x and @x2");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    },
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x2",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value2
                                                    }
                                                });
        }


        [TestMethod]
        public void AddBetween_TheFirstParameterIsNull_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            long? value = null;
            var value2 = fixture.Create<long?> ();


            // act
            sut.AddBetween ("Id", "@x", value, "@x2", value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id <= @x2");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x2",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value2
                                                    }
                                                });
        }


        [TestMethod]
        public void AddBetween_TheSecondParameterIsNull_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();
            long? value2 = null;


            // act
            sut.AddBetween ("Id", "@x", value, "@x2", value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id >= @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddBetween_BothParametersAreNull_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            long? value = null;
            long? value2 = null;


            // act
            sut.AddBetween ("Id", "@x", value, "@x2", value2);
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
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();
            var value2 = fixture.Create<long?> ();


            // act
            sut.AddNotBetween ("Id", "@x", value, "@x2", value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id not between @x and @x2");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    },
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x2",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value2
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotBetween_TheFirstParameterIsNull_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            long? value = null;
            var value2 = fixture.Create<long?> ();


            // act
            sut.AddNotBetween ("Id", "@x", value, "@x2", value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id > @x2");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x2",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value2
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotBetween_TheSecondParameterIsNull_AddsCorrectPredicate ()
        {
            // arrange
            var sut = new SqlClause ();
            var fixture = new FixtureBuilder ().Build ();
            var value = fixture.Create<long?> ();
            long? value2 = null;


            // act
            sut.AddNotBetween ("Id", "@x", value, "@x2", value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be ("Id < @x");
            parameters.ShouldAllBeEquivalentTo (new[]
                                                {
                                                    new SqlParameter
                                                    {
                                                        ParameterName = "@x",
                                                        SqlDbType = SqlDbType.BigInt,
                                                        Value = value
                                                    }
                                                });
        }


        [TestMethod]
        public void AddNotBetween_BothParametersAreNull_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();
            long? value = null;
            long? value2 = null;


            // act
            sut.AddNotBetween ("Id", "@x", value, "@x2", value2);
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (string.Empty);
            parameters.Should ().BeEmpty ();
        }
    }
}