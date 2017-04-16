using System;
using Match3.Element;

namespace Match3.Cell
{
	public class CellModel : ICellModelView, ICellModelController
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public ElementModel Element { get; private set; }
		public event EventHandler SelectedEvent;

		public void SetData(int x, int y)
		{
			X = x;
			Y = y;
		}
		/// <summary>
		/// Установить элемент.
		/// </summary>
		/// <param name="element">Элемент</param>
		public void SetElement(ElementModel element)
		{
			Element = element;
			if (Element != null)
				Element.Cell = this;
		}
		/// <summary>
		/// Отменить выбор ячейки.
		/// </summary>
		public void Unselected()
		{
			if (UnclampedEvent != null)
				UnclampedEvent(this, EventArgs.Empty);
		}

		public override string ToString()
		{
			return string.Format("Coord=[{0}:{1}], Elem:{2}", X, Y, Element) ;
		}

		#region ICellModelView

		/// <summary>
		/// Срабатывает когда ячейка зажата.
		/// </summary>
		public event EventHandler ClampedEvent;
		/// <summary>
		/// Срабатывает когда ячейка отжата.
		/// </summary>
		public event EventHandler UnclampedEvent;

		#endregion ICellModelView
		#region ICellModelController

		/// <summary>
		/// Ячейка зажата
		/// </summary>
		public void Clamp()
		{
			if (ClampedEvent != null)
				ClampedEvent(this, EventArgs.Empty);
		}
		/// <summary>
		/// Выбрать ячейку
		/// </summary>
		public void Selected()
		{
			if (SelectedEvent != null)
				SelectedEvent(this, EventArgs.Empty);
		}

		#endregion ICellModelController
	}
}
