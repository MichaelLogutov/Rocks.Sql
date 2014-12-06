using System;
using System.Linq;
using JetBrains.Annotations;

namespace Rocks.Sql.IntermediateBuilders
{
	/// <summary>
	///     Represents an intermediate builder for chain constructing operation.
	/// </summary>
	public class SqlSelectColumnsIntermediateBuilder
	{
		#region Private fields

		private readonly string[] columns;

		#endregion

		#region Construct

		public SqlSelectColumnsIntermediateBuilder (string[] columns)
		{
			this.columns = columns;
		}

		#endregion

		#region Public methods

		/// <summary>
		///     Finished the construction of new <see cref="SqlSelectStatementBuilder" /> object instance.
		/// </summary>
		public SqlSelectStatementBuilder From ([NotNull] string tableName)
		{
			var result = new SqlSelectStatementBuilder (tableName);

			if (this.columns != null && this.columns.Length > 0)
			{
				foreach (var column in this.columns)
					result.Select.Add (column, column);
			}

			return result;
		}

		#endregion
	}
}