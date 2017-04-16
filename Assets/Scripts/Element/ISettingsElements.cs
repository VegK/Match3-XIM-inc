namespace Match3.Element
{
	/// <summary>
	/// Интерфейс настройки элементов.
	/// </summary>
	public interface ISettingsElements
	{
		/// <summary>
		/// Скорость обмена элементов местами.
		/// </summary>
		int SwapElementsSpeed { get; }
		/// <summary>
		/// Скорость падения элементов.
		/// </summary>
		int DropElementsSpeed { get; }
	}
}
