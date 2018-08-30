using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public abstract class Solver
	{
		public async Task<bool> Solve(Board board, CancellationToken cancellationToken)
		{
			if (board == null)
				throw new ArgumentNullException("board");
			if (cancellationToken == null)
				throw new ArgumentNullException("cancellationToken");
			if (board.IsSolved)
				return true;

			var maxCost = board.GetTotalManhattanDistance();

			var solved = await SolveInTask(board, cancellationToken);
			
			Console.WriteLine();
			Console.WriteLine(board.SolveStepsInfo.Summary);

			if (solved)
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

		private async Task<bool> SolveInTask(Board board, CancellationToken cancellationToken)
		{
			await Task.Run(() =>
				TrySolve(board, cancellationToken),
				cancellationToken);

			return board.IsSolved;
		}

		/// <summary>
		/// try to solve a eight number question
		/// </summary>
		/// <param name="board">eight number question data</param>
		protected abstract void TrySolve(Board board, CancellationToken cancellationToken);
	}
}
