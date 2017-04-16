using Match3.Cell;

namespace Match3.Field
{
	public interface IElementsManagerExchange
	{
		int Width { get; }
		int Height { get; }
		CellModel SelectedCell { get; }
		/// <summary>
		/// Получить ячейку поля.
		/// </summary>
		/// <param name="x">Координата по X</param>
		/// <param name="y">Координата по Y</param>
		/// <returns>Модель ячейки поля</returns>
		CellModel GetCell(int x, int y);
	}
}
