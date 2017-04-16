using System;
using System.Collections.Generic;
using Match3.Cell;
using Match3.Element;

namespace Match3.Field
{
	public class ElementsManager : IElementsManagerExchange, IElementsManagerCells
	{
		/// <summary>
		/// Количество элементов находящихся в движении.
		/// </summary>
		public int WaitFinishMovementElements { get; private set; }

		/// <summary>
		/// Срабатывает когда закончилась обработка падения элементов.
		/// </summary>
		public event EventHandler FinishDropElementsEvent;

		private readonly IFieldModelElementsManager _field;
		private readonly ISettingsElements _settings;
		private readonly ExchangeElements _exchange;

		public ElementsManager(IFieldModelElementsManager field, ISettingsElements settings)
		{
			if (field == null)
				throw new ArgumentNullException("field");
			_field = field;
			_settings = settings;
			_exchange = new ExchangeElements(this);
		}
		/// <summary>
		/// Создать модель элемент.
		/// </summary>
		/// <param name="type">Тип элемента</param>
		/// <returns>Модель элемента</returns>
		public ElementModel CreateElement(ElementType type)
		{
			var element = new ElementModel(type, _settings);
			element.MovementFinishEvent += Element_OnMovementFinishEvent;
			return element;
		}
		/// <summary>
		/// Поменять местами элементы.
		/// </summary>
		/// <param name="cell">Ячейка с элементом для замены</param>
		/// <param name="direction">Направление замены</param>
		public void SwapElements(CellModel cell, MovementDirection direction)
		{
			if (WaitFinishMovementElements > 0)
				return;
			ElementModel firstElement;
			ElementModel secondElement;
			switch (direction)
			{
				case MovementDirection.Up:
					if (!_exchange.CheckSwap(MovementDirection.Up))
						return;
					if (cell.Y + 1 >= _field.Height)
						return;
					firstElement = cell.Element;
					secondElement = _field.GetCell(cell.X, cell.Y + 1).Element;
					cell.SetElement(secondElement);
					_field.GetCell(cell.X, cell.Y + 1).SetElement(firstElement);
					break;
				case MovementDirection.Right:
					if (!_exchange.CheckSwap(MovementDirection.Right))
						return;
					if (cell.X + 1 >= _field.Width)
						return;
					firstElement = cell.Element;
					secondElement = _field.GetCell(cell.X + 1, cell.Y).Element;
					cell.SetElement(secondElement);
					_field.GetCell(cell.X + 1, cell.Y).SetElement(firstElement);
					break;
				case MovementDirection.Down:
					if (!_exchange.CheckSwap(MovementDirection.Down))
						return;
					if (cell.Y - 1 < 0)
						return;
					firstElement = cell.Element;
					secondElement = _field.GetCell(cell.X, cell.Y - 1).Element;
					cell.SetElement(secondElement);
					_field.GetCell(cell.X, cell.Y - 1).SetElement(firstElement);
					break;
				case MovementDirection.Left:
					if (!_exchange.CheckSwap(MovementDirection.Left))
						return;
					if (cell.X - 1 < 0)
						return;
					firstElement = cell.Element;
					secondElement = _field.GetCell(cell.X - 1, cell.Y).Element;
					cell.SetElement(secondElement);
					_field.GetCell(cell.X - 1, cell.Y).SetElement(firstElement);
					break;
				default:
					throw new ArgumentOutOfRangeException("direction", direction, null);
			}
			if (firstElement != null && secondElement != null)
			{
				WaitFinishMovementElements = 2;
				firstElement.Swap(direction);
				secondElement.Swap(direction.InvertDirection());
			}
		}

		/// <summary>
		/// Уничтожить элемент.
		/// </summary>
		/// <param name="cellModel">Ячейка в которых находится элемент</param>
		private void DestroyElement(CellModel cellModel)
		{
			if (cellModel.Element == null)
				return;
			cellModel.Element.Destroy();
			cellModel.SetElement(null);
		}
		/// <summary>
		/// Уничтожить на поле все элементы с флагом "Должеть быть уничтожен".
		/// </summary>
		private void DestroyElements()
		{
			for (int x = 0; x < _field.Width; x++)
				for (int y = 0; y < _field.Height; y++)
				{
					var cell = _field.GetCell(x, y);
					if (cell == null || !cell.Element.MustByDestroyed)
						continue;
					DestroyElement(cell);
				}
		}
		/// <summary>
		/// Опустить все элементы под которыми нету элементов.
		/// </summary>
		/// <returns>Хотя бы один элемент был опущен</returns>
		private void DropElements()
		{
			for (int x = 0; x < _field.Width; x++)
			{
				var countDrop = 0;
				for (int y = 0; y < _field.Height; y++)
				{
					var cell = _field.GetCell(x, y);
					if (cell == null)
						continue;
					if (cell.Element == null)
					{
						countDrop++;
					}
					else if (countDrop > 0)
					{
						cell.Element.Drop(countDrop);
						_field.GetCell(x, y - countDrop).SetElement(cell.Element);
						cell.SetElement(null);
						WaitFinishMovementElements++;
					}
				}
				WaitFinishMovementElements += countDrop;
			}
			if (FinishDropElementsEvent != null)
				FinishDropElementsEvent(this, EventArgs.Empty);
		}
		/// <summary>
		/// Пометить элементы доступные для уничтожения.
		/// </summary>
		/// <param name="cell">Ячейка вокруг которой будут проверятся элементы</param>
		/// <returns>Список элементов с пометкой "для уничтожения"</returns>
		private void CheckElement(CellModel cell)
		{
			if (cell == null)
				throw new ArgumentNullException("cell");
			var listDestroying = GetElementDestroying(cell);
			foreach (CellModel cellModel in listDestroying)
				cellModel.Element.MustByDestroyed = true;
		}
		/// <summary>
		/// Для эвента элемента - срабатывает когда движение элемента закончилось.
		/// </summary>
		private void Element_OnMovementFinishEvent(object sender, EventArgs args)
		{
			WaitFinishMovementElements--;
			var element = sender as ElementModel;
			if (element == null)
				throw new ArgumentNullException("sender");
			if (element.Cell == null)
				throw new NullReferenceException("element.Cell");
			CheckElement(element.Cell);
			if (WaitFinishMovementElements <= 0)
			{
				WaitFinishMovementElements = 0;
				DestroyElements();
				DropElements();
			}
		}
		/// <summary>
		/// Получить элементы на уничтожение.
		/// </summary>
		/// <param name="cellModel">Ячейка для проверки</param>
		/// <returns>Список ячеек для уничтожения</returns>
		private List<CellModel> GetElementDestroying(CellModel cellModel)
		{
			var res = new List<CellModel>();
			var element = cellModel.Element;
			#region Горизонталь

			var destroy = false;

			if (cellModel.X - 1 >= 0 &&
				cellModel.X + 1 < _field.Width &&
				_field.GetCell(cellModel.X - 1, cellModel.Y) != null &&
				_field.GetCell(cellModel.X + 1, cellModel.Y) != null &&
				_field.GetCell(cellModel.X - 1, cellModel.Y).Element.Type == element.Type &&
				_field.GetCell(cellModel.X + 1, cellModel.Y).Element.Type == element.Type)
			{
				destroy = true;
			}
			else if (cellModel.X - 2 >= 0 &&
				_field.GetCell(cellModel.X - 1, cellModel.Y) != null &&
				_field.GetCell(cellModel.X - 2, cellModel.Y) != null &&
				_field.GetCell(cellModel.X - 1, cellModel.Y).Element.Type == element.Type &&
				_field.GetCell(cellModel.X - 2, cellModel.Y).Element.Type == element.Type)
			{
				destroy = true;
			}
			else if (cellModel.X + 2 < _field.Width &&
				_field.GetCell(cellModel.X + 1, cellModel.Y) != null &&
				_field.GetCell(cellModel.X + 2, cellModel.Y) != null &&
				_field.GetCell(cellModel.X + 1, cellModel.Y).Element.Type == element.Type &&
				_field.GetCell(cellModel.X + 2, cellModel.Y).Element.Type == element.Type)
			{
				destroy = true;
			}

			if (destroy)
			{
				// Вправо
				for (int x = cellModel.X + 1; x < _field.Width; x++)
				{
					var c = _field.GetCell(x, cellModel.Y);
					if (c != null && c.Element.Type == element.Type)
						res.Add(c);
					else
						break;
				}
				// Влево
				for (int x = cellModel.X - 1; x >= 0; x--)
				{
					var c = _field.GetCell(x, cellModel.Y);
					if (c != null && c.Element.Type == element.Type)
						res.Add(c);
					else
						break;
				}
			}

			#endregion Горизонталь
			#region Вертикаль

			destroy = false;

			if (cellModel.Y - 1 >= 0 &&
				cellModel.Y + 1 < _field.Height &&
				_field.GetCell(cellModel.X, cellModel.Y - 1) != null &&
				_field.GetCell(cellModel.X, cellModel.Y + 1) != null &&
				_field.GetCell(cellModel.X, cellModel.Y - 1).Element.Type == element.Type &&
				_field.GetCell(cellModel.X, cellModel.Y + 1).Element.Type == element.Type)
			{
				destroy = true;
			}
			else if (cellModel.Y - 2 >= 0 &&
				_field.GetCell(cellModel.X, cellModel.Y - 1) != null &&
				_field.GetCell(cellModel.X, cellModel.Y - 2) != null &&
				_field.GetCell(cellModel.X, cellModel.Y - 1).Element.Type == element.Type &&
				_field.GetCell(cellModel.X, cellModel.Y - 2).Element.Type == element.Type)
			{
				destroy = true;
			}
			else if (cellModel.Y + 2 < _field.Height &&
				_field.GetCell(cellModel.X, cellModel.Y + 1) != null &&
				_field.GetCell(cellModel.X, cellModel.Y + 2) != null &&
				_field.GetCell(cellModel.X, cellModel.Y + 1).Element.Type == element.Type &&
				_field.GetCell(cellModel.X, cellModel.Y + 2).Element.Type == element.Type)
			{
				destroy = true;
			}

			if (destroy)
			{
				// Вверх
				for (int y = cellModel.Y + 1; y < _field.Height; y++)
				{
					var c = _field.GetCell(cellModel.X, y);
					if (c != null && c.Element.Type == element.Type)
						res.Add(c);
					else
						break;
				}
				// Вниз
				for (int y = cellModel.Y - 1; y >= 0; y--)
				{
					var c = _field.GetCell(cellModel.X, y);
					if (c != null && c.Element.Type == element.Type)
						res.Add(c);
					else
						break;
				}
			}

			#endregion Вертикаль
			if (res.Count > 0)
				res.Add(cellModel);
			return res;
		}

		#region IElementsManagerExchange

		public int Width
		{
			get
			{
				return _field.Width;
			}
		}

		public int Height
		{
			get
			{
				return _field.Height;
			}
		}

		public CellModel SelectedCell
		{
			get
			{
				return _field.SelectedCell;
			}
		}

		public CellModel GetCell(int x, int y)
		{
			return _field.GetCell(x, y);
		}

		#endregion IElementsManagerExchange
	}
}
