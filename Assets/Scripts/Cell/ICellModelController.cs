namespace Match3.Cell
{
	public interface ICellModelController
	{
		/// <summary>
		/// Зажать ячейку
		/// </summary>
		void Clamp();
		/// <summary>
		/// Выбрать ячейку
		/// </summary>
		void Selected();
	}
}
