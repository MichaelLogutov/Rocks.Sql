using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.SqlClauseTests
{
	[TestClass]
	public class ContainsTests
	{
		[TestMethod]
		public void ContainsExpressions_NoExpressions_ReturnsFalse ()
		{
			// arrange
			var sut = new SqlClause ();


			// act
			var result = sut.ContainsExpression ("key");


			// arrange
			result.Should ().BeFalse ();
		}


		[TestMethod]
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


		[TestMethod]
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
