using System;
using Match3.Cell;

namespace Match3.Field
{
	public class FieldModel : IFieldModelView, IFieldModelCellsManager,
		IFieldModelElementsManager
	{
		/// <summary>
		/// Ширина поля.
		/// </summary>
		public int Width { get; private set; }
		/// <summary>
		/// Высота поля.
		/// </summary>
		public int Height { get; private set; }
		/// <summary>
		/// Выбранная ячейка поля.
		/// </summary>
		public CellModel SelectedCell {
			get
			{
				return _cellsManager.SelectedCell;
			}
		}

		private readonly CellsManager _cellsManager;
		private readonly ElementsManager _elementsManager;

		public FieldModel(int width, int height, ISettings settings)
		{
			Width = width;
			Height = height;

			_elementsManager = new ElementsManager(this, settings);
			_cellsManager = new CellsManager(this, _elementsManager);

			_elementsManager.FinishDropElementsEvent += ElementsManager_OnFinishDropElementsEvent;
			_cellsManager.FillCellsEvent += CellsManager_OnFillCellsEvent;
			_cellsManager.FillEmptyCellsWithoutMatch();
			
			if (_settedDataEvent != null)
				_settedDataEvent(this, new SettedDataArgs(_cellsManager.GetCells()));
		}

		private void CellsManager_OnFillCellsEvent(object sender, FillEmptyCellsArgs args)
		{
			if (_fillCellsEvent != null)
				_fillCellsEvent(this, args);
		}

		private void ElementsManager_OnFinishDropElementsEvent(object sender, EventArgs args)
		{
			_cellsManager.FillEmptyCells();
		}

		/// <summary>
		/// Получить ячейку поля.
		/// </summary>
		/// <param name="x">Координата по X</param>
		/// <param name="y">Координата по Y</param>
		/// <returns>Модель ячейки поля</returns>
		public CellModel GetCell(int x, int y)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				throw new IndexOutOfRangeException(string.Format("_cell[{0}:{1}]", x, y));
			return _cellsManager.GetCell(x, y);
		}
		/// <summary>
		/// Попробывать поменять выбранный элемент с правым элементом.
		/// </summary>
		public void SwapRight()
		{
			_elementsManager.SwapElements(SelectedCell, MovementDirection.Right);
		}
		/// <summary>
		/// Попробывать поменять выбранный элемент с левым элементом.
		/// </summary>
		public void SwapLeft()
		{
			_elementsManager.SwapElements(SelectedCell, MovementDirection.Left);
		}
		/// <summary>
		/// Попробывать поменять выбранный элемент с верхним элементом.
		/// </summary>
		public void SwapUp()
		{
			_elementsManager.SwapElements(SelectedCell, MovementDirection.Up);
		}
		/// <summary>
		/// Попробывать поменять выбранный элемент с нижним элементом.
		/// </summary>
		public void SwapDown()
		{
			_elementsManager.SwapElements(SelectedCell, MovementDirection.Down);
		}
		/// <summary>
		/// Выбор завершён.
		/// </summary>
		public void SwapFinish()
		{
			_cellsManager.UnselectedCell();
		}

		#region IFieldModelView

		private event EventHandler<SettedDataArgs> _settedDataEvent;
		private event EventHandler<FillEmptyCellsArgs> _fillCellsEvent;

		public event EventHandler<SettedDataArgs> SettedDataEvent
		{
			add
			{
				_settedDataEvent += value;
				if (_settedDataEvent != null)
					_settedDataEvent(this, new SettedDataArgs(_cellsManager.GetCells()));
			}
			remove
			{
				_settedDataEvent -= value;
			}
		}
		public event EventHandler<FillEmptyCellsArgs> FillCellsEvent
		{
			add
			{
				_fillCellsEvent += value;
				if (_fillCellsEvent != null)
					_fillCellsEvent(this, new FillEmptyCellsArgs(_cellsManager.GetCells(), Height,
						false));
			}
			remove
			{
				_fillCellsEvent -= value;
			}
		}

		#endregion IFieldModelView
	}
}
