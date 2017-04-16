using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Cell;
using Match3.Element;
using UnityEngine;

namespace Match3.Field
{
	public class FieldView : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private CellView PrefabCell;
		[SerializeField] private ElementView[] PrefabElements;
		[Space(10)] //
		[SerializeField] private Transform ParentCells;
		[SerializeField] private Transform ParentElements;
#pragma warning restore 649

		private IFieldModelView _model;

		/// <summary>
		/// Привязаться к модели.
		/// </summary>
		/// <param name="model">Модель поля</param>
		public void SubscribeModel(IFieldModelView model)
		{
			if (model == null)
				throw new ArgumentNullException("model");

			UnsubscriveModel();
			_model = model;

			_model.SettedDataEvent += Model_OnSettedDataEvent;
			_model.FillCellsEvent += Model_OnFillCellsEvent;
		}
		/// <summary>
		/// Отвязаться от модели.
		/// </summary>
		public void UnsubscriveModel()
		{
			if (_model == null)
				return;

			_model.SettedDataEvent -= Model_OnSettedDataEvent;
			_model.FillCellsEvent -= Model_OnFillCellsEvent;

			_model = null;
		}

		/// <summary>
		/// Преобразовать точку в координаты на сцене.
		/// </summary>
		private Vector2 CoordToPosition(int x, int y)
		{
			var res = transform.position;
			res.x += x + 0.5f;
			res.y += y + 0.5f;
			return res;
		}
		/// <summary>
		/// Создать ячейки поля.
		/// </summary>
		/// <param name="cells">Список моделей ячеек для создания</param>
		private void CreateCells(List<CellModel> cells)
		{
			if (cells == null)
				throw new ArgumentNullException("cells");
			foreach (CellModel cellModel in cells)
			{
				var cell = Instantiate(PrefabCell);
				cell.SubscribeModel(cellModel);
				var cellController = cell.GetComponent<CellController>();
				if (cellController != null)
					cellController.SetModel(cellModel);
				cell.name = string.Format("Cell [{0}:{1}]", cellModel.X, cellModel.Y);
				cell.transform.SetParent(ParentCells);
				cell.transform.position = CoordToPosition(cellModel.X, cellModel.Y);
			}
		}
		/// <summary>
		/// Заполнить ячейки элементами.
		/// </summary>
		/// <param name="cells">Список ячеек поля</param>
		/// <param name="heightField">Высота поля</param>
		/// <param name="showDrop">Показать падение элементов</param>
		private void FillElements(List<CellModel> cells, int heightField, bool showDrop)
		{
			var firstPrefab = PrefabElements.FirstOrDefault();
			if (firstPrefab == null)
				throw new NullReferenceException("PrefabElements");

			var elementsInColumn = new Dictionary<int, int>();
			foreach (CellModel cell in cells)
			{
				if (cell.Element == null)
					throw new NullReferenceException("cell.Element");
				if (showDrop && !elementsInColumn.ContainsKey(cell.X))
					elementsInColumn.Add(cell.X, 0);
				var prefab = PrefabElements.FirstOrDefault(p => p.ElementType == cell.Element.Type)
				             ?? firstPrefab;
				var elementObject = Instantiate(prefab);
				elementObject.name = string.Format("{0} [{1}:{2}]",
					cell.Element.Type, cell.X, cell.Y);
				elementObject.SubscribeModel(cell.Element);
				elementObject.transform.SetParent(ParentElements);
				elementObject.transform.position = CoordToPosition(cell.X,
					showDrop ? heightField + elementsInColumn[cell.X] : cell.Y);
				var pos = elementObject.transform.localPosition;
				pos.z = 0;
				elementObject.transform.localPosition = pos;
				if (showDrop)
				{
					cell.Element.Drop(heightField + elementsInColumn[cell.X] - cell.Y);
					elementsInColumn[cell.X]++;
				}
			}
		}

		#region MonoBehaviour

		// ReSharper disable once UnusedMember.Local
		private void OnDestroy()
		{
			UnsubscriveModel();
		}

		#endregion MonoBehaviour
		#region События Model

		private void Model_OnSettedDataEvent(object sender, SettedDataArgs args)
		{
			CreateCells(args.Cells);
		}

		private void Model_OnFillCellsEvent(object sender, FillEmptyCellsArgs args)
		{
			FillElements(args.Cells, args.Height, args.ShowDrop);
		}

		#endregion События Model
	}
}