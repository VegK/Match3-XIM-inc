using System;

namespace Match3.Element
{
	public interface IElementModelView
	{
		/// <summary>
		/// Срабатывает когда элемент уничтожен.
		/// </summary>
		event EventHandler DestroyEvent;
		/// <summary>
		/// Срабатывает когда элемент опускается.
		/// </summary>
		event EventHandler<DropArgs> DropEvent;
		/// <summary>
		/// Срабатывает когда элемент меняется местами с соседним элементом.
		/// </summary>
		event EventHandler<SwapArgs> SwapEvent;
		/// <summary>
		/// Движение элемента завершено.
		/// </summary>
		void MovementFinish(object sender, EventArgs args);
	}
}
