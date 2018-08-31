using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
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

		public void RevertLastSucceedMove()
		{
			if (this.successedStepCount == 0)
				return;

			this.successedStepCount--;
			this.totalStepCount--;
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

		public void UpdateBy(MoveCounter other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			this.totalStepCount = other.totalStepCount;
			this.successedStepCount = other.successedStepCount;
			this.failedStepCount = other.failedStepCount;
		}
	}

}
