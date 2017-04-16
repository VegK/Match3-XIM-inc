using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Cell;
using Match3.Element;
using Random = UnityEngine.Random;

namespace Match3.Field
{
	public class CellsManager
	{
		/// <summary>
		/// Выбранная ячейка.
		/// </summary>
		public CellModel SelectedCell { get; private set; }

		/// <summary>
		/// Срабатывает когда ячейки заполнены.
		/// </summary>
		public event EventHandler<FillEmptyCellsArgs> FillCellsEvent;

		private readonly IFieldModelCellsManager _field;
		private readonly IElementsManagerCells _elementsManager;
		private readonly CellModel[,] _cells;

		public CellsManager(IFieldModelCellsManager field, IElementsManagerCells elementsManager)
		{
			if (field == null)
				throw new ArgumentNullException("field");
			_field = field;
			_elementsManager = elementsManager;
			_cells = new CellModel[_field.Width, _field.Height];
			for (int x = 0; x < _cells.GetLength(0); x++)
				for (int y = 0; y < _cells.GetLength(1); y++)
					_cells[x, y] = CreateCell(x, y);

		}

		/// <summary>
		/// Отменить выбор ячейки.
		/// </summary>
		public void UnselectedCell()
		{
			if (SelectedCell == null)
				return;
			SelectedCell.Unselected();
			SelectedCell = null;
		}
		/// <summary>
		/// Заполнить пустые ячейки поля случайными элементами.
		/// </summary>
		/// <returns>Список ячеек с новыми элементами</returns>
		public List<CellModel> FillEmptyCells()
		{
			var elementTypes = Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToList();
			var cellsWithNewElements = new List<CellModel>();
			for (int x = 0; x < _cells.GetLength(0); x++)
			{
				// Если верхняя ячейка столбца содержит элемент, то пропускаем столбец
				if (_cells[x, _cells.GetLength(1) - 1].Element != null)
					continue;
				for (int y = 0; y < _cells.GetLength(1); y++)
				{
					var cell = _cells[x, y];
					if (cell.Element != null)
						continue;
					var type = elementTypes[Random.Range(0, elementTypes.Count)];
					cell.SetData(x, y);
					cell.SetElement(_elementsManager.CreateElement(type));
					cellsWithNewElements.Add(cell);
				}
			}
			if (FillCellsEvent != null && cellsWithNewElements.Count > 0)
				FillCellsEvent(this, new FillEmptyCellsArgs(cellsWithNewElements, _field.Height, true));
			return cellsWithNewElements;
		}
		/// <summary>
		/// Заполнить пустые ячейки поля случайными элементами без соответствий.
		/// </summary>
		public void FillEmptyCellsWithoutMatch()
		{
			var elementTypesDefault = Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToList();
			var elementTypes = new List<ElementType>(elementTypesDefault);
			for (int x = 0; x < _cells.GetLength(0); x++)
				for (int y = 0; y < _cells.GetLength(1); y++)
				{
					var cell = _cells[x, y];
					if (x > 1 && _cells[x - 1, y].Element.Type == _cells[x - 2, y].Element.Type)
						elementTypes.Remove(_cells[x - 1, y].Element.Type);
					if (y > 1 && _cells[x, y - 1].Element.Type == _cells[x, y - 2].Element.Type)
						elementTypes.Remove(_cells[x, y - 1].Element.Type);
					var type = elementTypes[Random.Range(0, elementTypes.Count)];
					cell.SetData(x, y);
					cell.SetElement(_elementsManager.CreateElement(type));
					elementTypes = new List<ElementType>(elementTypesDefault);
				}
			if (FillCellsEvent != null)
				FillCellsEvent(this, new FillEmptyCellsArgs(GetCells(), _field.Height, false));
		}
		/// <summary>
		/// Получить все ячейки списком.
		/// </summary>
		/// <returns>Список ячеек</returns>
		public List<CellModel> GetCells()
		{
			return _cells.Cast<CellModel>()
			             .ToList();
		}
		/// <summary>
		/// Получить ячейку по координатам поля.
		/// </summary>
		/// <param name="x">Координата X поля</param>
		/// <param name="y">Координата Y поля</param>
		/// <returns>Ячейка поля</returns>
		public CellModel GetCell(int x, int y)
		{
			if (x < 0 || x >= _cells.GetLength(0) || y < 0 || y >= _cells.GetLength(1))
				throw new IndexOutOfRangeException(string.Format("_cell[{0}:{1}]", x, y));
			return _cells[x, y];
		}

		/// <summary>
		/// Создать ячейку
		/// </summary>
		/// <param name="x">Координата X на поле</param>
		/// <param name="y">Координата Y на поле</param>
		/// <returns>Созданная ячейка</returns>
		private CellModel CreateCell(int x, int y)
		{
			var cell = new CellModel();
			cell.SetData(x, y);
			cell.ClampedEvent += Cell_OnClampedEvent;
			cell.SelectedEvent += Cell_OnSelectedEvent;
			return cell;
		}
		/// <summary>
		/// Для эвента ячейки - срабатывает когда ячейка нажата.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void Cell_OnClampedEvent(object sender, EventArgs args)
		{
			if (SelectedCell == null)
				SelectedCell = sender as CellModel;
		}
		/// <summary>
		/// Для эвента ячейки - срабатывает когда ячейка выбрана.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void Cell_OnSelectedEvent(object sender, EventArgs args)
		{
			if (SelectedCell == null)
				return;
			var cell = sender as CellModel;
			if (cell == null)
				return;
			if (cell == SelectedCell)
				return;
			var diffX = cell.X - SelectedCell.X;
			var diffY = cell.Y - SelectedCell.Y;
			if (diffX == 1 && diffY == 0)
				_field.SwapRight();
			else if (diffX == -1 && diffY == 0)
				_field.SwapLeft();
			else if (diffY == 1 && diffX == 0)
				_field.SwapUp();
			else if (diffY == -1 && diffX == 0)
				_field.SwapDown();
			cell.Unselected();
			_field.SwapFinish();
		}
	}
}
