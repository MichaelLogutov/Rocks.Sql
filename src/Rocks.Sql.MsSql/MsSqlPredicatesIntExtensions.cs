using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Rocks.Sql.MsSql
{
    public static class MsSqlPredicatesIntExtensions
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
        ///     Adds "<paramref name="columnName" /> in (<paramref name="parameterName" />1, 
        ///     <paramref name="parameterName" />2, ...) expression to the clause.
        ///     If <paramref name="values"/> is null or contains no elements then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddIn (this SqlClause sqlClause,
                                       [NotNull] string columnName,
                                       [NotNull] string parameterName,
                                       [CanBeNull] IEnumerable<int> values)
        {
            if (values == null)
                return sqlClause;

            var parameters = values.Select ((value, index) => new SqlParameter
                                                              {
                                                                  ParameterName = parameterName + (index + 1),
                                                                  SqlDbType = SqlDbType.Int,
                                                                  Value = value
                                                              });

            return sqlClause.AddIn (columnName, parameters);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> in (<paramref name="parameterName" />1, 
        ///     <paramref name="parameterName" />2, ...) expression to the clause.
        ///     If <paramref name="values"/> is null or contains no elements then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddIn (this SqlClause sqlClause,
                                       [NotNull] string columnName,
                                       [NotNull] string parameterName,
                                       [CanBeNull] params int[] values)
        {
            return sqlClause.AddIn (columnName, parameterName, (IEnumerable<int>) values);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> not in (<paramref name="parameterName" />1, 
        ///     <paramref name="parameterName" />2, ...) expression to the clause.
        ///     If <paramref name="values"/> is null or contains no elements then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotIn (this SqlClause sqlClause,
                                          [NotNull] string columnName,
                                          [NotNull] string parameterName,
                                          [CanBeNull] IEnumerable<int> values)
        {
            if (values == null)
                return sqlClause;

            var parameters = values.Select ((value, index) => new SqlParameter
                                                              {
                                                                  ParameterName = parameterName + (index + 1),
                                                                  SqlDbType = SqlDbType.Int,
                                                                  Value = value
                                                              });

            return sqlClause.AddNotIn (columnName, parameters);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> not in (<paramref name="parameterName" />1, 
        ///     <paramref name="parameterName" />2, ...) expression to the clause.
        ///     If <paramref name="values"/> is null or contains no elements then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotIn (this SqlClause sqlClause,
                                          [NotNull] string columnName,
                                          [NotNull] string parameterName,
                                          [CanBeNull] params int[] values)
        {
            return sqlClause.AddNotIn (columnName, parameterName, (IEnumerable<int>) values);
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


        /// <summary>
        ///     <para>
        ///         Adds "<paramref name="columnName" /> between <paramref name="parameterName" />
        ///         and <paramref name="parameterName2" />" expression to the clause.
        ///     </para>
        ///     <para>
        ///         If <paramref name="value2" /> is null then adds "<paramref name="columnName" /> &gt;=
        ///         <paramref name="parameterName" />" expression to the clause.
        ///     </para>
        ///		<para>
        ///         If <paramref name="value" /> is null then adds "<paramref name="columnName" /> &lt;=
        ///         <paramref name="parameterName2" />" expression to the clause.
        ///     </para>
        ///     <para>
        ///         If both <paramref name="value" /> and <paramref name="value2" /> are null
        ///         then nothing will be added.
        ///     </para>
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddBetween (this SqlClause sqlClause,
                                            [NotNull] string columnName,
                                            [NotNull] string parameterName,
                                            int? value,
                                            [NotNull] string parameterName2,
                                            int? value2)
        {
            if (value == null && value2 == null)
                return sqlClause;

            var parameter = value == null ? null : new SqlParameter
                                                   {
                                                       ParameterName = parameterName,
                                                       SqlDbType = SqlDbType.Int,
                                                       Value = value
                                                   };

            var parameter2 = value2 == null ? null : new SqlParameter
                                                     {
                                                         ParameterName = parameterName2,
                                                         SqlDbType = SqlDbType.Int,
                                                         Value = value2
                                                     };

            return sqlClause.AddBetween (columnName, parameter, parameter2);
        }


        /// <summary>
        ///     <para>
        ///         Adds "<paramref name="columnName" /> not between <paramref name="parameterName" />
        ///         and <paramref name="parameterName2" />" expression to the clause.
        ///     </para>
        ///     <para>
        ///         If <paramref name="value2" /> is null then adds "<paramref name="columnName" /> &lt;
        ///         <paramref name="parameterName" />" expression to the clause.
        ///     </para>
        ///		<para>
        ///         If <paramref name="value" /> is null then adds "<paramref name="columnName" /> &gt;
        ///         <paramref name="parameterName2" />" expression to the clause.
        ///     </para>
        ///     <para>
        ///         If both <paramref name="value" /> and <paramref name="value2" /> are null
        ///         then nothing will be added.
        ///     </para>
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotBetween (this SqlClause sqlClause,
                                               [NotNull] string columnName,
                                               [NotNull] string parameterName,
                                               int? value,
                                               [NotNull] string parameterName2,
                                               int? value2)
        {
            if (value == null && value2 == null)
                return sqlClause;

            var parameter = value == null ? null : new SqlParameter
                                                   {
                                                       ParameterName = parameterName,
                                                       SqlDbType = SqlDbType.Int,
                                                       Value = value
                                                   };

            var parameter2 = value2 == null ? null : new SqlParameter
                                                     {
                                                         ParameterName = parameterName2,
                                                         SqlDbType = SqlDbType.Int,
                                                         Value = value2
                                                     };

            return sqlClause.AddNotBetween (columnName, parameter, parameter2);
        }
    }
}