using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EightNumberQuestion.Solvers;
using System.Threading;
using System.Collections.Concurrent;

namespace EightNumberQuestion
{
	class Program
	{
		static void Main(string[] args)
		{
			SetConsoleCtrlHandler(ConsoleStateCheck, true);

			//var originConsoleWriter = Console.Out;
			//var fs = new FileStream("log.txt", FileMode.Create);
			//var writer = new StreamWriter(fs);
			//Console.SetOut(writer);
			
				var waitTime = TimeSpan.FromMinutes(1);
			var cancellationTokenSource = new CancellationTokenSource(waitTime);

			Closing += delegate
			{
			//	Console.SetOut(originConsoleWriter);
			//	fs.Flush();
			//	writer.Dispose();
			//	fs.Dispose();
				cancellationTokenSource.Dispose();
			};
			try
			{
				var board = new Board();

				Console.WriteLine("Initial state");
				Console.WriteLine(board.ToString());

				board.ResetCountSteps();

				var solvers = new Solver[]
				{
					new RandomReliableMoveSolver(),
					new ReliableMoveAndReduceHSolver(),
					new ReliableMoveAndReduceHCostSolver(),
				};

				var exceptions = new ConcurrentQueue<Exception>();

				using (cancellationTokenSource)
				{
					var cancellationToken = cancellationTokenSource.Token;

					Parallel.ForEach(solvers, (solver, state) =>
					{
						solver.Solve(board.Copy(), cancellationToken)
							.ContinueWith(t =>
							{
								if (t.IsFaulted)
								{
									foreach (var inner in t.Exception.InnerExceptions)
										exceptions.Enqueue(inner);
									return;
								}

								if (t.Result)
								{
									cancellationTokenSource.Cancel();
									state.Stop();
								}
							}).Wait();
					});
				}

				if (!exceptions.IsEmpty)
					throw new AggregateException(exceptions).Flatten();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			finally
			{
				Console.ReadKey(true);
			}
		}

		private static bool ConsoleStateCheck(int CtrlType)
		{
			if (CtrlType == 2)
			{
				if (Closing != null)
					Closing();
			}
			return true;
		}

		private static Action Closing;

		[DllImport("Kernel32")]
		public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

		public delegate bool HandlerRoutine(int CtrlType);
	}
}
