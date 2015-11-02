using System;
using JetBrains.Annotations;

namespace Rocks.Sql
{
    public static class InitializersExtensions
    {
        /// <summary>
        ///     Initialize <see cref="SqlClause" /> to represent the sql clause of column names list.
        ///		The format is:
        ///		<code>
        ///			<paramref name="statement"/>
        ///				expression1,
        ///				expression2,
        ///				...
        ///		</code>
        /// </summary>
        [NotNull]
        public static SqlClause AsColumnsListClause ([NotNull] this SqlClause sqlClause, string statement, string indent = "\t")
        {
            sqlClause.Prefix = statement + Environment.NewLine;
            sqlClause.ExpressionsPrefix = indent;
            sqlClause.ExpressionsSeparator = "," + Environment.NewLine + indent;
            sqlClause.ExpressionsSuffix = Environment.NewLine;
            sqlClause.Suffix = null;

            return sqlClause;
        }


        /// <summary>
        ///     Initialize <see cref="SqlClause" /> to represent the sql clause of predicates list.
        ///		The format is:
        ///		<code>
        ///			<paramref name="statement"/>
        ///				(expression1)
        ///				<paramref name="logic"/> (expression2)
        ///				<paramref name="logic"/> (expression3)
        ///				...
        ///		</code>
        /// </summary>
        [NotNull]
        public static SqlClause AsPredicatesClause ([NotNull] this SqlClause sqlClause, string statement, string logic = "and", string indent = "\t")
        {
            sqlClause.Prefix = statement + Environment.NewLine;
            sqlClause.ExpressionsPrefix = indent + "(";
            sqlClause.ExpressionsSeparator = ")" + Environment.NewLine + indent + logic + " (";
            sqlClause.ExpressionsSuffix = ")" + Environment.NewLine;
            sqlClause.Suffix = null;

            return sqlClause;
        }


        /// <summary>
        ///     Initialize <see cref="SqlClause" /> to represent the sql clause of predicates inline list.
        ///		The format is:
        ///		<code>
        ///				(expression1) <paramref name="logic"/> (expression2) <paramref name="logic"/> (expression3) ...
        ///		</code>
        /// </summary>
        [NotNull]
        public static SqlClause AsInlinePredicatesClause ([NotNull] this SqlClause sqlClause, string logic = "and")
        {
            sqlClause.Prefix = null;
            sqlClause.ExpressionsPrefix = "(";
            sqlClause.ExpressionsSeparator = ") " + logic + " (";
            sqlClause.ExpressionsSuffix = ")";
            sqlClause.Suffix = null;

            return sqlClause;
        }


        /// <summary>
        ///     Initialize <see cref="SqlClause" /> to represent the sql clause of statements list.
        ///		The format is:
        ///		<code>
        ///			<paramref name="statement"/>
        ///				expression1
        ///				expression2
        ///				...
        ///		</code>
        /// </summary>
        [NotNull]
        public static SqlClause AsStatementsClause ([NotNull] this SqlClause sqlClause, string statement = null, string indent = "\t")
        {
            sqlClause.Prefix = statement + Environment.NewLine;
            sqlClause.ExpressionsPrefix = indent;
            sqlClause.ExpressionsSeparator = Environment.NewLine + indent;
            sqlClause.ExpressionsSuffix = Environment.NewLine;
            sqlClause.Suffix = null;

            return sqlClause;
        }
    }
}