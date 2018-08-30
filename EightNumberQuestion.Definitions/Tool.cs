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

		public static int GetColumnIndex(int index, int height)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");
			if (height <= 0)
				throw new ArgumentOutOfRangeException("height");

			return index % height;
		}

		public static int GetManhattanDistance(int fromIndex, int toIndex, int width, int height)
		{
			if (fromIndex < 0)
				throw new ArgumentOutOfRangeException("fromIndex");
			if (toIndex < 0)
				throw new ArgumentOutOfRangeException("toIndex");
			if (width <= 0)
				throw new ArgumentOutOfRangeException("width");
			if (height <= 0)
				throw new ArgumentOutOfRangeException("height");

			if (fromIndex == toIndex)
				return 0;

			return Math.Abs(GetRowIndex(fromIndex, width) - GetRowIndex(toIndex, width)) +
				Math.Abs(GetColumnIndex(fromIndex, height) - GetColumnIndex(toIndex, height));
		}
	}
}
