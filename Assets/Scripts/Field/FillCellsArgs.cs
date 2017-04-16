using System;
using System.Collections.Generic;
using Match3.Cell;

namespace Match3.Field
{
	public class FillEmptyCellsArgs : EventArgs
	{
		public List<CellModel> Cells { get; set; }
		public int Height { get; set; }
		public bool ShowDrop { get; set; }

		public FillEmptyCellsArgs()
		{

		}

		public FillEmptyCellsArgs(List<CellModel> cells, int heightField, bool showDrop)
		{
			Cells = cells;
			Height = heightField;
			ShowDrop = showDrop;
		}
	}
}
