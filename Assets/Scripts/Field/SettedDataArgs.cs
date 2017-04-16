using System;
using System.Collections.Generic;
using Match3.Cell;

namespace Match3.Field
{
	public class SettedDataArgs : EventArgs
	{
		/// <summary>
		/// Список ячеек.
		/// </summary>
		public List<CellModel> Cells { get; set; }

		public SettedDataArgs()
		{

		}

		public SettedDataArgs(List<CellModel> cells)
		{
			Cells = cells;
		}
	}
}
