using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                                           [CanBeNull] string value)
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
                                              [CanBeNull] string value)
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
        ///     Adds "<paramref name="columnName" /> in (<paramref name="parameterName" />1, 
        ///     <paramref name="parameterName" />2, ...) expression to the clause.
        ///     If <paramref name="values"/> is null or contains no elements then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddIn (this SqlClause sqlClause,
                                       [NotNull] string columnName,
                                       [NotNull] string parameterName,
                                       [CanBeNull] IEnumerable<string> values)
        {
            if (values == null)
                return sqlClause;

            var parameters = values.Select ((value, index) => new SqlParameter
                                                              {
                                                                  ParameterName = parameterName + (index + 1),
                                                                  SqlDbType = SqlDbType.VarChar,
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
                                       [CanBeNull] params string[] values)
        {
            return sqlClause.AddIn (columnName, parameterName, (IEnumerable<string>) values);
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
                                          [CanBeNull] IEnumerable<string> values)
        {
            if (values == null)
                return sqlClause;

            var parameters = values.Select ((value, index) => new SqlParameter
                                                              {
                                                                  ParameterName = parameterName + (index + 1),
                                                                  SqlDbType = SqlDbType.VarChar,
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
                                          [CanBeNull] params string[] values)
        {
            return sqlClause.AddNotIn (columnName, parameterName, (IEnumerable<string>) values);
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
                                         [CanBeNull] string value)
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
                                            [CanBeNull] string value)
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


        /// <summary>
        ///     Adds "<paramref name="columnName" /> like <paramref name="parameterName" />
        ///     expression to the clause. The <paramref name="value"/> parameter is escaped
        ///     and appended with the '%' sign.
        ///     If <paramref name="value"/> is null or empty string then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddStartsWith (this SqlClause sqlClause,
                                               [NotNull] string columnName,
                                               [NotNull] string parameterName,
                                               [CanBeNull] string value)
        {
            if (string.IsNullOrEmpty (value))
                return sqlClause;

            return sqlClause.AddLike (columnName, new SqlParameter
                                                  {
                                                      ParameterName = parameterName,
                                                      SqlDbType = SqlDbType.VarChar,
                                                      Value = value.Replace ("%", "\\%") + "%"
                                                  });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> not like <paramref name="parameterName" />
        ///     expression to the clause. The <paramref name="value"/> parameter is escaped
        ///     and appended with the '%' sign.
        ///     If <paramref name="value"/> is null or empty string then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotStartsWith (this SqlClause sqlClause,
                                                  [NotNull] string columnName,
                                                  [NotNull] string parameterName,
                                                  [CanBeNull] string value)
        {
            if (string.IsNullOrEmpty (value))
                return sqlClause;

            return sqlClause.AddNotLike (columnName, new SqlParameter
                                                     {
                                                         ParameterName = parameterName,
                                                         SqlDbType = SqlDbType.VarChar,
                                                         Value = value.Replace ("%", "\\%") + "%"
                                                     });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> like <paramref name="parameterName" />
        ///     expression to the clause. The <paramref name="value"/> parameter is escaped
        ///     and prepended with the '%' sign.
        ///     If <paramref name="value"/> is null or empty string then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddEndsWith (this SqlClause sqlClause,
                                             [NotNull] string columnName,
                                             [NotNull] string parameterName,
                                             [CanBeNull] string value)
        {
            if (string.IsNullOrEmpty (value))
                return sqlClause;

            return sqlClause.AddLike (columnName, new SqlParameter
                                                  {
                                                      ParameterName = parameterName,
                                                      SqlDbType = SqlDbType.VarChar,
                                                      Value = "%" + value.Replace ("%", "\\%")
                                                  });
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> not like <paramref name="parameterName" />
        ///     expression to the clause. The <paramref name="value"/> parameter is escaped
        ///     and prepended with the '%' sign.
        ///     If <paramref name="value"/> is null or empty string then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotEndsWith (this SqlClause sqlClause,
                                                [NotNull] string columnName,
                                                [NotNull] string parameterName,
                                                [CanBeNull] string value)
        {
            if (string.IsNullOrEmpty (value))
                return sqlClause;

            return sqlClause.AddNotLike (columnName, new SqlParameter
                                                     {
                                                         ParameterName = parameterName,
                                                         SqlDbType = SqlDbType.VarChar,
                                                         Value = "%" + value.Replace ("%", "\\%")
                                                     });
        }
    }
}