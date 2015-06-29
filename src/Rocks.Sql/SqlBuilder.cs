using JetBrains.Annotations;

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
        public static SqlSelectStatementBuilder SelectFrom (string tableName)
        {
            return new SqlSelectStatementBuilder (tableName);
        }
    }
}