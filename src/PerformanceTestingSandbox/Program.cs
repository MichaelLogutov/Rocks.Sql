using System;

namespace PerformanceTestingSandbox
{
	internal class Program
	{
		private static void Main ()
		{
			try
			{
				new SelectStatementBenchmark ().Run ();
			}
			catch (Exception ex)
			{
				Console.WriteLine ("\n\n{0}\n\n", ex);
			}
		}
	}
}