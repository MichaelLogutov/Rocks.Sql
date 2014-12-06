using JetBrains.Annotations;
using Rocks.Sql.IntermediateBuilders;

namespace Rocks.Sql
{
	/// <summary>
	///     A helper static class for creation of common sql statements.
	/// </summary>
	public static class SqlBuilder
	{
		/// <summary>
		///     Creates new builder for sql "select" statement.
		/// </summary>
		[NotNull]
		public static SqlSelectColumnsIntermediateBuilder Select (params string[] columns)
		{
			return new SqlSelectColumnsIntermediateBuilder (columns);
		}
	}
}