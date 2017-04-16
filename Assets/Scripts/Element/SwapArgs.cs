using System;
using Match3.Field;

namespace Match3.Element
{
	public class SwapArgs : EventArgs
	{
		/// <summary>
		/// Направление замены.
		/// </summary>
		public MovementDirection Direction { get; set; }
		/// <summary>
		/// Скорость замены.
		/// </summary>
		public int Speed { get; set; }

		public SwapArgs()
		{

		}

		public SwapArgs(MovementDirection direction, int speed)
		{
			Direction = direction;
			Speed = speed;
		}
	}
}
