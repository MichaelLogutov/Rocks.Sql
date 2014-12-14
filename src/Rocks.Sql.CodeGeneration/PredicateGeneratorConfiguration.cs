using System;
using System.Data;
using System.Linq;

namespace Rocks.Sql.CodeGeneration
{
	public abstract class PredicateGeneratorConfiguration
	{
		#region Construct

		protected PredicateGeneratorConfiguration (string type)
		{
			this.Type = type;

			this.IsNullable = true;
			this.GenerateEqualsMethods = true;
			this.GenerateGreaterOrLessMethods = true;
			this.GenerateLikeMethods = true;
		}

		#endregion

		#region Public properties

		public string Type { get; set; }
		public bool IsNullable { get; set; }
		public bool GenerateEqualsMethods { get; set; }
		public bool GenerateGreaterOrLessMethods { get; set; }
		public bool GenerateLikeMethods { get; set; }

		#endregion

		#region Public methods

		/// <summary>
		///     Returns full type declaration.
		/// </summary>
		public string GetTypeDeclaration ()
		{
			return this.IsNullable ? this.Type + "?" : this.Type;
		}


		/// <summary>
		///     Generates a code for creation of <see cref="IDbDataParameter" /> from
		///     given name and value parameters.
		/// </summary>
		public abstract string GetCreateDbParameterCode (string nameParameterName, string valueParameterName);

		#endregion
	}
}