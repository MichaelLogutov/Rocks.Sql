using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Xunit;

namespace Rocks.Sql.Tests.BuildersTests
{
    public class SelectUseCaseTests
    {
        [Fact]
        public void Select_ReturnsCorrectSqlAndParameters()
        {
            // arrange
            var filter = CreateFilter();
            var sut = CreateSut(filter);


            // act
            var sql = sut.GetSql();
            var parameters = sut.GetParameters();


            // assert
            sql.Should().BeEquivalentToSql("select top(@top) o.Id, o.Date " +
                                           "from Orders as o " +
                                           "inner join Users as u on (o.UserId = u.Id) " +
                                           "where (u.Name = @userName) and (u.Email = @userEmail) " +
                                           "order by o.Date");

            parameters.Should().BeEquivalentTo(new[]
                                               {
                                                   new SqlParameter
                                                   {
                                                       ParameterName = "@top",
                                                       SqlDbType = SqlDbType.Int,
                                                       Value = filter.MaxRecords
                                                   },
                                                   new SqlParameter
                                                   {
                                                       ParameterName = "@userName",
                                                       SqlDbType = SqlDbType.VarChar,
                                                       Value = filter.UserName
                                                   },
                                                   new SqlParameter
                                                   {
                                                       ParameterName = "@userEmail",
                                                       SqlDbType = SqlDbType.VarChar,
                                                       Value = filter.UserEmail
                                                   }
                                               });
        }


        [Fact]
        public void Select_IsPerformant()
        {
            // arrange
            Action action = Benchmark;


            // act, assert
            action.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(30));
        }


        protected internal static void Benchmark()
        {
            CreateSut(CreateFilter()).GetSql();
        }


        private static Filter CreateFilter()
        {
            return new Filter
                   {
                       UserName = "aaa",
                       UserEmail = "user@email.com",
                       MaxRecords = 10
                   };
        }


        private static SqlClause CreateSut(Filter filter)
        {
            var sql = SqlBuilder.SelectFrom("Orders as o")
                                .Columns("o.Id", "o.Date");

            if (filter.MaxRecords != null)
            {
                sql.Top(new SqlParameter
                        {
                            ParameterName = "@top",
                            SqlDbType = SqlDbType.Int,
                            Value = filter.MaxRecords
                        });
            }

            if (!string.IsNullOrEmpty(filter.UserName))
            {
                sql.From.Add("u", "inner join Users as u on (o.UserId = u.Id)");
                sql.Where.AddEquals("u.Name",
                                    new SqlParameter
                                    {
                                        ParameterName = "@userName",
                                        SqlDbType = SqlDbType.VarChar,
                                        Value = filter.UserName
                                    });
            }

            if (!string.IsNullOrEmpty(filter.UserEmail))
            {
                sql.From.Add("u", "inner join Users as u on (o.UserId = u.Id)");
                sql.Where.AddEquals("u.Email",
                                    new SqlParameter
                                    {
                                        ParameterName = "@userEmail",
                                        SqlDbType = SqlDbType.VarChar,
                                        Value = filter.UserEmail
                                    });
            }

            sql.OrderBy.Add("o.Date");

            return sql.Build();
        }


        public class Filter
        {
            public string UserName { get; set; }
            public string UserEmail { get; set; }
            public int? MaxRecords { get; set; }
        }
    }
}