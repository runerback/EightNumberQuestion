using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EightNumberQuestion.Test
{
	[TestClass]
	public class BasicUnitTest
	{
		[TestMethod]
		public void ValidateBoard()
		{
			var board = new Board();
			Assert.IsTrue(Enumerable.Range(Board.MinValue, Board.LEN)
				.SequenceEqual(board.OrderBy(item => item)));
			Assert.AreNotEqual(Board.GOAL, board.GetTotalManhattanDistance());
			Assert.IsFalse(board.IsSolved);
		}

		[TestMethod]
		public void BoardMoveTest()
		{
			var board = new Board();
			var emptyCellIndex = board.EmptyCellIndex;
			board.TryMove(emptyCellIndex, Board.GetNextIndex(emptyCellIndex));
		}
	}
}
