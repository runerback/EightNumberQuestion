using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	public abstract class RandomSolver : Solver
	{
		protected readonly Random rnd = new Random((int)DateTime.Now.Ticks);
		protected const long MaxRetryCount = 20000;
	}
}
