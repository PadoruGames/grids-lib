using System;
using UnityEngine;

namespace Padoru.Grids
{
	public class SquareGridDrawer : IGridDrawer
	{
		private Vector2Int size;
		private float cellSize;
		private Func<Vector2Int, Vector3> girdPositionToWorldPosition;
		private GUIStyle coordsStyle;

		public SquareGridDrawer(Vector2Int size, float cellSize, Func<Vector2Int, Vector3> girdPositionToWorldPosition)
		{
			this.size = size;
			this.cellSize = cellSize;
			this.girdPositionToWorldPosition = girdPositionToWorldPosition;

			coordsStyle = new GUIStyle();
			coordsStyle.alignment = TextAnchor.MiddleCenter;
			coordsStyle.fontStyle = FontStyle.Bold;
			coordsStyle.normal.textColor = Color.white;
		}

		public void Draw()
		{
			Gizmos.color = Color.white;

			var offset = new Vector3(-cellSize / 2f, -cellSize / 2f);

			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					var cellPos = girdPositionToWorldPosition(new Vector2Int(x, y));
					Gizmos.DrawLine(cellPos + offset, girdPositionToWorldPosition(new Vector2Int(x + 1, y)) + offset);
					Gizmos.DrawLine(cellPos + offset, girdPositionToWorldPosition(new Vector2Int(x, y + 1)) + offset);

#if UNITY_EDITOR
					UnityEditor.Handles.Label(cellPos, $"{x},{y}", coordsStyle);
#endif
				}
			}

			Gizmos.DrawLine(girdPositionToWorldPosition(new Vector2Int(size.x, 0)) + offset, girdPositionToWorldPosition(new Vector2Int(size.x, size.y)) + offset);
			Gizmos.DrawLine(girdPositionToWorldPosition(new Vector2Int(0, size.y)) + offset, girdPositionToWorldPosition(new Vector2Int(size.x, size.y)) + offset);
		}
	}
}
