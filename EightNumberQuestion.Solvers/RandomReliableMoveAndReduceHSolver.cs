using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public sealed class RandomReliableMoveAndReduceHSolver : RandomSolver
	{
		protected override void TrySolve(Board board, System.Threading.CancellationToken cancellationToken)
		{
			board.RecordLastStep = true;

			var maxRetryCount = MaxRetryCount;

			int sourceIndex, targetIndex;
			long counter = 0;

			while (!board.IsSolved)
			{
				if (cancellationToken.IsCancellationRequested)
					break;
				targetIndex = board.EmptyCellIndex;
				sourceIndex = getAnotherIndex(targetIndex);
				int currentDistance = board.GetTotalManhattanDistance();
				if (!board.TryMove(sourceIndex, targetIndex))
					throw new Exception("bad move");
				int movedDistance = board.GetTotalManhattanDistance();
				if (movedDistance >= currentDistance)
					board.TryRevertLastStep();
				else
				{
					Console.WriteLine(board.ToString());
					if (counter++ == maxRetryCount)
						break;
				}
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
