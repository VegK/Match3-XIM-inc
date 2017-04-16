using System;

namespace Match3.Field
{
	public static class MovementDirectionExtension
	{
		/// <summary>
		/// Инвертировать направление движения.
		/// </summary>
		/// <param name="direction">Направление движения для инверсии</param>
		/// <returns>Инвертированное направление</returns>
		public static MovementDirection InvertDirection(this MovementDirection direction)
		{
			switch (direction)
			{
				case MovementDirection.Up:
					return MovementDirection.Down;
				case MovementDirection.Right:
					return MovementDirection.Left;
				case MovementDirection.Down:
					return MovementDirection.Up;
				case MovementDirection.Left:
					return MovementDirection.Right;
				default:
					throw new ArgumentOutOfRangeException("direction", direction, null);
			}
		}
	}
}
