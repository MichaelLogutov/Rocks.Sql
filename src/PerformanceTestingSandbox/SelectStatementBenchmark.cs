using System;
using System.Diagnostics;
using Rocks.Sql.MsSql.Tests;

namespace PerformanceTestingSandbox
{
	internal class SelectStatementBenchmark
	{
		public void Run ()
		{
			// warmup
			for (var k = 0; k < 10; k++)
				SelectUseCaseTests.Benchmark ();

			
			// benchmark
			var stopwatch = new Stopwatch ();
			var max_tries = 100000;

			stopwatch.Start ();

			for (var k = 0; k < max_tries; k++)
				SelectUseCaseTests.Benchmark ();

			stopwatch.Stop ();

			// report
			Console.WriteLine ("{0:#,##0.000} ms per call", (double) stopwatch.ElapsedMilliseconds / max_tries);
		}
	}
}