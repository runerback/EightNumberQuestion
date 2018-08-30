using Runerback.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EightNumberQuestion.GUI.Controls
{
	sealed class BoardPresenter : Control
	{
		public BoardPresenter()
		{
			UpdateDataSource();
			SetCellCommand(new RelayCommand(moveCell, canMoveCell));
		}

		private readonly Board board = new Board();
		
		public const double SIZE = 36;

		public int[] DataSource
		{
			get { return (int[])GetValue(DataSourceProperty); }
		}

		static readonly DependencyPropertyKey DataSourcePropertyKey =
			DependencyProperty.RegisterReadOnly(
				"DataSource",
				typeof(int[]),
				typeof(BoardPresenter),
				new PropertyMetadata());

		public static readonly DependencyProperty DataSourceProperty =
			DataSourcePropertyKey.DependencyProperty;

		private void UpdateDataSource()
		{
			this.SetValue(DataSourcePropertyKey, this.board.ToArray());
		}

		public ICommand MoveCellCommand
		{
			get { return (ICommand)GetValue(MoveCellCommandProperty); }
		}

		static readonly DependencyPropertyKey MoveCellCommandPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"MoveCellCommand",
				typeof(ICommand),
				typeof(BoardPresenter),
				new PropertyMetadata());

		public static readonly DependencyProperty MoveCellCommandProperty =
			MoveCellCommandPropertyKey.DependencyProperty;

		private void SetCellCommand(ICommand value)
		{
			SetValue(MoveCellCommandPropertyKey, value);
		}

		private bool canMoveCell(object obj)
		{
			return board.CanMove((int)obj, board.EmptyCellIndex);
		}

		private void moveCell(object obj)
		{
			if (!board.TryMove((int)obj, board.EmptyCellIndex))
				throw new InvalidOperationException("should check whether can move first");
			UpdateDataSource();

			if (board.IsSolved)
			{
				MessageBox.Show(
					Window.GetWindow(this),
					string.Format("You solved it in {0} steps", board.SolveStepsInfo.TotalStepCount),
					"Congratulations!");
				this.board.Reset();
				UpdateDataSource();
			}
		}
	}
}
