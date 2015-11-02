using System;
using System.Data.SqlClient;
using FluentAssertions;
using Xunit;

namespace Rocks.Sql.Tests.BuildersTests
{
    public class SqlSelectStatementBuilderTests
    {
        [Fact]
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


        [Fact]
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


        [Fact]
        public void WithCTE_CreatesCorrectlyFormattedStatement ()
        {
            // arrange
            var sut = SqlBuilder.SelectFrom ("Table")
                                .CTE (SqlBuilder.SelectFrom ("CTE"))
                                .Build ();


            // act
            var sql = sut.GetSql ();
            var parameters = sut.GetParameters ();


            // assert
            sql.Should ().Be (";with X as (" + Environment.NewLine +
                              "\tselect" + Environment.NewLine +
                              "\t*" + Environment.NewLine +
                              "from" + Environment.NewLine +
                              "\tCTE" + Environment.NewLine +
                              ")" + Environment.NewLine +
                              Environment.NewLine +
                              "select" + Environment.NewLine +
                              "\t*" + Environment.NewLine +
                              "from" + Environment.NewLine +
                              "\tTable" + Environment.NewLine);
            parameters.Should ().BeEmpty ();
        }


        [Fact]
        public void WithPrefix_CreatesCorrectlyFormattedStatement ()
        {
            // arrange
            var source = SqlBuilder.SelectFrom ("Table").Column ("Id");
            source.Prefix.Add ("set deadlock_priority high");

            var sut = source.Build ();


            // act
            var sql = sut.GetSql ();


            // assert
            sql.Should ().Be ("set deadlock_priority high" + Environment.NewLine +
                              "select" + Environment.NewLine +
                              "\tId" + Environment.NewLine +
                              "from" + Environment.NewLine +
                              "\tTable" + Environment.NewLine);
        }


        [Fact]
        public void WithSuffix_CreatesCorrectlyFormattedStatement ()
        {
            // arrange
            var source = SqlBuilder.SelectFrom ("Table").Column ("Id");
            source.Suffix.Add ("offset 0 rows");
            source.Suffix.Add ("fetch next 20 rows only");

            var sut = source.Build ();


            // act
            var sql = sut.GetSql ();


            // assert
            sql.Should ().Be ("select" + Environment.NewLine +
                              "\tId" + Environment.NewLine +
                              "from" + Environment.NewLine +
                              "\tTable" + Environment.NewLine +
                              Environment.NewLine +
                              "offset 0 rows" + Environment.NewLine +
                              "fetch next 20 rows only");
        }
    }
}