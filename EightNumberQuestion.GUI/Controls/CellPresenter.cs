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
	sealed class CellPresenter : Control
	{
		public CellPresenter()
		{

		}

		public int Value
		{
			get { return (int)this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(
				"Value",
				typeof(int),
				typeof(CellPresenter));

		public int Index
		{
			get { return (int)this.GetValue(IndexProperty); }
			set { this.SetValue(IndexProperty, value); }
		}

		public static readonly DependencyProperty IndexProperty =
			DependencyProperty.Register(
				"Index",
				typeof(int),
				typeof(CellPresenter));

		public ICommand Command
		{
			get { return (ICommand)this.GetValue(CommandProperty); }
			set { this.SetValue(CommandProperty, value); }
		}

		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register(
				"Command",
				typeof(ICommand),
				typeof(CellPresenter));

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);

			if (Command != null)
				Dispatcher.BeginInvoke((Action)delegate
				{
					if (Command.CanExecute(Index))
						Command.Execute(Index);
				});
		}
	}
}
