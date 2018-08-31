using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace EightNumberQuestion.Solvers
{
	public sealed class TreeSolver : Solver
	{
		protected override void TrySolve(Board board, System.Threading.CancellationToken cancellationToken)
		{
			var rootBranch = new BoardBranch(board);

			var branches = new TransformManyBlock<BoardBranch, BoardBranch>(
				data => data,
				new ExecutionDataflowBlockOptions
				{
					CancellationToken = cancellationToken
				});

			var solver = new TransformBlock<BoardBranch, BoardBranchSolveResult>(
				branch => branch.Solve(),
				new ExecutionDataflowBlockOptions
				{
					CancellationToken = cancellationToken
				});

			var checker = new ActionBlock<BoardBranchSolveResult>(result =>
			{
				var branch = result.Branch;
				Console.WriteLine(branch.ToString());

				if (result.Solved)
					branches.Complete(); //always can solve, branch count of each step count is const
				else
					branches.Post(branch);
			},
			new ExecutionDataflowBlockOptions
			{
				CancellationToken = cancellationToken
			});

			var solverLink = branches.LinkTo(solver);
			branches.Completion.ContinueWith(t =>
			{
				solver.Complete();
				solverLink.Dispose();
			});

			var checkerLink = solver.LinkTo(checker);
			solver.Completion.ContinueWith(t =>
			{
				checker.Complete();
				checkerLink.Dispose();
			});

			branches.Post(rootBranch);

			checker.Completion.Wait();
		}

		sealed class BoardBranch : IEnumerable<BoardBranch>
		{
			//initial state
			public BoardBranch(Board board)
				: this(board, -1, -1, new BranchInfo())
			{
				this.isRoot = true;
			}

			//branches
			private BoardBranch(Board board, int sourceIndex, int targetIndex, BranchInfo branchInfo)
			{
				if (board == null)
					throw new ArgumentNullException("board");
				this.board = board;//.Copy(true);
				this.sourceIndex = sourceIndex;
				this.targetIndex = targetIndex;
				this.info = branchInfo;
			}

			private readonly Board board;
			private readonly BranchInfo info;

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

			public IEnumerator<BoardBranch> GetEnumerator()
			{
				var board = this.board;
				var branchInfo = this.info;
				var targetIndex = board.EmptyCellIndex;

				return Tool.GetAvaliableMoveIndex(targetIndex, Board.SIZE)
					.Where(item => item != branchInfo.LastTargetIndex)
					.Select(item => new BoardBranch(board.Copy(true), item, targetIndex, new BranchInfo(branchInfo, item, targetIndex)))
					.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public override string ToString()
			{
				return string.Format("[{0}]\r\n{1}", this.info.Indexes, this.board.ToString());
			}

			sealed class BranchInfo
			{
				public BranchInfo() { }

				public BranchInfo(BranchInfo previousBranch, int sourceIndex, int lastTargetIndex)
				{
					if (previousBranch == null)
						throw new ArgumentNullException("previousBranch");
					this.indexes = string.Format("{0}{1}", previousBranch.indexes, sourceIndex);
					this.lastTargetIndex = lastTargetIndex;
				}

				private readonly string indexes = null;
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
			}
		}

		sealed class BoardBranchSolveResult
		{
			public BoardBranchSolveResult(BoardBranch branch, bool solved)
			{
				if (branch == null)
					throw new ArgumentNullException("branch");
				this.branch = branch;
				this.solved = solved;
			}

			private readonly BoardBranch branch;
			public BoardBranch Branch
			{
				get { return this.branch; }
			}

			private readonly bool solved;
			public bool Solved
			{
				get { return solved; }
			}
		}
	}
}
