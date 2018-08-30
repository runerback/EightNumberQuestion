using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	public static class Tool
	{
		public static int GetRowIndex(int index, int width)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			if (width <= 0)
				throw new ArgumentOutOfRangeException("width");

			return index / width;
		}

		public static int GetColumnIndex(int index, int width)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			if (width <= 0)
				throw new ArgumentOutOfRangeException("width");

			return index % width;
		}

		public static int GetManhattanDistance(int fromIndex, int toIndex, int width)
		{
			if (fromIndex < 0)
				throw new ArgumentOutOfRangeException("fromIndex");
			if (toIndex < 0)
				throw new ArgumentOutOfRangeException("toIndex");
			if (width <= 0)
				throw new ArgumentOutOfRangeException("width");

			if (fromIndex == toIndex)
				return 0;

			return Math.Abs(GetRowIndex(fromIndex, width) - GetRowIndex(toIndex, width)) +
				Math.Abs(GetColumnIndex(fromIndex, width) - GetColumnIndex(toIndex, width));
		}

		public static IEnumerable<int> GetAvaliableMoveIndex(int sourceIndex, int width, int height)
		{
			if (sourceIndex < 0)
				throw new ArgumentException("sourceIndex");
			if (width <= 0)
				throw new ArgumentException("width");
			if (height <= 0)
				throw new ArgumentException("height");

			var indexes = Enumerable.Empty<int>();

			var rowIndex = GetRowIndex(sourceIndex, width);
			var columnIndex = GetColumnIndex(sourceIndex, width);

			if (columnIndex > 0)
				yield return sourceIndex - 1;
			if (columnIndex < width - 1)
				yield return sourceIndex + 1;
			if (rowIndex > 0)
				yield return sourceIndex - width;
			if (rowIndex < height - 1)
				yield return sourceIndex + width;
		}

		public static IEnumerable<int> GetAvaliableMoveIndex(int sourceIndex, int size)
		{
			return GetAvaliableMoveIndex(sourceIndex, size, size);
		}
	}
}
