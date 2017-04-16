using System;
using UnityEngine;

namespace Match3.Cell
{
	public class CellController : MonoBehaviour
	{
		private ICellModelController _model;

		/// <summary>
		/// Задать модель ячейки.
		/// </summary>
		/// <param name="model">Модель ячейки</param>
		public void SetModel(ICellModelController model)
		{
			if (model == null)
				throw new ArgumentNullException("model");

			enabled = true;
			_model = model;
		}

		#region MonoBehaviour

		// ReSharper disable once UnusedMember.Local
		private void Awake()
		{
			if (_model == null)
				enabled = false;
		}

		// ReSharper disable once UnusedMember.Local
		private void OnMouseDown()
		{
			if (Input.GetMouseButtonDown(0))
				if (_model != null)
					_model.Clamp();
		}

		// ReSharper disable once UnusedMember.Local
		private void OnMouseUp()
		{
			if (Input.GetMouseButtonUp(0))
				if (_model != null)
					_model.Selected();
		}

		#endregion MonoBehaviour
	}
}
