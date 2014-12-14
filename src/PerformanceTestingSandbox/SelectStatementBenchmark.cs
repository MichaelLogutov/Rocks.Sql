using Rocks.Sql.MsSql.Tests;

namespace PerformanceTestingSandbox
{
	internal class SelectStatementBenchmark
	{
		public void Run ()
		{
			for (var k = 0; k < 1000; k++)
				SelectUseCaseTests.Benchmark ();
		}
	}
}