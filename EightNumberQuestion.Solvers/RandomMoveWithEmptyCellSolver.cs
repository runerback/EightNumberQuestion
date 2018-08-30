using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public sealed class RandomMoveWithEmptyCellSolver : RandomSolver
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
				targetIndex = board.EmptyCellIndex;
				sourceIndex = getAnotherIndex(targetIndex);
				board.TryMove(sourceIndex, targetIndex);
			}
		}

		private int getAnotherIndex(int emptyIndex)
		{
			var rnd = this.rnd;

			int otherIndex;
			do
			{
				otherIndex = rnd.Next(Board.MinValue, Board.LEN);
			} while (emptyIndex == otherIndex);
			return otherIndex;
		}
	}
}
