using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	sealed class StepRecorder
	{
		private MoveStep lastMoveStep = null;

		public void RecordLastSucceedStep(MoveStep stepInfo)
		{
			if (stepInfo == null)
				throw new ArgumentNullException("stepInfo");
			this.lastMoveStep = stepInfo;
		}

		public bool TryRevertStep(out MoveStep stepInfo)
		{
			stepInfo = null;

			var lastStep = this.lastMoveStep;
			if (lastStep == null)
				return false;

			stepInfo = lastStep;
			this.lastMoveStep = null;
			return true;
		}

		public void Reset()
		{
			this.lastMoveStep = null;
		}
	}
}
