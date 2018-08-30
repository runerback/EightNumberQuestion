using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	sealed class MoveStep
	{
		public MoveStep(int fromIndex, int toIndex)
		{
			this.from = fromIndex;
			this.to = toIndex;
		}

		private readonly int from;
		public int From
		{
			get { return this.from; }
		}

		private readonly int to;
		public int To
		{
			get { return this.to; }
		}
	}
}
