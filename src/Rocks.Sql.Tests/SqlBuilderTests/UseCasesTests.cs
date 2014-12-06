using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.SqlBuilderTests
{
	[TestClass]
	public class UseCasesTests
	{
		#region Public methods

		[TestMethod]
		public void Select_ReturnsCorrectSqlAndParameters ()
		{
			// arrange
			var filter = new Filter
			             {
				             UserName = "aaa",
				             UserEmail = "user@email.com",
				             MaxRecords = 10
			             };


			// act
			var result = CreateSql (filter);


			// assert
			result.GetSql ().Should ().BeEquivalentToSql ("select top(@top) o.Id, o.Date " +
			                                              "from Orders as o " +
			                                              "inner join Users as u on (o.UserId = u.Id) " +
			                                              "where (u.Name = @userName) and (u.Email = @userEmail) " +
			                                              "order by o.Date");

			result.GetParameters ().ShouldAllBeEquivalentTo (new[]
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


		[TestMethod]
		public void Select_IsPerformant ()
		{
			// arrange
			var filter = new Filter
			             {
				             UserName = "aaa",
				             UserEmail = "user@email.com",
				             MaxRecords = 10
			             };


			// act
			Action action = () =>
			{
				var sql = CreateSql (filter);
				sql.GetSql ();
			};


			// assert
			action.ExecutionTime ().ShouldNotExceed (30.Milliseconds ());
		}

		#endregion

		#region Private methods

		private static SqlClause CreateSql (Filter filter)
		{
			var select = SqlBuilder.Select ("top(@top) o.Id", "o.Date");
			var from = SqlBuilder.From ("Orders as o");
			var where = SqlBuilder.Where ();
			var order = SqlBuilder.OrderBy ("o.Date");

			if (filter.MaxRecords != null)
			{
				@select.AddParameter (new SqlParameter
				                      {
					                      ParameterName = "@top",
					                      SqlDbType = SqlDbType.Int,
					                      Value = filter.MaxRecords
				                      });
			}

			if (!string.IsNullOrEmpty (filter.UserName))
			{
				@from.AddExpression ("u", "inner join Users as u on (o.UserId = u.Id)");
				@where.AddExpression ("u.Name = @userName",
				                      new SqlParameter
				                      {
					                      ParameterName = "@userName",
					                      SqlDbType = SqlDbType.VarChar,
					                      Value = filter.UserName
				                      });
			}

			if (!string.IsNullOrEmpty (filter.UserEmail))
			{
				@from.AddExpression ("u", "inner join Users as u on (o.UserId = u.Id)");
				@where.AddExpression ("u.Email = @userEmail",
				                      new SqlParameter
				                      {
					                      ParameterName = "@userEmail",
					                      SqlDbType = SqlDbType.VarChar,
					                      Value = filter.UserEmail
				                      });
			}

			var result = SqlBuilder.Statement (@select, @from, @where, order);

			return result;
		}

		#endregion

		#region Nested type: Filter

		public class Filter
		{
			#region Public properties

			public string UserName { get; set; }
			public string UserEmail { get; set; }
			public int? MaxRecords { get; set; }

			#endregion
		}

		#endregion
	}
}