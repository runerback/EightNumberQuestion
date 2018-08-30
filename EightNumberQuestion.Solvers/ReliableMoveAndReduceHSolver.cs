using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public sealed class ReliableMoveAndReduceHSolver : Solver
	{
		protected override void TrySolve(Board board)
		{
			int sourceIndex, targetIndex;
			while (!board.IsSolved)
			{
				targetIndex = board.EmptyCellIndex;
				sourceIndex = getAnotherIndex(targetIndex, board);
				//int currentDistance = board.GetTotalManhattanDistance();
				if (!board.TryMove(sourceIndex, targetIndex))
					throw new Exception("bad move");
				//int movedDistance = board.GetTotalManhattanDistance();
				//if (movedDistance < currentDistance)
				//	board.TryRevertLastStep();
				Console.WriteLine(board.ToString());
			}
		}

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

			return moveableIndexes
				.Where(index => index >= Board.MinValue && index < Board.LEN)
				.OrderBy(index =>
				{
					var goalIndex = board[index] - 1;
					//cell will moved to empty cell
					return Tool.GetManhattanDistance(emptyIndex, goalIndex, Board.SIZE, Board.SIZE);
				})
				.First();
		}
	}
}
