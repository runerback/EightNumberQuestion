using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public sealed class RandomReliableMoveSolver : RandomSolver
	{
		protected override void TrySolve(Board board)
		{
			var maxRetryCount = MaxRetryCount;

			int sourceIndex, targetIndex;
			long counter = 0;
			while (!board.IsSolved & counter++ < maxRetryCount)
			{
				targetIndex = board.EmptyCellIndex;
				sourceIndex = getAnotherIndex(targetIndex);
				if (!board.TryMove(sourceIndex, targetIndex))
					throw new Exception("bad move");
			}
		}

		private int getAnotherIndex(int emptyIndex)
		{
			int[] moveableIndexes = new int[4]
			{
				emptyIndex - 3, //up
				emptyIndex - 1, //left
				emptyIndex + 1, //right
				emptyIndex + 3 //down
			};
			switch (emptyIndex % 3)
			{
				case 0:
					moveableIndexes[1] = -1;
					break;
				case 2:
					moveableIndexes[2] = -1;
					break;
				default: break;
			}

			var filtered = moveableIndexes
				.Where(item => item >= Board.MinValue && item < Board.LEN)
				.ToArray();

			return filtered[this.rnd.Next(0, filtered.Length)];
		}
	}
}
