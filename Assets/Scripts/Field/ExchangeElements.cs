using System;
using Match3.Cell;

namespace Match3.Field
{
	public class ExchangeElements
	{
		private readonly IElementsManagerExchange _field;

		public ExchangeElements(IElementsManagerExchange field)
		{
			_field = field;
		}
		/// <summary>
		/// Проверить возможность сменить выбранный элемент с элементом выбранного направления.
		/// </summary>
		/// <param name="direction">Направление проверки</param>
		/// <returns>Смена возможна</returns>
		public bool CheckSwap(MovementDirection direction)
		{
			if (_field.SelectedCell == null || _field.SelectedCell.Element == null)
				return false;

			CellModel swipeElement = null;
			switch (direction)
			{
				case MovementDirection.Up:
					if (_field.SelectedCell.Y + 1 < _field.Height)
						swipeElement = _field.GetCell(_field.SelectedCell.X, _field.SelectedCell.Y + 1);
					break;
				case MovementDirection.Right:
					if (_field.SelectedCell.X + 1 < _field.Width)
						swipeElement = _field.GetCell(_field.SelectedCell.X + 1, _field.SelectedCell.Y);
					break;
				case MovementDirection.Down:
					if (_field.SelectedCell.Y - 1 >= 0)
						swipeElement = _field.GetCell(_field.SelectedCell.X, _field.SelectedCell.Y - 1);
					break;
				case MovementDirection.Left:
					if (_field.SelectedCell.X - 1 >= 0)
						swipeElement = _field.GetCell(_field.SelectedCell.X - 1, _field.SelectedCell.Y);
					break;
				default:
					throw new ArgumentOutOfRangeException("direction", direction, null);
			}
			if (swipeElement == null || swipeElement.Element == null)
				return false;

			if (!AvailableSwap(_field.SelectedCell.X, _field.SelectedCell.Y,
					swipeElement.Element.Type, direction) &&
				!AvailableSwap(swipeElement.X, swipeElement.Y,
					_field.SelectedCell.Element.Type, direction.InvertDirection()))
				return false;

			return true;
		}
		/// <summary>
		/// Доступность смены элемента
		/// </summary>
		/// <param name="x">Координа X элемента</param>
		/// <param name="y">Координа Y элемента</param>
		/// <param name="type">Тип элемента для проверки</param>
		/// <param name="direction">Направление проверки</param>
		/// <returns>Элемент можно поменять</returns>
		private bool AvailableSwap(int x, int y, ElementType type, MovementDirection direction)
		{
			if (x < 0 || x >= _field.Width || y < 0 || y >= _field.Height)
				return false;

			#region Проверка по горизонтале

			if (x - 2 >= 0)
			{
				var cellL1 = (direction == MovementDirection.Left) ? _field.GetCell(x, y) : _field.GetCell(x - 1, y);
				var cellL2 = _field.GetCell(x - 2, y);
				if (cellL1.Element != null && cellL1.Element.Type == type &&
					cellL2.Element != null && cellL2.Element.Type == type)
					return true;
			}

			if (x + 2 < _field.Width)
			{
				var cellR1 = (direction == MovementDirection.Right) ? _field.GetCell(x, y) : _field.GetCell(x + 1, y);
				var cellR2 = _field.GetCell(x + 2, y);
				if (cellR1.Element != null && cellR1.Element.Type == type &&
					cellR2.Element != null && cellR2.Element.Type == type)
					return true;
			}

			if (x - 1 >= 0 && x + 1 < _field.Width)
			{
				var cellL1 = (direction == MovementDirection.Left) ? _field.GetCell(x, y) : _field.GetCell(x - 1, y);
				var cellR1 = (direction == MovementDirection.Right) ? _field.GetCell(x, y) : _field.GetCell(x + 1, y);
				if (cellL1.Element != null && cellL1.Element.Type == type &&
					cellR1.Element != null && cellR1.Element.Type == type)
					return true;
			}

			#endregion Проверка по горизонтале
			#region Проверка по вертикале

			if (y + 2 < _field.Height)
			{
				var cellU1 = (direction == MovementDirection.Up) ? _field.GetCell(x, y) : _field.GetCell(x, y + 1);
				var cellU2 = _field.GetCell(x, y + 2);
				if (cellU1.Element != null && cellU1.Element.Type == type &&
					cellU2.Element != null && cellU2.Element.Type == type)
					return true;
			}

			if (y - 2 >= 0)
			{
				var cellD1 = (direction == MovementDirection.Down) ? _field.GetCell(x, y) : _field.GetCell(x, y - 1);
				var cellD2 = _field.GetCell(x, y - 2);
				if (cellD1.Element != null && cellD1.Element.Type == type &&
					cellD2.Element != null && cellD2.Element.Type == type)
					return true;
			}

			if (y - 1 >= 0 && y + 1 < _field.Height)
			{
				var cellD1 = (direction == MovementDirection.Down) ? _field.GetCell(x, y) : _field.GetCell(x, y - 1);
				var cellU1 = (direction == MovementDirection.Up) ? _field.GetCell(x, y) : _field.GetCell(x, y + 1);
				if (cellU1.Element != null && cellU1.Element.Type == type &&
					cellD1.Element != null && cellD1.Element.Type == type)
					return true;
			}

			#endregion Проверка по вертикале

			return false;
		}
	}
}
