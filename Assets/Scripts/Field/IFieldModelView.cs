using System;

namespace Match3.Field
{
	public interface IFieldModelView
	{
		event EventHandler<SettedDataArgs> SettedDataEvent;
		event EventHandler<FillEmptyCellsArgs> FillCellsEvent;
	}
}
