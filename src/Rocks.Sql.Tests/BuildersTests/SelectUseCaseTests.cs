using System;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.BuildersTests
{
	[TestClass]
	public class SelectUseCaseTests
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
			var sql = SqlBuilder.Select ("top(@top) o.Id", "o.Date")
			                    .From ("Orders as o");

			if (filter.MaxRecords != null)
			{
				sql.Select.Add (new SqlParameter
				                {
					                ParameterName = "@top",
					                SqlDbType = SqlDbType.Int,
					                Value = filter.MaxRecords
				                });
			}

			if (!string.IsNullOrEmpty (filter.UserName))
			{
				sql.From.Add ("u", "inner join Users as u on (o.UserId = u.Id)");
				sql.Where.Add ("u.Name = @userName",
				               new SqlParameter
				               {
					               ParameterName = "@userName",
					               SqlDbType = SqlDbType.VarChar,
					               Value = filter.UserName
				               });
			}

			if (!string.IsNullOrEmpty (filter.UserEmail))
			{
				sql.From.Add ("u", "inner join Users as u on (o.UserId = u.Id)");
				sql.Where.Add ("u.Email = @userEmail",
				               new SqlParameter
				               {
					               ParameterName = "@userEmail",
					               SqlDbType = SqlDbType.VarChar,
					               Value = filter.UserEmail
				               });
			}

			sql.OrderBy.Add ("o.Date");

			return sql.Build ();
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