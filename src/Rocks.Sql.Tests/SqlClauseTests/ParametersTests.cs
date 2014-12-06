using System;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	[TestClass]
	public class ParametersTests
	{
		[TestMethod]
		public void GetParameters_NoParameters_ReturnsEmptyList ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.GetParameters ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void GetParameters_OneParameter_NoName_Throws ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter ();


			// act
			Action action = () => sut.AddParameter (parameter);


			// arrange
			action.ShouldThrow<ArgumentNullException> ()
			      .And.ParamName.Should ().Be ("ParameterName");
		}


		[TestMethod]
		public void GetParameters_OneParameter_ReturnsCorrectList ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter { ParameterName = "@test" };
			sut.AddParameter (parameter);


			// act
			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter);
		}


		[TestMethod]
		public void ContainsParameters_NoParameters_ReturnsFalse ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.ContainsParameter ("@test");


			// arrange
			result.Should ().BeFalse ();
		}


		[TestMethod]
		public void ContainsParameters_OneParameter_ReturnsTrue ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter { ParameterName = "@test" };
			sut.AddParameter (parameter);


			// act
			var result = sut.ContainsParameter ("@test");


			// arrange
			result.Should ().BeTrue ();
		}
	}
}