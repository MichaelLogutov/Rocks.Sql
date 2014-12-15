using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Sql.MsSql
{
	public static class SqlSelectStatementBuilderExtensions
	{
		/// <summary>
		///     Changes the select clause to start with "select top(<paramref name="topParameterName"/>)" text.
		///		Adds the <paramref name="top"/> value parameter to the clause parameters list.
		///		If <paramref name="top"/> is null then nothing will be added.
		/// </summary>
		public static SqlSelectStatementBuilder Top ([NotNull] this SqlSelectStatementBuilder builder, int? top, [NotNull] string topParameterName = "@top")
		{
			if (top == null)
				return builder;

			var parameter = new SqlParameter
			                {
				                ParameterName = topParameterName,
				                SqlDbType = SqlDbType.Int,
				                Value = top.Value
			                };

			builder.Select.Prefix = "select top(" + topParameterName + ")" + Environment.NewLine;
			builder.Select.Add (parameter);

			return builder;
		}


		/// <summary>
		///     Changes the select clause to start with "select top(<paramref name="topParameterName"/>)" text.
		///		Adds the <paramref name="top"/> value parameter to the clause parameters list.
		///		If <paramref name="top"/> is null then nothing will be added.
		/// </summary>
		public static SqlSelectStatementBuilder Top ([NotNull] this SqlSelectStatementBuilder builder, long? top, [NotNull] string topParameterName = "@top")
		{
			if (top == null)
				return builder;

			var parameter = new SqlParameter
			                {
				                ParameterName = topParameterName,
				                SqlDbType = SqlDbType.BigInt,
				                Value = top.Value
			                };

			builder.Select.Prefix = "select top(" + topParameterName + ")" + Environment.NewLine;
			builder.Select.Add (parameter);

			return builder;
		}
	}
}