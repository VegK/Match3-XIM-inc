using System;

namespace Match3.Cell
{
	public interface ICellModelView
	{
		/// <summary>
		/// Срабатывает когда ячейка зажата.
		/// </summary>
		event EventHandler ClampedEvent;
		/// <summary>
		/// Срабатывает когда ячейка отжата.
		/// </summary>
		event EventHandler UnclampedEvent;
	}
}
