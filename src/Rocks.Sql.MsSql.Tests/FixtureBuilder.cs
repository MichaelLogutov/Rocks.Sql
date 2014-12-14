using System;
using System.Linq;
using Ploeh.AutoFixture;

namespace Rocks.Sql.MsSql.Tests
{
	internal class FixtureBuilder
	{
		public IFixture Build ()
		{
			return new Fixture ();
		}
	}
}