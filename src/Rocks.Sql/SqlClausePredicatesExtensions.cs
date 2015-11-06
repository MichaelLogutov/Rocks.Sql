using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace Rocks.Sql
{
    public static class SqlClausePredicatesExtensions
    {
        /// <summary>
        ///     Adds "<paramref name="columnName" /> <paramref name="operand"/> <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddPredicate (this SqlClause sqlClause,
                                              [NotNull] string columnName,
                                              [NotNull] string operand,
                                              [CanBeNull] IDbDataParameter parameter)
        {
            if (parameter == null)
                return sqlClause;

            sqlClause.Add (columnName + operand + parameter.ParameterName);
            sqlClause.Add (parameter);

            return sqlClause;
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> = <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddEquals (this SqlClause sqlClause,
                                           [NotNull] string columnName,
                                           [CanBeNull] IDbDataParameter parameter)
        {
            return sqlClause.AddPredicate (columnName, " = ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> = <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotEquals (this SqlClause sqlClause,
                                              [NotNull] string columnName,
                                              [CanBeNull] IDbDataParameter parameter)
        {
            return sqlClause.AddPredicate (columnName, " <> ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &gt; <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        /// 	If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddGreater (this SqlClause sqlClause,
                                            [NotNull] string columnName,
                                            [CanBeNull] IDbDataParameter parameter)
        {
            return sqlClause.AddPredicate (columnName, " > ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &gt;= <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        /// 	If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddGreaterOrEquals (this SqlClause sqlClause,
                                                    [NotNull] string columnName,
                                                    [CanBeNull] IDbDataParameter parameter)
        {
            return sqlClause.AddPredicate (columnName, " >= ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &lt; <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        /// 	If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddLess (this SqlClause sqlClause,
                                         [NotNull] string columnName,
                                         [CanBeNull] IDbDataParameter parameter)
        {
            return sqlClause.AddPredicate (columnName, " < ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &lt;= <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        /// 	If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddLessOrEquals (this SqlClause sqlClause,
                                                 [NotNull] string columnName,
                                                 [CanBeNull] IDbDataParameter parameter)
        {
            return sqlClause.AddPredicate (columnName, " <= ", parameter);
        }


        /// <summary>
        ///     <para>
        ///         Adds "<paramref name="columnName" /> between <paramref name="parameter" />.ParameterName
        ///         and <paramref name="parameter2" />.ParameterName" expression to the clause.
        ///     </para>
        ///     <para>
        ///         If <paramref name="parameter2" /> is null then adds "<paramref name="columnName" /> &gt;=
        ///         <paramref name="parameter" />.ParameterName" expression to the clause.
        ///     </para>
        ///		<para>
        ///         If <paramref name="parameter" /> is null then adds "<paramref name="columnName" /> &lt;=
        ///         <paramref name="parameter2" />.ParameterName" expression to the clause.
        ///     </para>
        ///     <para>
        ///         If both <paramref name="parameter" /> and <paramref name="parameter2" /> are null
        ///         then nothing will be added.
        ///     </para>
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddBetween (this SqlClause sqlClause,
                                            [NotNull] string columnName,
                                            [CanBeNull] IDbDataParameter parameter,
                                            [CanBeNull] IDbDataParameter parameter2)
        {
            if (parameter == null && parameter2 == null)
                return sqlClause;

            if (parameter == null)
                return sqlClause.AddLessOrEquals (columnName, parameter2);

            if (parameter2 == null)
                return sqlClause.AddGreaterOrEquals (columnName, parameter);

            sqlClause.Add (columnName + " between " + parameter.ParameterName + " and " + parameter2.ParameterName);
            sqlClause.Add (parameter);
            sqlClause.Add (parameter2);

            return sqlClause;
        }


        /// <summary>
        ///     <para>
        ///         Adds "<paramref name="columnName" /> not between <paramref name="parameter" />.ParameterName
        ///         and <paramref name="parameter2" />.ParameterName" expression to the clause.
        ///     </para>
        ///     <para>
        ///         If <paramref name="parameter2" /> is null then adds "<paramref name="columnName" /> &lt;
        ///         <paramref name="parameter" />.ParameterName" expression to the clause.
        ///     </para>
        ///		<para>
        ///         If <paramref name="parameter" /> is null then adds "<paramref name="columnName" /> &gt;
        ///         <paramref name="parameter2" />.ParameterName" expression to the clause.
        ///     </para>
        ///     <para>
        ///         If both <paramref name="parameter" /> and <paramref name="parameter2" /> are null
        ///         then nothing will be added.
        ///     </para>
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotBetween (this SqlClause sqlClause,
                                               [NotNull] string columnName,
                                               [CanBeNull] IDbDataParameter parameter,
                                               [CanBeNull] IDbDataParameter parameter2)
        {
            if (parameter == null && parameter2 == null)
                return sqlClause;

            if (parameter == null)
                return sqlClause.AddGreater (columnName, parameter2);

            if (parameter2 == null)
                return sqlClause.AddLess (columnName, parameter);

            sqlClause.Add (columnName + " not between " + parameter.ParameterName + " and " + parameter2.ParameterName);
            sqlClause.Add (parameter);
            sqlClause.Add (parameter2);

            return sqlClause;
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> like <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddLike (this SqlClause sqlClause,
                                         [NotNull] string columnName,
                                         [CanBeNull] IDbDataParameter parameter)
        {
            return sqlClause.AddPredicate (columnName, " like ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> not like <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotLike (this SqlClause sqlClause,
                                            [NotNull] string columnName,
                                            [CanBeNull] IDbDataParameter parameter)
        {
            return sqlClause.AddPredicate (columnName, " not like ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> in (<paramref name="parameters" />.ParameterName, ...)"
        ///     expression to the clause.
        ///		If <paramref name="parameters"/> is null or contains no elements then nothing will be added.
        /// </summary>
        public static SqlClause AddIn (this SqlClause sqlClause,
                                       [NotNull] string columnName,
                                       [CanBeNull] IEnumerable<IDbDataParameter> parameters)
        {
            if (parameters == null)
                return sqlClause;

            var list = parameters as IReadOnlyList<IDbDataParameter>;
            if (list == null)
                list = parameters.ToList ();

            if (list.Count == 0)
                return sqlClause;

            if (list.Count == 1)
                return sqlClause.AddEquals (columnName, list[0]);


            var expression = new StringBuilder (columnName);
            expression.Append (" in (");

            for (var k = 0; k < list.Count; k++)
            {
                var p = list[k];

                if (k > 0)
                    expression.Append (", ");

                expression.Append (p.ParameterName);
                sqlClause.Add (p);
            }

            expression.Append (')');

            sqlClause.Add (expression.ToString ());

            return sqlClause;
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> in (<paramref name="parameters" />.ParameterName, ...)"
        ///     expression to the clause.
        ///		If <paramref name="parameters"/> is null or contains no elements then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddIn (this SqlClause sqlClause,
                                       [NotNull] string columnName,
                                       [CanBeNull] params IDbDataParameter[] parameters)
        {
            return sqlClause.AddIn (columnName, (IEnumerable<IDbDataParameter>) parameters);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> not in (<paramref name="parameters" />.ParameterName, ...)"
        ///     expression to the clause.
        /// </summary>
        public static SqlClause AddNotIn (this SqlClause sqlClause,
                                          [NotNull] string columnName,
                                          [CanBeNull] IEnumerable<IDbDataParameter> parameters)
        {
            if (parameters == null)
                return sqlClause;

            var list = parameters as IReadOnlyList<IDbDataParameter>;
            if (list == null)
                list = parameters.ToList ();

            if (list.Count == 0)
                return sqlClause;

            if (list.Count == 1)
                return sqlClause.AddNotEquals (columnName, list[0]);


            var expression = new StringBuilder (columnName);
            expression.Append (" not in (");

            for (var k = 0; k < list.Count; k++)
            {
                var p = list[k];

                if (k > 0)
                    expression.Append (", ");

                expression.Append (p.ParameterName);
                sqlClause.Add (p);
            }

            expression.Append (')');

            sqlClause.Add (expression.ToString ());

            return sqlClause;
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> not in (<paramref name="parameters" />.ParameterName, ...)"
        ///     expression to the clause.
        ///		If <paramref name="parameters"/> is null or contains no elements then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotIn (this SqlClause sqlClause,
                                          [NotNull] string columnName,
                                          [CanBeNull] params IDbDataParameter[] parameters)
        {
            return sqlClause.AddNotIn (columnName, (IEnumerable<IDbDataParameter>) parameters);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> is null" expression to the clause.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddIsNull (this SqlClause sqlClause, [NotNull] string columnName)
        {
            sqlClause.Add (columnName + " is null");

            return sqlClause;
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> is null" expression to the clause.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddIsNotNull (this SqlClause sqlClause, [NotNull] string columnName)
        {
            sqlClause.Add (columnName + " is not null");

            return sqlClause;
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &amp; <paramref name="parameter" />.ParameterName = <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddBitAnd (this SqlClause sqlClause,
                                           [NotNull] string columnName,
                                           [CanBeNull] IDbDataParameter parameter)
        {
            if (parameter == null)
                return sqlClause;

            return sqlClause.AddPredicate (columnName, " & " + parameter.ParameterName + " = ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &amp; <paramref name="parameter" />.ParameterName &lt;&gt; <paramref name="parameter" />.ParameterName"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotBitAnd (this SqlClause sqlClause,
                                              [NotNull] string columnName,
                                              [CanBeNull] IDbDataParameter parameter)
        {
            if (parameter == null)
                return sqlClause;

            return sqlClause.AddPredicate (columnName, " & " + parameter.ParameterName + " <> ", parameter);
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &amp; <paramref name="parameter" />.ParameterName > 0"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddBitOr (this SqlClause sqlClause,
                                          [NotNull] string columnName,
                                          [CanBeNull] IDbDataParameter parameter)
        {
            if (parameter == null)
                return sqlClause;

            sqlClause.Add (columnName + " & " + parameter.ParameterName + " > 0");
            sqlClause.Add (parameter);

            return sqlClause;
        }


        /// <summary>
        ///     Adds "<paramref name="columnName" /> &amp; <paramref name="parameter" />.ParameterName &lt;= 0"
        ///     expression to the clause.
        ///		If <paramref name="parameter"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotBitOr (this SqlClause sqlClause,
                                             [NotNull] string columnName,
                                             [CanBeNull] IDbDataParameter parameter)
        {
            if (parameter == null)
                return sqlClause;

            sqlClause.Add (columnName + " & " + parameter.ParameterName + " <= 0");
            sqlClause.Add (parameter);

            return sqlClause;
        }


        /// <summary>
        ///     Adds "exists (<paramref name="clause" />)" expression to the clause.
        ///		If <paramref name="clause"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddExists (this SqlClause sqlClause, [CanBeNull] SqlClause clause)
        {
            if (clause == null)
                return sqlClause;

            sqlClause.Add ("exists (");
            sqlClause.Add (clause);
            sqlClause.Add (")");

            return sqlClause;
        }


        /// <summary>
        ///     Adds "exists (<paramref name="clause" />)" expression to the clause.
        ///		If <paramref name="clause"/> is null then nothing will be added.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SqlClause AddNotExists (this SqlClause sqlClause, [CanBeNull] SqlClause clause)
        {
            if (clause == null)
                return sqlClause;

            sqlClause.Add ("not exists (");
            sqlClause.Add (clause);
            sqlClause.Add (")");

            return sqlClause;
        }
    }
}