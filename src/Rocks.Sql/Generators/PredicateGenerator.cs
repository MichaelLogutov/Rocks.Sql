using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TextTemplating;

namespace Rocks.Sql.Generators
{
	public abstract class PredicateGenerator
	{
		#region Private fields

		private readonly TextTransformation tt;

		#endregion

		#region Construct

		protected PredicateGenerator (TextTransformation tt)
		{
			this.tt = tt;
		}

		#endregion

		#region Public methods

		/// <summary>
		///     Performs generation of code.
		/// </summary>
		public void Generate ()
		{
			var supported_types = this.GetSupportedTypes ();

			if (supported_types.Count == 0)
				return;

			foreach (var type in supported_types)
				this.GenerateEqualMethod (type);
		}

		#endregion

		#region Protected methods

		/// <summary>
		///     Returns a list of types of values for methods that will be generated.
		/// </summary>
		[NotNull]
		protected abstract IList<string> GetSupportedTypes ();


		/// <summary>
		///     Generates a code for creation of <see cref="IDbDataParameter" /> from
		///     given name and value parameters.
		/// </summary>
		[NotNull]
		protected abstract string GetCreateDbParameterCode (string type, string nameParameterName, string valueParameterName);


		protected virtual void GenerateEqualMethod (string type)
		{
			this.tt.Write (
				@"		/// <summary>
		///     Adds ""<paramref name=""columnName"" /> = <paramref name=""parameterName"" />""
		///     expression to the clause.
		///		If <paramref name=""value""/> is null then nothing will be added.
		/// </summary>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static SqlClause AddEquals (this SqlClause sqlClause,
										   [NotNull] string columnName,
										   [NotNull] string parameterName,
										   int? value)
		{
			if (value == null)
				return sqlClause;

			return sqlClause.AddEquals (columnName, ");

			this.tt.PushIndent ("			                                        ");
			this.tt.WriteLine (this.GetCreateDbParameterCode (type, "parameterName", "value") + ");");
			this.tt.PopIndent ();
			this.tt.WriteLine ("			}");
		}


		//protected virtual void Write (string code)
		//{
		//	var lines = code.Split (new[] { "\r\n" }, StringSplitOptions.None);

		//	//this.tt.PushIndent
		//	foreach (var line in lines)
		//		this.tt.WriteLine (line);
		//}

		#endregion
	}
}