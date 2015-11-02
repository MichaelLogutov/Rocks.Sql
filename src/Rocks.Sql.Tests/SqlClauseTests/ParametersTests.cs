using System;
using System.Data.SqlClient;
using FluentAssertions;
using Xunit;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	public class ParametersTests
	{
		[Fact]
		public void GetParameters_NoParameters_ReturnsEmptyList ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.GetParameters ();


			// arrange
			result.Should ().BeEmpty ();
		}


		[Fact]
		public void GetParameters_OneParameter_NoName_Throws ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter ();


			// act
			Action action = () => sut.Add (parameter);


			// arrange
			action.ShouldThrow<ArgumentNullException> ()
			      .And.ParamName.Should ().Be ("parameter.ParameterName");
		}


		[Fact]
		public void GetParameters_OneParameter_ReturnsCorrectList ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter { ParameterName = "@test" };
			sut.Add (parameter);


			// act
			var result = sut.GetParameters ();


			// arrange
			result.Should ().Equal (parameter);
		}


		[Fact]
		public void ContainsParameters_NoParameters_ReturnsFalse ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.ContainsParameter ("@test");


			// arrange
			result.Should ().BeFalse ();
		}


		[Fact]
		public void ContainsParameters_OneParameter_ReturnsTrue ()
		{
			// arrange
			var sut = new SqlClause ();

			var parameter = new SqlParameter { ParameterName = "@test" };
			sut.Add (parameter);


			// act
			var result = sut.ContainsParameter ("@test");


			// arrange
			result.Should ().BeTrue ();
		}
	}
}


