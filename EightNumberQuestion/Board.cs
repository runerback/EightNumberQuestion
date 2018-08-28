using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	public sealed class Board : IEnumerable<int>
	{
		public Board()
		{
			Distribute();
		}

		public const int LEN = 9;
		public const int SIZE = 3;
		public const int MinValue = 0;
		public const int GOAL = 0;
		private readonly int[] data = new int[LEN];

		public void Distribute()
		{
			var data = this.data;

			int index = 0;
			foreach (byte value in Enumerable.Range(MinValue, LEN)
				.OrderBy(item => Guid.NewGuid()))
			{
				if (value == MinValue)
					emptyIndex = index;
				data[index++] = value;
			}

			while (GetTotalManhattanDistance() == GOAL)
				Distribute();
		}

		public override string ToString()
		{
			var data = this.data;
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < 3; i++)
				builder.AppendLine(string.Join(" ", data.Skip(i * 3).Take(3)));
			return builder.Replace('0', ' ').ToString();
		}

		private static void checkIndex(int index)
		{
			if (index < MinValue || index >= LEN)
				throw new ArgumentException("index out of range");
		}

		public static int GetRowIndex(int index)
		{
			checkIndex(index);
			return index / SIZE;
		}

		public static int GetColumnIndex(int index)
		{
			checkIndex(index);
			return index % SIZE;
		}

		public static int GetNextIndex(int index)
		{
			if (index == LEN - 1)
				return MinValue;
			return index + 1;
		}

		public int GetManhattanDistance(int index)
		{
			checkIndex(index);

			var data = this.data;

			int value = data[index];
			if (value == MinValue)
				return MinValue; //not move empty cell deliberately

			int targetIndex = value - 1;

			return Math.Abs(GetRowIndex(index) - GetRowIndex(targetIndex)) +
				Math.Abs(GetColumnIndex(index) - GetColumnIndex(targetIndex));
		}

		public int GetTotalManhattanDistance()
		{
			return Enumerable.Range(MinValue, LEN)
				.Sum(item => GetManhattanDistance(item));
		}

		private int IndexOf(int value)
		{
			var data = this.data;
			for (int i = 0, j = LEN; i < j; i++)
			{
				if (data[i] == value)
					return i;
			}
			throw new InvalidOperationException("value no find. " + value);
		}

		private bool isCompleted = false;
		public bool IsSolved
		{
			get { return this.isCompleted; }
		}

		private int emptyIndex;
		public int EmptyCellIndex
		{
			get { return emptyIndex; }
		}

		public bool TryMove(int sourceIndex, int targetIndex)
		{
			checkIndex(sourceIndex);
			checkIndex(targetIndex);

			if (innerMove(sourceIndex, targetIndex))
			{
				this.counter.Succeed();
				return true;
			}
			else
			{
				this.counter.Failed();
				return false;
			}
		}

		private bool innerMove(int sourceIndex, int targetIndex)
		{
			if (sourceIndex == emptyIndex)
				return false; //source is empty
			if (targetIndex != emptyIndex)
				return false; //target is not empty

			var data = this.data;

			var sourceRowIndex = GetRowIndex(sourceIndex);
			var sourceColumnIndex = GetColumnIndex(sourceIndex);
			var targetRowIndex = GetRowIndex(targetIndex);
			var targetColumnIndex = GetColumnIndex(targetIndex);

			if (sourceRowIndex == targetRowIndex)
			{
				if (Math.Abs(sourceColumnIndex - targetColumnIndex) != 1)
					return false;
			}
			else if (sourceColumnIndex == targetColumnIndex)
			{
				if (Math.Abs(sourceRowIndex - targetRowIndex) != 1)
					return false;
			}
			else
				return false;

			int sourceValue = data[sourceIndex];
			data[sourceIndex] = data[targetIndex];
			data[targetIndex] = sourceValue;

			emptyIndex = sourceIndex;

			return true;
		}

		private readonly MoveCounter counter = new MoveCounter();
		public ISolveStepsInfo SolveStepsInfo
		{
			get { return counter; }
		}

		public void ResetCountSteps()
		{
			this.counter.Reset();
		}

		public IEnumerator<int> GetEnumerator()
		{
			return data.AsEnumerable().GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		sealed class MoveCounter : ISolveStepsInfo
		{
			private int totalStepCount = 0;
			public int TotalStepCount
			{
				get { return this.totalStepCount; }
			}

			private int successedStepCount = 0;
			public int SuccessedStepCount
			{
				get { return this.successedStepCount; }
			}

			private int failedStepCount = 0;
			public int FailedStepCount
			{
				get { return this.failedStepCount; }
			}

			public void Reset()
			{
				this.totalStepCount = 0;
			}

			public void Succeed()
			{
				this.successedStepCount++;
				this.totalStepCount++;
			}

			public void Failed()
			{
				this.failedStepCount++;
				this.totalStepCount++;
			}

			private string buildSummary()
			{
				if (this.totalStepCount == 0)
					return "No step";

				StringBuilder builder = new StringBuilder();
				builder.AppendLine(
					string.Format("Succeed step count: {0}", successedStepCount));
				builder.AppendLine(
					string.Format("Failed step count: {0}", failedStepCount));
				builder.AppendLine(
					string.Format("Total step count: {0}", totalStepCount));
				return builder.ToString();
			}

			public string Summary
			{
				get { return buildSummary(); }
			}

			public override string ToString()
			{
				return buildSummary();
			}
		}
	}
}
