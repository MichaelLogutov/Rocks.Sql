using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Rocks.Sql.MsSql
{
	public static class MsSqlPredicatesExtensions
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
                                           int? value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddEquals (columnName, new SqlParameter
                                                    {
                                                        ParameterName = parameterName,
                                                        SqlDbType = SqlDbType.Int,
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
                                              int? value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddNotEquals (columnName, new SqlParameter
                                                       {
                                                           ParameterName = parameterName,
                                                           SqlDbType = SqlDbType.Int,
                                                           Value = value
                                                       });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &gt; <paramref name="parameterName" />
        ///     expression to the clause.
        ///     If <paramref name="value"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddGreater (this SqlClause sqlClause,
                                            [NotNull] string columnName,
                                            [NotNull] string parameterName,
                                            int? value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddGreater (columnName, new SqlParameter
                                                     {
                                                         ParameterName = parameterName,
                                                         SqlDbType = SqlDbType.Int,
                                                         Value = value
                                                     });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &gt;= <paramref name="parameterName" />
        ///     expression to the clause.
        ///     If <paramref name="value"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddGreaterOrEquals (this SqlClause sqlClause,
                                                    [NotNull] string columnName,
                                                    [NotNull] string parameterName,
                                                    int? value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddGreaterOrEquals (columnName, new SqlParameter
                                                             {
                                                                 ParameterName = parameterName,
                                                                 SqlDbType = SqlDbType.Int,
                                                                 Value = value
                                                             });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &gt; <paramref name="parameterName" />
        ///     expression to the clause.
        ///     If <paramref name="value"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddLess (this SqlClause sqlClause,
                                         [NotNull] string columnName,
                                         [NotNull] string parameterName,
                                         int? value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddLess (columnName, new SqlParameter
                                                  {
                                                      ParameterName = parameterName,
                                                      SqlDbType = SqlDbType.Int,
                                                      Value = value
                                                  });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &gt;= <paramref name="parameterName" />
        ///     expression to the clause.
        ///     If <paramref name="value"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddLessOrEquals (this SqlClause sqlClause,
                                                 [NotNull] string columnName,
                                                 [NotNull] string parameterName,
                                                 int? value)
        {
            if (value == null)
                return sqlClause;

            return sqlClause.AddLessOrEquals (columnName, new SqlParameter
                                                          {
                                                              ParameterName = parameterName,
                                                              SqlDbType = SqlDbType.Int,
                                                              Value = value
                                                          });
        }
	}
}