using System.Data;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Rocks.Sql
{
	public static class SqlClausePredicatesExtensions
	{
		/// <summary>
		///     Adds "<paramref name="columnName" /> <paramref name="operand"/> <paramref name="parameter" />.ParameterName"
		///     expression to the clause.
		/// </summary>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static SqlClause AddPredicate (this SqlClause sqlClause,
		                                      [NotNull] string columnName,
		                                      [NotNull] string operand,
		                                      [NotNull] IDbDataParameter parameter)
		{
			sqlClause.Add (columnName + operand + parameter.ParameterName);
			sqlClause.Add (parameter);

			return sqlClause;
		}
	}
}