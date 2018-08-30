using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public abstract class ReliableSolver : Solver
	{
		protected override void TrySolve(Board board, System.Threading.CancellationToken cancellationToken)
		{
			int sourceIndex, targetIndex;
			while (!board.IsSolved)
			{
				if (cancellationToken.IsCancellationRequested)
					break;
				targetIndex = board.EmptyCellIndex;
				sourceIndex = getAnotherIndex(targetIndex, board);
				if (!board.TryMove(sourceIndex, targetIndex))
					throw new Exception("bad move");
				lastMovedIndex = targetIndex;
			}
		}

		private int lastMovedIndex = -1;

		private int getAnotherIndex(int emptyIndex, Board board)
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

			return Tool.GetAvaliableMoveIndex(emptyIndex, Board.SIZE)
				.Where(item => item != lastMovedIndex)
				.OrderBy(index => getCost(index, board))
				.First();
		}

		protected abstract int getCost(int sourceIndex, Board board);
	}
}
