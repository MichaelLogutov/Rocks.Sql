using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Xunit;

namespace Rocks.Sql.Tests.SqlClauseTests
{
    public class ExpressionsTests
    {
        [Fact]
        public void GetSql_NoPrefix_NoSuffix_NoSeparator_NoExpressions_ByDefault_ReturnsEmptyString ()
        {
            // arrange
            var sut = new SqlClause ();


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().BeEmpty ();
        }


        [Fact]
        public void GetSql_WithPrefix_WithSuffix_WithSeparator_NoExpressions_ByDefault_ReturnsEmptyString ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Prefix = "prefix ";
            sut.ExpressionsSeparator = " and ";
            sut.Suffix = " suffix";


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().BeEmpty ();
        }


        [Fact]
        public void GetSql_WithPrefix_WithSuffix_WithSeparator_NoExpressions_WithRenderIfEmpty_ReturnsEmptyString ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Prefix = "prefix ";
            sut.ExpressionsSeparator = " and ";
            sut.Suffix = " suffix";
            sut.RenderIfEmpty = true;


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("prefix  suffix");
        }


        [Fact]
        public void GetSql_NoPrefix_NoSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Add ("a");
            sut.Add ("b");


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("ab");
        }


        [Fact]
        public void GetSql_WithPrefix_NoSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Prefix = "prefix ";
            sut.Add ("a");
            sut.Add ("b");


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("prefix ab");
        }


        [Fact]
        public void GetSql_NoPrefix_WithSuffix_NoSeparator_TwoExpressions_ReturnsCorrectSql ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Suffix = " suffix";
            sut.Add ("a");
            sut.Add ("b");


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("ab suffix");
        }


        [Fact]
        public void GetSql_NoPrefix_NoSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.ExpressionsSeparator = " and ";
            sut.Add ("a");
            sut.Add ("b");


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("a and b");
        }


        [Fact]
        public void GetSql_WithPrefix_WithSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Prefix = "prefix ";
            sut.ExpressionsSeparator = " and ";
            sut.Suffix = " suffix";
            sut.Add ("a");
            sut.Add ("b");


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("prefix a and b suffix");
        }


        [Fact]
        public void ToString_WithPrefix_WithSuffix_WithSeparator_TwoExpressions_ReturnsCorrectSql ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Prefix = "prefix ";
            sut.ExpressionsSeparator = " and ";
            sut.Suffix = " suffix";
            sut.Add ("a");
            sut.Add ("b");


            // act
            var result = sut.ToString ();


            // arrange
            result.Should ().Be ("prefix a and b suffix");
        }


        [Theory]
        [InlineData (null, typeof (object), "null")]
        [InlineData (true, typeof (bool), "1")]
        [InlineData (false, typeof (bool), "0")]
        [InlineData ((byte) 123, typeof (byte), "123")]
        [InlineData ((sbyte) 123, typeof (sbyte), "123")]
        [InlineData ((short) 123, typeof (short), "123")]
        [InlineData ((ushort) 123, typeof (ushort), "123")]
        [InlineData (123, typeof (int), "123")]
        [InlineData ((uint) 123, typeof (uint), "123")]
        [InlineData ((long) 123, typeof (long), "123")]
        [InlineData ((ulong) 123, typeof (ulong), "123")]
        [InlineData ((float) 123.45, typeof (float), "123.45")]
        [InlineData (123.45, typeof (double), "123.45")]
        [InlineData (123.45, typeof (decimal), "123.45")]
        [InlineData ("a'bc", typeof (string), "'a''bc'")]
        public void ToString_WithParameter_ReplacesParameterWithItsValue (object value, Type type, string expected)
        {
            // arrange
            if (value != null)
                value = Convert.ChangeType (value, type);

            var sut = new SqlClause ();
            sut.Add ("@p", new SqlParameter ("@p", value));


            // act
            var result = sut.ToString ();


            // arrange
            result.Should ().Be (expected);
        }


        [Fact]
        public void ToString_WithDateParameter_ReplacesParameterWithItsValue ()
        {
            // arrange
            var sut = new SqlClause ();
            var date = new DateTime (2000, 1, 2, 3, 4, 5);
            sut.Add ("@p1 @p2 @p3",
                     new SqlParameter { ParameterName = "@p1", SqlDbType = SqlDbType.DateTime, Value = date },
                     new SqlParameter { ParameterName = "@p2", SqlDbType = SqlDbType.Date, Value = (DateTime?) date },
                     new SqlParameter { ParameterName = "@p3", SqlDbType = SqlDbType.Time, Value = (DateTime?) date });


            // act
            var result = sut.ToString ();


            // arrange
            result.Should ().Be ("'2000-01-02 03:04:05' " +
                                 "'2000-01-02' " +
                                 "'03:04:05'");
        }


        [Fact]
        public void ToString_WithObjectParameter_ReplacesParameterWithItsToStringRepresentation ()
        {
            // arrange
            var sut = new SqlClause ();
            sut.Add ("@p", new SqlParameter ("@p", new { x = 1 }));


            // act
            var result = sut.ToString ();


            // arrange
            result.Should ().Be ("'{ x = 1 }'");
        }


        [Fact]
        public void AddKeyed_NoPreviousExpressions_ReturnsCorrectSql ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Add ("key", "a");


            // act
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("a");
        }


        [Fact]
        public void AddKeyed_WithPreviousExpressionWithTheSameKey_ByDefault_AddsNothing ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Add ("key", "a");


            // act
            sut.Add ("key", "b");
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("a");
        }


        [Fact]
        public void AddKeyed_WithPreviousExpressionWithTheSameKey_WithOverwrite_OverwritesTheExpression ()
        {
            // arrange
            var sut = new SqlClause ();

            sut.Add ("key", "a");


            // act
            sut.Add ("key", "b", overwrite: true);
            var result = sut.GetSql ();


            // arrange
            result.Should ().Be ("b");
        }
    }
}