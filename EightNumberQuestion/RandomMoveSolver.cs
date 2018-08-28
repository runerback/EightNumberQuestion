using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	public sealed class RandomMoveSolver : Solver
	{
		private readonly Random rnd = new Random((int)DateTime.Now.Ticks);
		private const long MaxRetryCount = long.MaxValue;

		protected override void TrySolve(Board board)
		{
			int sourceIndex, targetIndex;
			long counter = 0;
			while (!board.IsSolved & counter++ < MaxRetryCount)
			{
				getTwoDiffIndex(out sourceIndex, out targetIndex);
				board.TryMove(sourceIndex, targetIndex);
			}
		}

		private void getTwoDiffIndex(out int index1, out int index2)
		{
			index1 = rnd.Next(Board.MinValue, Board.LEN);
			index2 = index1;
			do
			{
				index2 = rnd.Next(Board.MinValue, Board.LEN);
			} while (index1 == index2);
		}
	}
}
