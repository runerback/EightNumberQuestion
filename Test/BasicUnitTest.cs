using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
				.SequenceEqual(board.OrderBy(item => item).Select(item => (int)item)));
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

		[TestMethod]
		public void GetManhattanDistanceTest()
		{
			Assert.AreEqual(1, Tool.GetManhattanDistance(0, 1, 3));
			Assert.AreEqual(3, Tool.GetManhattanDistance(0, 7, 3));
		}

        [TestMethod]
        public void MemoryAllocateTest()
        {
            var board = new Board();
            for (long i = 0, j = int.MaxValue; i < j; i++)
            {
                var copied = board.Copy(i % 2 == 0);
            }


        }

		[TestMethod]
		public void TwoSumTest()
		{
			var a = TwoSum(new int[] { 2, 7, 11, 15 }, 9);
		}

		public int[] TwoSum(int[] nums, int target)
		{
			return TwoSumIterator(
				nums.AsEnumerable().Select((value, index) => new NumInfo(value, index)),
				target)
				.Select(item => item.Index)
				.OrderBy(item => item)
				.ToArray();
		}

		private IEnumerable<NumInfo> TwoSumIterator(IEnumerable<NumInfo> nums, int target)
		{
			var groups = nums
					.GroupBy(item => item <= ((double)target / 2))
					.OrderBy(item => !item.Key);
			if (groups.Count() == 1)
			{
				foreach (var item in nums.OrderBy(item => item).Reverse().Take(2))
					yield return item;
				yield break;
			}

			using (var leftIterator = groups
				.First()
				.OrderByDescending(item => item)
				.GetEnumerator())
			using (var rightIterator = groups
				.Skip(1)
				.First()
				.OrderBy(item => item)
				.GetEnumerator())
			{
				bool moveLeft = true, moveRight = true, leftMoved = true, rightMoved = true;

				while (leftMoved || rightMoved)
				{
					leftMoved = moveLeft && leftIterator.MoveNext();
					rightMoved = moveRight && rightIterator.MoveNext();

					int sum = leftIterator.Current + rightIterator.Current;
					int test = Math.Abs(sum).CompareTo(Math.Abs(target));
					if (test == 0)
						break;
					else if (test < 0)
					{
						moveLeft = false;
						moveRight = true;
					}
					else
					{
						moveLeft = true;
						moveRight = false;
					}
				}

				if (leftIterator.Current + rightIterator.Current != target)
					throw new Exception("wrong result");

				yield return leftIterator.Current;
				yield return rightIterator.Current;
			}
		}

		sealed class NumInfo : IComparable<NumInfo>
		{
			public NumInfo(int value, int index)
			{
				this.value = value;
				this.index = index;
			}

			private readonly int value;
			public int Value
			{
				get { return this.value; }
			}

			private readonly int index;
			public int Index
			{
				get { return index; }
			}

			public static implicit operator int(NumInfo info)
			{
				if (info == null)
					throw new ArgumentNullException("info");
				return info.value;
			}

			public override string ToString()
			{
				return value.ToString();
			}

			public int CompareTo(NumInfo other)
			{
				if (other == null)
					throw new ArgumentNullException("other");
				return this.value.CompareTo(other.value);
			}
		}
	}
}
