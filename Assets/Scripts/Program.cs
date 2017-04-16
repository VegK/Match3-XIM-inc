using Match3.Field;
using UnityEngine;

namespace Match3
{
	public class Program : MonoBehaviour, ISettings
	{
#pragma warning disable 649
		[SerializeField] private int Width = 6;
		[SerializeField] private int Height = 8;
		[SerializeField] private int SpeedSwapElements = 3;
		[SerializeField] private int SpeedDropElements = 2;
		[Space(10)] //
		[SerializeField] private FieldView Field;
		[SerializeField] private Swipe MouseSwipe;
#pragma warning restore 649

		private void Init()
		{
			var fieldModel = new FieldModel(Width, Height, this);
			Field.SubscribeModel(fieldModel);

			MouseSwipe.SwipeRight += fieldModel.SwapRight;
			MouseSwipe.SwipeLeft += fieldModel.SwapLeft;
			MouseSwipe.SwipeUp += fieldModel.SwapUp;
			MouseSwipe.SwipeDown += fieldModel.SwapDown;
			MouseSwipe.SwipeFinish += fieldModel.SwapFinish;
		}

		#region MonoBehaviour

		// ReSharper disable once UnusedMember.Local
		private void Start()
		{
			Init();
		}

		// ReSharper disable once UnusedMember.Local
		private void OnDrawGizmos()
		{
			var boxColor = new Color(1, 0, 0, .5f);
			var size = Vector3.one;
			size.x -= 0.05f;
			size.y -= 0.05f;
			size.z = 0;

			for (int x = 0; x < Width; x++)
			{
				Gizmos.color = boxColor;
				for (int y = 0; y < Height; y++)
				{
					var pos = new Vector3(x + 0.5f, y + 0.5f);
					pos += Field.transform.position;
					if (Application.isPlaying)
						Gizmos.DrawWireCube(pos, size);
					else
						Gizmos.DrawCube(pos, size);
					GizmosUtils.DrawText(GUI.skin, "[" + x + ":" + y + "]",
						pos, Color.white, 10);
				}
			}

			// Рисуем общую рамку поля
			Gizmos.color = Color.yellow;
			var posField = Field.transform.position;
			posField.x += Width / 2f;
			posField.y += Height / 2f;
			Gizmos.DrawWireCube(posField, new Vector3(Width, Height));
		}

		#endregion MonoBehaviour
		#region ISettings

		/// <summary>
		/// Скорость обмена элементов местами.
		/// </summary>
		public int SwapElementsSpeed
		{
			get
			{
				return SpeedSwapElements;
			}
		}
		/// <summary>
		/// Скорость падения элементов.
		/// </summary>
		public int DropElementsSpeed
		{
			get
			{
				return SpeedDropElements;
			}
		}

		#endregion ISettings
	}
}