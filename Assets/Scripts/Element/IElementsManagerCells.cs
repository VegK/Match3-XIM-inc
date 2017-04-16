namespace Match3.Element
{
	public interface IElementsManagerCells
	{
		/// <summary>
		/// Создать элемент.
		/// </summary>
		/// <param name="type">Тип элемента</param>
		/// <returns>Элемент</returns>
		ElementModel CreateElement(ElementType type);
	}
}
