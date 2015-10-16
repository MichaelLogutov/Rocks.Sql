using System;
using FluentAssertions;
using Xunit;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	public class ContainsTests
	{
		[Fact]
		public void ContainsExpressions_NoExpressions_ReturnsFalse ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.ContainsExpression ("key");


			// arrange
			result.Should ().BeFalse ();
		}


		[Fact]
		public void ContainsExpressions_HasNotKeyedExpression_ReturnsFalse ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("a");


			// act
			var result = sut.ContainsExpression ("key");


			// arrange
			result.Should ().BeFalse ();
		}


		[Fact]
		public void ContainsExpressions_OneExpression_ReturnsTrue ()
		{
			// arrange
			var sut = new SqlClause ();

			sut.Add ("key", "a");


			// act
			var result = sut.ContainsExpression ("key");


			// arrange
			result.Should ().BeTrue ();
		}
	}
}



