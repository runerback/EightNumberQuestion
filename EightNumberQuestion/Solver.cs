using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	public abstract class Solver
	{
		public bool Solve(Board board)
		{
			if (board == null)
				throw new ArgumentNullException("board");
			if (board.IsSolved)
				return true;

			int maxCost = board.GetTotalManhattanDistance();
			board.ResetCountSteps();

			TrySolve(board);

			Console.WriteLine(board.SolveStepsInfo.Summary);
			if (board.IsSolved)
			{
				Console.WriteLine("Solved");
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
