using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace EightNumberQuestion.Solvers
{
	public class TreeSolver : Solver
	{
		protected override void TrySolve(Board board, System.Threading.CancellationToken cancellationToken)
		{
			var rootBranch = new BoardBranch(board);

            var solverBlock = new TransformBlock<BoardBranch, BoardBranchSolveResult>(
                branch => branch.Solve(),
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = cancellationToken,
                    BoundedCapacity = 10,
                    MaxDegreeOfParallelism = 3,
                    SingleProducerConstrained = true
                });

			var checker = new ActionBlock<BoardBranchSolveResult>(result =>
			{
                var branch = result.Branch;
                if(board.EmptyCellIndex == 4)
                    Console.WriteLine(branch.ToString());

				if (result.Solved)
					solverBlock.Complete(); //always can solve, branch count of each step count is const
				else
					foreach(var innerBranch in branch.GetBranches(GetCost))
						solverBlock.Post(innerBranch);
				result.Dispose();
				result = null;
			},
			new ExecutionDataflowBlockOptions
			{
				CancellationToken = cancellationToken
			});

			var checkerLink = solverBlock.LinkTo(checker);
			solverBlock.Completion.ContinueWith(t =>
			{
				checker.Complete();
				checkerLink.Dispose();
			});

			foreach(var branch1 in rootBranch.GetBranches(GetCost))
				solverBlock.Post(branch1);
			rootBranch.Dispose();
            rootBranch = null;

			checker.Completion.Wait();
		}

        protected virtual int GetCost(int sourceIndex, int targetIndex, int goalIndex)
        {
            return 1;
        }

		sealed class BoardBranch : IDisposable
		{
			//initial state
			public BoardBranch(Board board)
				: this(board, -1, -1)
			{
				this.isRoot = true;
			}

			//branches
			private BoardBranch(Board board, int sourceIndex, int targetIndex)
			{
				if (board == null)
					throw new ArgumentNullException("board");
				this.board = board;
                this.sourceIndex = sourceIndex;
				this.targetIndex = targetIndex;
                this.boardInfo = board.ToString();
            }

			private Board board;

			private readonly int sourceIndex;
			private readonly int targetIndex;

			private readonly bool isRoot = false;

			public BoardBranchSolveResult Solve()
			{
				if (isRoot)
					throw new InvalidOperationException("root branch");

				if (!this.board.TryMove(this.sourceIndex, this.targetIndex))
					throw new Exception("bad move");
				return new BoardBranchSolveResult(this, board.IsSolved);
			}

            public IEnumerable<BoardBranch> GetBranches(Func<int, int, int, int> cost)
            {
                if (cost == null)
                    throw new ArgumentNullException("cost");

                var board = this.board;
                var targetIndex = board.EmptyCellIndex;

                return Tool.GetAvaliableMoveIndex(targetIndex, Board.SIZE)
                    .Where(item => item != this.targetIndex)
                    .OrderBy(item => cost(item, targetIndex, board.GetGoalIndex(item)))
                    .Select(item => new BoardBranch(
                        board.Copy(true),
                        item,
                        targetIndex))
                    .Take(1);
            }

            private string boardInfo;
			public override string ToString()
			{
                return this.boardInfo;

            }

            public void Dispose()
            {
                this.board = null;
                this.boardInfo = null;
            }

            [Obsolete("this need more and more memory")]
			sealed class BranchInfo : IDisposable
			{
				public BranchInfo() { }

				public BranchInfo(BranchInfo previousBranch, int sourceIndex, int lastTargetIndex)
				{
                    if (previousBranch == null)
                        throw new ArgumentNullException("previousBranch");
                    this.indexes = previousBranch.indexes + sourceIndex.ToString(); //this one eat memory
                    this.lastTargetIndex = lastTargetIndex;
				}

				private string indexes = null;
				public string Indexes
				{
					get { return indexes; }
				}

				private readonly int lastTargetIndex = -1;
				public int LastTargetIndex
				{
					get { return this.lastTargetIndex; }
				}

				public override string ToString()
				{
					return indexes ?? "root";
				}

                public void Dispose()
                {
                    this.indexes = null;
                }
            }
		}

		sealed class BoardBranchSolveResult : IDisposable
		{
			public BoardBranchSolveResult(BoardBranch branch, bool solved)
			{
				if (branch == null)
					throw new ArgumentNullException("branch");
				this.branch = branch;
				this.solved = solved;
			}

			private BoardBranch branch;
			public BoardBranch Branch
			{
				get { return this.branch; }
			}

			private readonly bool solved;
			public bool Solved
			{
				get { return solved; }
			}

			public void Dispose()
			{
				this.branch.Dispose();
				this.branch = null;
			}
		}
	}
}
