using System;
using Match3.Cell;
using Match3.Field;

namespace Match3.Element
{
	public class ElementModel : IElementModelView
	{
		/// <summary>
		/// Тип элемента.
		/// </summary>
		public ElementType Type { get; private set; }
		/// <summary>
		/// Ячейка в которой находится элемент.
		/// </summary>
		public CellModel Cell { get; set; }
		/// <summary>
		/// Элемент должен быть уничтожен.
		/// </summary>
		public bool MustByDestroyed { get; set; }

		/// <summary>
		/// Срабатывает когда элемент заканчивает движение.
		/// </summary>
		public event EventHandler MovementFinishEvent;

		private readonly ISettingsElements _settings;

		public ElementModel(ElementType type, ISettingsElements settings)
		{
			Type = type;
			_settings = settings;
		}
		/// <summary>
		/// Поменять элемент местами с сосденим элементом.
		/// </summary>
		/// <param name="direction">Направление замены</param>
		public void Swap(MovementDirection direction)
		{
			if (SwapEvent != null)
				SwapEvent(this, new SwapArgs(direction, _settings.SwapElementsSpeed));
		}
		/// <summary>
		/// Опустить элемент на указанное количество единиц.
		/// </summary>
		/// <param name="count">Количество единиц опускания</param>
		public void Drop(int count)
		{
			if (DropEvent != null)
				DropEvent(this, new DropArgs(count, _settings.DropElementsSpeed));
		}
		/// <summary>
		/// Уничтожить элемент.
		/// </summary>
		public void Destroy()
		{
			if (DestroyEvent != null)
				DestroyEvent(this, EventArgs.Empty);
		}

		public override string ToString()
		{
			return string.Format("{0} [{1},{2}]", Type, Cell.X, Cell.Y);
		}

		#region IElementModelView

		/// <summary>
		/// Срабатывает когда элемент уничтожен.
		/// </summary>
		public event EventHandler DestroyEvent;
		/// <summary>
		/// Срабатывает когда элемент опускается.
		/// </summary>
		public event EventHandler<DropArgs> DropEvent;
		/// <summary>
		/// Срабатывает когда элемент меняется местами с соседним элементом.
		/// </summary>
		public event EventHandler<SwapArgs> SwapEvent;

		/// <summary>
		/// Движение элемента завершено.
		/// </summary>
		public void MovementFinish(object sender, EventArgs args)
		{
			if (MovementFinishEvent != null)
				MovementFinishEvent(this, EventArgs.Empty);
		}

		#endregion IElementModelView
	}
}
