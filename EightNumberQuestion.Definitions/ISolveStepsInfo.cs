
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
