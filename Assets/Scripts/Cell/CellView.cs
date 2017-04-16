using System;
using UnityEngine;

namespace Match3.Cell
{
	public class CellView : MonoBehaviour
	{
		private SpriteRenderer _spriteRenderer;
		private ICellModelView _model;
		private bool _clamped;

		/// <summary>
		/// Привязаться к модели.
		/// </summary>
		/// <param name="model">Модель ячейки</param>
		public void SubscribeModel(ICellModelView model)
		{
			if (model == null)
				throw new ArgumentNullException("model");

			UnsubscriveModel();
			_model = model;

			_model.ClampedEvent += Model_OnClampedEvent;
			_model.UnclampedEvent += Model_OnUnclampedEvent;
		}
		/// <summary>
		/// Отвязаться от модели.
		/// </summary>
		public void UnsubscriveModel()
		{
			if (_model == null)
				return;

			_model.ClampedEvent -= Model_OnClampedEvent;
			_model.UnclampedEvent -= Model_OnUnclampedEvent;

			_model = null;
		}

		#region MonoBehaviour

		// ReSharper disable once UnusedMember.Local
		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		#endregion MonoBehaviour
		#region События Model

		/// <summary>
		/// Для эвента модели - зажатие ячейки.
		/// </summary>
		private void Model_OnClampedEvent(object sender, EventArgs args)
		{
			if (_clamped)
			{
				if (_spriteRenderer != null)
					_spriteRenderer.color = Color.white;
				_clamped = false;
			}
			else
			{
				if (_spriteRenderer != null)
					_spriteRenderer.color = Color.green;
				_clamped = true;
			}
		}
		/// <summary>
		/// Для эвента модели - отжатие ячейки.
		/// </summary>
		private void Model_OnUnclampedEvent(object sender, EventArgs args)
		{
			if (_spriteRenderer != null)
				_spriteRenderer.color = Color.white;
			_clamped = false;
		}

		#endregion События Model
	}
}
