using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	public sealed class Board : IEnumerable<byte>
	{
		public Board() : this(true) { }

		private Board(bool generate)
		{
			if (generate)
				Reset();
		}

		public const int LEN = 9;
		public const int SIZE = 3;
		public const int MinValue = 0;
		public const int GOAL = 0;
		private readonly byte[] data = new byte[LEN];

		public void Reset()
		{
			var data = this.data;

			//int index = 0;
			//foreach (byte value in Enumerable.Range(MinValue, LEN)
			//	.OrderBy(item => Guid.NewGuid()))
			//{
			//	if (value == MinValue)
			//		emptyIndex = index;
			//	data[index++] = value;
			//}

			//while (GetTotalManhattanDistance() == GOAL)
			//	Distribute();

			//to make things more simple, always put empty cell to center
			var emptyIndex = 4;

			using (var valueIterator = Enumerable.Range(MinValue + 1, LEN - 1)
				.OrderBy(item => Guid.NewGuid())
				.GetEnumerator())
			using (var indexIterator = Enumerable.Range(MinValue, LEN)
				.GetEnumerator())
			{
				while (valueIterator.MoveNext() && indexIterator.MoveNext())
				{
					if (indexIterator.Current == emptyIndex)
						if (!indexIterator.MoveNext())
							break;
					data[indexIterator.Current] = (byte)valueIterator.Current;
				}
			}
			data[emptyIndex] = 0;
			this.emptyIndex = emptyIndex;

			this.isCompleted = false;
			this.stepRecorder.Reset();
			ResetCountSteps();
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

		public static int GetNextIndex(int index)
		{
			if (index == LEN - 1)
				return MinValue;
			return index + 1;
		}

		public int GetManhattanDistance(int index)
		{
			checkIndex(index);

			if (index == this.emptyIndex)
				return 0;
			return Tool.GetManhattanDistance(index, this.data[index] - 1, SIZE);
		}

		public int GetTotalManhattanDistance()
		{
			return Enumerable.Range(MinValue, LEN)
				.Sum(item => GetManhattanDistance(item));
		}

		public int IndexOf(int value)
		{
			if (value < 0 || value >= LEN - 1)
				return -1;
			if (value == 0)
				return emptyIndex;

			var data = this.data;
			for (int i = 0, j = LEN; i < j; i++)
			{
				if (data[i] == value)
					return i;
			}
			throw new InvalidOperationException("value no find. " + value);
		}

        public int GetGoalIndex(int index)
        {
            checkIndex(index);
            if (index == this.emptyIndex)
                throw new ArgumentException("index. empty cell index");
            return data[index] - 1;
        }

		public int this[int index]
		{
			get
			{
				checkIndex(index);
				return this.data[index];
			}
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

		private bool recordLastStep = false;
		public bool RecordLastStep
		{
			get { return recordLastStep; }
			set
			{
				if (value != recordLastStep)
				{
					recordLastStep = value;
					if (!value)
						stepRecorder.Reset();
				}
			}
		}
		private readonly StepRecorder stepRecorder = new StepRecorder();

		public bool TryMove(int sourceIndex, int targetIndex)
		{
			if (CanMove(sourceIndex, targetIndex))
			{
				var data = this.data;

				int sourceValue = data[sourceIndex];
				data[sourceIndex] = data[targetIndex];
				data[targetIndex] = (byte)sourceValue;

				emptyIndex = sourceIndex;

				if(recordLastStep)
					this.stepRecorder.RecordLastSucceedStep(new MoveStep(sourceIndex, targetIndex));
				this.counter.Succeed();

				if (GetTotalManhattanDistance() == 0)
					this.isCompleted = true;

				return true;
			}
			else
			{
				this.counter.Failed();
				return false;
			}
		}

		public bool CanMove(int sourceIndex, int targetIndex)
		{
			checkIndex(sourceIndex);
			checkIndex(targetIndex);

			if (sourceIndex == this.emptyIndex)
				return false; //source is empty
			if (targetIndex != this.emptyIndex)
				return false; //target is not empty

			var sourceRowIndex = Tool.GetRowIndex(sourceIndex, SIZE);
			var sourceColumnIndex = Tool.GetColumnIndex(sourceIndex, SIZE);
			var targetRowIndex = Tool.GetRowIndex(targetIndex, SIZE);
			var targetColumnIndex = Tool.GetColumnIndex(targetIndex, SIZE);

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

			return true;
		}

		public bool TryRevertLastStep()
		{
			MoveStep lastStep;
			if (!this.stepRecorder.TryRevertStep(out lastStep))
				return false;

			if (lastStep.From != this.emptyIndex)
				return false;

			this.counter.RevertLastSucceedMove();
			TryMove(lastStep.To, lastStep.From);
			this.counter.RevertLastSucceedMove();
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

		public Board Copy(bool copyStepCount = false)
		{
			var copied = new Board(false);

			copied.emptyIndex = this.emptyIndex;
			copied.recordLastStep = this.recordLastStep;

			var sourceData = this.data;
			var targetData = copied.data;
			for (int i = MinValue, j = LEN; i < j; i++)
			{
				targetData[i] = sourceData[i];
			}

			if (copyStepCount)
				copied.counter.UpdateBy(this.counter);

			return copied;
		}

		public IEnumerator<byte> GetEnumerator()
		{
			return data.AsEnumerable().GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}
