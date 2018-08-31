using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public sealed class ReliableMoveAndReduceHSolver : ReliableSolver
	{
		protected override int getCost(int sourceIndex, Board board)
		{
            var goalIndex = board.GetGoalIndex(sourceIndex);
			return Tool.GetManhattanDistance(board.EmptyCellIndex, goalIndex, Board.SIZE);
		}
	}
}
