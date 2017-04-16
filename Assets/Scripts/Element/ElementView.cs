using System;
using System.Collections;
using Match3.Field;
using UnityEngine;

namespace Match3.Element
{
	public class ElementView : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private ElementType Type;
#pragma warning restore 649

		/// <summary>
		/// Тип элемента.
		/// </summary>
		public ElementType ElementType
		{
			get { return Type; }
		}

		/// <summary>
		/// Срабатывает когда элемент завершил движение.
		/// </summary>
		public event EventHandler MovementFinishEvent;

		private IElementModelView _model;

		/// <summary>
		/// Привязаться к модели.
		/// </summary>
		/// <param name="model">Модель элемента</param>
		public void SubscribeModel(IElementModelView model)
		{
			if (model == null)
				throw new ArgumentNullException("model");

			UnsubscriveModel();
			_model = model;

			_model.DestroyEvent += Model_OnDestroyEvent;
			_model.DropEvent += Model_OnDropEvent;
			_model.SwapEvent += Model_OnSwapEvent;
			MovementFinishEvent += _model.MovementFinish;
		}
		/// <summary>
		/// Отвязаться от модели.
		/// </summary>
		public void UnsubscriveModel()
		{
			if (_model == null)
				return;

			_model.DestroyEvent -= Model_OnDestroyEvent;
			_model.DropEvent -= Model_OnDropEvent;
			_model.SwapEvent -= Model_OnSwapEvent;
			MovementFinishEvent -= _model.MovementFinish;

			_model = null;
		}

		/// <summary>
		/// Корутин для движения элемента.
		/// </summary>
		/// <param name="countX">Количество единиц движения по горизонтали</param>
		/// <param name="countY">Количество единиц движения по вертикали</param>
		/// <param name="speed">Скорость движения элемента</param>
		private IEnumerator CMovement(int countX, int countY, int speed)
		{
			var startPosition = transform.localPosition;
			var finishPosition = new Vector2(
				startPosition.x + countX,
				startPosition.y + countY);
			var timer = 0f;
			while (timer < 1)
			{
				timer += Time.deltaTime * speed;
				var position = Vector2.Lerp(startPosition, finishPosition, timer);
				transform.localPosition = position;
				yield return null;
			}
			name = _model.ToString();
			if (MovementFinishEvent != null)
				MovementFinishEvent(this, EventArgs.Empty);
		}

		#region MonoBehaviour

		// ReSharper disable once UnusedMember.Local
		private void OnDestroy()
		{
			UnsubscriveModel();
		}

		#endregion MonoBehaviour
		#region События Model

		/// <summary>
		/// Для эвента модели - уничтожение элемента.
		/// </summary>
		private void Model_OnDestroyEvent(object sender, EventArgs args)
		{
			Destroy(gameObject);
		}
		/// <summary>
		/// Для эвента модели - падение элемента.
		/// </summary>
		private void Model_OnDropEvent(object sender, DropArgs args)
		{
			StartCoroutine(CMovement(0, -args.CountDrop, args.Speed));
		}
		/// <summary>
		/// Для эвента модели - обмен местами с соседним элементом.
		/// </summary>
		private void Model_OnSwapEvent(object sender, SwapArgs args)
		{
			int movementX = 0;
			int movementY = 0;
			switch (args.Direction)
			{
				case MovementDirection.Up:
					movementY = 1;
					break;
				case MovementDirection.Right:
					movementX = 1;
					break;
				case MovementDirection.Down:
					movementY = -1;
					break;
				case MovementDirection.Left:
					movementX = -1;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			StartCoroutine(CMovement(movementX, movementY, args.Speed));
		}

		#endregion События Model
	}
}