using System;

namespace Match3.Element
{
	public class DropArgs : EventArgs
	{
		/// <summary>
		/// Количество единиц падения.
		/// </summary>
		public int CountDrop { get; set; }
		/// <summary>
		/// Скорость падения.
		/// </summary>
		public int Speed { get; set; }

		public DropArgs()
		{

		}

		public DropArgs(int countDrop, int speed)
		{
			CountDrop = countDrop;
			Speed = speed;
		}
	}
}
