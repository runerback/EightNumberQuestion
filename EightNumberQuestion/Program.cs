using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EightNumberQuestion.Solvers;

namespace EightNumberQuestion
{
	class Program
	{
		static void Main(string[] args)
		{
			SetConsoleCtrlHandler(ConsoleStateCheck, true);

			var originConsoleWriter = Console.Out;
			var fs = new FileStream("log.txt", FileMode.Create);
			var writer = new StreamWriter(fs);
			Console.SetOut(writer);

			Closing += delegate
			{
				Console.SetOut(originConsoleWriter);
				fs.Flush();
				writer.Dispose();
				fs.Dispose();
			};

			try
			{
				var board = new Board();
				var solvers = new Solver[]
				{
					new RandomMoveSolver(),
					new RandomMoveWithEmptyCellSolver(),
					new RandomReliableMoveSolver(),
					new RandomReliableMoveAndReduceHSolver(),
					new ReliableMoveAndReduceHSolver(),
				};

				solvers[4].Solve(board);
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
