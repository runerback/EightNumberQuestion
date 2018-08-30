using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
	public sealed class ReliableMoveAndReduceHCostSolver : ReliableSolver
	{
		protected override int getCost(int sourceIndex, Board board)
		{
			var goalIndex = board[sourceIndex] - 1;
			return Tool.GetManhattanDistance(sourceIndex, goalIndex, Board.SIZE);
		}
	}
}
