using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public sealed class RandomReliableMoveSolver : RandomSolver
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
				if (!board.TryMove(sourceIndex, targetIndex))
					throw new Exception("bad move");
				lastMovedIndex = targetIndex;
			}
		}

		private int lastMovedIndex = -1;

		private int getAnotherIndex(int emptyIndex)
		{
			var moveableIndexes = Tool.GetAvaliableMoveIndex(emptyIndex, Board.SIZE)
				.Where(item => item != lastMovedIndex)
				.ToArray();

			return moveableIndexes[this.rnd.Next(0, moveableIndexes.Length)];
		}
	}
}
