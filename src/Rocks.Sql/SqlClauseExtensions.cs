using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Rocks.Sql
{
    public static class SqlClauseExtensions
    {
        /// <summary>
        ///     Adds the specified <paramref name="sql"/> and <paramref name="parameters"/> to the list of sql expressions.
        ///		If <paramref name="sql"/> is null or empty - nothing will be added.
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause Add ([NotNull] this SqlClause source,
                                     [CanBeNull] string sql,
                                     [NotNull] IEnumerable<IDbDataParameter> parameters)
        {
            source.Add (sql);
            source.Add (parameters);

            return source;
        }


        /// <summary>
        ///     Adds the specified <paramref name="sql"/> and <paramref name="parameters"/> to the list of sql expressions.
        ///		If <paramref name="sql"/> is null or empty - nothing will be added.
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause Add ([NotNull] this SqlClause source,
                                     [CanBeNull] string sql,
                                     [NotNull] params IDbDataParameter[] parameters)
        {
            return source.Add (sql, (IEnumerable<IDbDataParameter>) parameters);
        }


        /// <summary>
        ///     Adds specified parameters to the parameters list.
        ///     If there is a parameter with the same name present (case sensitive), it will be overwritten.
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause Add ([NotNull] this SqlClause source, [NotNull] IEnumerable<IDbDataParameter> parameters)
        {
            foreach (var parameter in parameters)
                source.Add (parameter);

            return source;
        }


        /// <summary>
        ///     Adds specified parameters to the parameters list.
        ///     If there is a parameter with the same name present (case sensitive), it will be overwritten.
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause Add ([NotNull] this SqlClause source, [NotNull] params IDbDataParameter[] parameters)
        {
            return source.Add ((IEnumerable<IDbDataParameter>) parameters);
        }


        /// <summary>
        ///     Adds specified <paramref name="sqlClause" /> by appending
        ///     it sql representation as new expression and adding
        ///     all parameters (overwriting existing with the same name).
        /// </summary>
        /// <returns>Current instance (for chain calls).</returns>
        [NotNull]
        public static SqlClause Add ([NotNull] this SqlClause source, [NotNull] SqlClause sqlClause, bool trimmed = false)
        {
            if (sqlClause == null)
                throw new ArgumentNullException ("sqlClause");

            var sql = sqlClause.GetSql ();
            if (trimmed)
                sql = sql.Trim (' ', '\t', '\n', '\r');

            source.Add (sql);
            source.Add (sqlClause.GetParameters ());

            return source;
        }
    }
}