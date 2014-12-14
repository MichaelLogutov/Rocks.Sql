using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Rocks.Sql.MsSql
{
    public static class MsSqlPredicatesStringExtensions
    {
        /// <summary>
        ///     Adds "<paramref name="columnName" /> = <paramref name="parameterName" />
        ///     expression to the clause.
        ///     If <paramref name="value"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddEquals (this SqlClause sqlClause,
                                           [NotNull] string columnName,
                                           [NotNull] string parameterName,
                                           string value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddEquals (columnName, new SqlParameter
                                                    {
                                                        ParameterName = parameterName,
                                                        SqlDbType = SqlDbType.VarChar,
                                                        Value = value
                                                    });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &lt;&gt; <paramref name="parameterName" />
        ///     expression to the clause.
        ///     If <paramref name="value"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotEquals (this SqlClause sqlClause,
                                              [NotNull] string columnName,
                                              [NotNull] string parameterName,
                                              string value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddNotEquals (columnName, new SqlParameter
                                                       {
                                                           ParameterName = parameterName,
                                                           SqlDbType = SqlDbType.VarChar,
                                                           Value = value
                                                       });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> like <paramref name="parameterName" />
        ///     expression to the clause.
        ///     If <paramref name="value"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddLike (this SqlClause sqlClause,
                                         [NotNull] string columnName,
                                         [NotNull] string parameterName,
                                         string value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddLike (columnName, new SqlParameter
                                                  {
                                                      ParameterName = parameterName,
                                                      SqlDbType = SqlDbType.VarChar,
                                                      Value = value
                                                  });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> not like <paramref name="parameterName" />
        ///     expression to the clause.
        ///     If <paramref name="value"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotLike (this SqlClause sqlClause,
                                            [NotNull] string columnName,
                                            [NotNull] string parameterName,
                                            string value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddNotLike (columnName, new SqlParameter
                                                     {
                                                         ParameterName = parameterName,
                                                         SqlDbType = SqlDbType.VarChar,
                                                         Value = value
                                                     });
        }
    }
}