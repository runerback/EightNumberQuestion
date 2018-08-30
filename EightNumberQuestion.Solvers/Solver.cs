using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public abstract class Solver
	{
		public bool Solve(Board board)
		{
			if (board == null)
				throw new ArgumentNullException("board");
			if (board.IsSolved)
				return true;

			Console.WriteLine("Initial state");
			Console.WriteLine(board.ToString());

			int maxCost = board.GetTotalManhattanDistance();
			board.ResetCountSteps();

			TrySolve(board);

			Console.WriteLine(board.SolveStepsInfo.Summary);
			if (board.IsSolved)
			{
				Console.WriteLine("Solved");
				if (board.SolveStepsInfo.TotalStepCount > maxCost)
					Console.WriteLine("Not effective");
				return true;
			}
			else
			{
				Console.WriteLine("Not solve");
				return false;
			}
		}

		/// <summary>
		/// try to solve a eight number question
		/// </summary>
		/// <param name="board">eight number question data</param>
		protected abstract void TrySolve(Board board);
	}
}
