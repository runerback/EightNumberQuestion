using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public sealed class RandomMoveSolver : RandomSolver
	{
		protected override void TrySolve(Board board, System.Threading.CancellationToken cancellationToken)
		{
			var maxRetryCount = MaxRetryCount;

			int sourceIndex, targetIndex;
			long counter = 0;
			while (!board.IsSolved & counter++ < maxRetryCount)
			{
				if (cancellationToken.IsCancellationRequested)
					break;
				getTwoDiffIndex(out sourceIndex, out targetIndex);
				board.TryMove(sourceIndex, targetIndex);
			}
		}

		private void getTwoDiffIndex(out int index1, out int index2)
		{
			var rnd = this.rnd;

			index1 = rnd.Next(Board.MinValue, Board.LEN);
			index2 = index1;
			do
			{
				index2 = rnd.Next(Board.MinValue, Board.LEN);
			} while (index1 == index2);
		}
	}
}
