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


        //[Theory]
        //[InlineData (null, "null")]
        //[InlineData (true, "1")]
        //[InlineData (false, "0")]
        //[InlineData ((bool?) true, "1")]
        //[InlineData ((bool?) false, "0")]
        //[InlineData ((byte) 123, "123")]
        //[InlineData ((byte?) 123, "123")]
        //[InlineData ((sbyte) 123, "123")]
        //[InlineData ((sbyte?) 123, "123")]
        //[InlineData ((short) 123, "123")]
        //[InlineData ((short?) 123, "123")]
        //[InlineData ((ushort) 123, "123")]
        //[InlineData ((ushort?) 123, "123")]
        //[InlineData (123, "123")]
        //[InlineData ((int?) 123, "123")]
        //[InlineData ((uint) 123, "123")]
        //[InlineData ((uint?) 123, "123")]
        //[InlineData ((long) 123, "123")]
        //[InlineData ((long?) 123, "123")]
        //[InlineData ((ulong) 123, "123")]
        //[InlineData ((ulong?) 123, "123")]
        //[InlineData ((float) 123.45, "123.45")]
        //[InlineData ((float?) 123.45, "123.45")]
        //[InlineData (123.45, "123.45")]
        //[InlineData ((double?) 123.45, "123.45")]
        //[InlineData ("abc", "'abc'")]
        //public void ToString_WithParameter_ReplacesParameterWithItsValues (object value, string expected)
        //{
        //    // arrange
        //    var sut = new SqlClause ();
        //    sut.Add ("@p", new SqlParameter ("@p", value));


        //    // act
        //    var result = sut.ToString ();


        //    // arrange
        //    result.Should ().Be (expected);
        //}


        //public void ToString_WithDateParameter_ReplacesParameterWithItsValues (object value, string expected)
        //{
        //    // arrange
        //    var sut = new SqlClause ();
        //    var date = new DateTime (2000, 1, 2, 3, 4, 5);
        //    sut.Add ("@p @p2 @p3",
        //             new SqlParameter ("@p", date),
        //             new SqlParameter ("@p2", (DateTime?) date),
        //             new SqlParameter { ParameterName = "@p2", SqlDbType = SqlDbType.Date, Value = (DateTime?) date });


        //    // act
        //    var result = sut.ToString ();


        //    // arrange
        //    result.Should ().Be (expected);
        //}


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