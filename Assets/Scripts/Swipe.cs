using UnityEngine;

namespace Match3
{
	public class Swipe : MonoBehaviour
	{
#pragma warning disable 649
		[SerializeField] private int MinSwipeDistX = 30;
		[SerializeField] private int MinSwipeDistY = 30;
#pragma warning restore 649

		public event SwipeHandler SwipeRight;
		public event SwipeHandler SwipeLeft;
		public event SwipeHandler SwipeUp;
		public event SwipeHandler SwipeDown;
		public event SwipeHandler SwipeFinish;

		private Vector2 _startPosition;

		// ReSharper disable once UnusedMember.Local
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				_startPosition = Input.mousePosition;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				var distX = Input.mousePosition.x - _startPosition.x;
				var distY = Input.mousePosition.y - _startPosition.y;
				var distXabs = Mathf.Abs(distX);
				var distYabs = Mathf.Abs(distY);
				var swipe = false;

				if (distXabs >= MinSwipeDistX && distXabs >= distYabs)
				{
					if (distX > 0)
					{
						swipe = true;
						if (SwipeRight != null)
							SwipeRight();
					}
					else if (distX < 0)
					{
						swipe = true;
						if (SwipeLeft != null)
							SwipeLeft();
					}
				}

				if (distYabs >= MinSwipeDistY && distYabs > distXabs)
				{
					if (distY > 0)
					{
						swipe = true;
						if (SwipeUp != null)
							SwipeUp();
					}
					else if (distY < 0)
					{
						swipe = true;
						if (SwipeDown != null)
							SwipeDown();
					}
				}

				if (swipe)
					if (SwipeFinish != null)
						SwipeFinish();
			}
		}

		public delegate void SwipeHandler();
	}
}