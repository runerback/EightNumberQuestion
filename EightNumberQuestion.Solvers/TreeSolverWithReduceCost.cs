using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion.Solvers
{
    public sealed class TreeSolverWithReduceCost : TreeSolver
    {
        protected override int GetCost(int sourceIndex, int targetIndex, int goalIndex)
        {
            return Tool.GetManhattanDistance(targetIndex, goalIndex, Board.SIZE) -
                Tool.GetManhattanDistance(sourceIndex, goalIndex, Board.SIZE);
        }
    }
}
