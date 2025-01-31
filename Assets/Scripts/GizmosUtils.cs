﻿using UnityEngine;

// ReSharper disable CheckNamespace

namespace Match3
{
	public static class GizmosUtils
	{
		// https://habrahabr.ru/post/247951/
		public static void DrawText(GUISkin guiSkin, string text, Vector3 position,
			Color? color = null, int fontSize = 0, float yOffset = 0)
		{
#if UNITY_EDITOR
			var prevSkin = GUI.skin;
			if (guiSkin == null)
				Debug.LogWarning("editor warning: guiSkin parameter is null");
			else
				GUI.skin = guiSkin;

			GUIContent textContent = new GUIContent(text);

			GUIStyle style = (guiSkin != null)
				? new GUIStyle(guiSkin.GetStyle("Label"))
				: new GUIStyle();
			if (color != null)
				style.normal.textColor = (Color) color;
			if (fontSize > 0)
				style.fontSize = fontSize;

			Vector2 textSize = style.CalcSize(textContent);
			Vector3 screenPoint = Camera.current.WorldToScreenPoint(position);

			if (screenPoint.z > 0)
				// проверка, необходимая чтобы текст не был виден, если  камера направлена в противоположную сторону относительно объекта
			{
				var worldPosition =
					Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f,
						screenPoint.y + textSize.y * 0.5f + yOffset, screenPoint.z));
				UnityEditor.Handles.Label(worldPosition, textContent, style);
			}
			GUI.skin = prevSkin;
#endif
		}
	}
}