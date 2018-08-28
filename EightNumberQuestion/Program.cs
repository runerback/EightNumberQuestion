using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightNumberQuestion
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var board = new Board();
				new RandomMoveSolver().Solve(board);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			finally
			{
				Console.ReadKey(true);
			}
		}
	}
}
