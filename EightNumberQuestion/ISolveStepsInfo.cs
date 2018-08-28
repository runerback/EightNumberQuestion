using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	public interface ISolveStepsInfo
	{
		int TotalStepCount { get; }
		int SuccessedStepCount { get; }
		int FailedStepCount { get; }

		string Summary { get; }
	}
}
