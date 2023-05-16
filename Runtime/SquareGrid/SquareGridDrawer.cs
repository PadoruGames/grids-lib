using System;
using UnityEngine;

namespace Padoru.Grids
{
	public class SquareGridDrawer : IGridDrawer
	{
		private readonly Vector3 offset;
		private Vector2Int size;
		private float cellSize;
		private Func<Vector2Int, Vector3> girdPositionToWorldPosition;
		private GUIStyle coordsStyle;

		public SquareGridDrawer(Vector2Int size, Vector3 offset, float cellSize, Func<Vector2Int, Vector3> girdPositionToWorldPosition)
		{
			this.size = size;
			this.cellSize = cellSize;
			this.offset = offset;
			this.girdPositionToWorldPosition = girdPositionToWorldPosition;

			coordsStyle = new GUIStyle();
			coordsStyle.alignment = TextAnchor.MiddleCenter;
			coordsStyle.fontStyle = FontStyle.Bold;
			coordsStyle.normal.textColor = Color.white;
		}

		public void Draw()
		{
			Gizmos.color = Color.white;

			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					var cellPos = girdPositionToWorldPosition(new Vector2Int(x, y));
					Gizmos.DrawLine(cellPos - offset, girdPositionToWorldPosition(new Vector2Int(x + 1, y)) - offset);
					Gizmos.DrawLine(cellPos - offset, girdPositionToWorldPosition(new Vector2Int(x, y + 1)) - offset);

#if UNITY_EDITOR
					UnityEditor.Handles.Label(cellPos, $"{x},{y}", coordsStyle);
#endif
				}
			}

			Gizmos.DrawLine(girdPositionToWorldPosition(new Vector2Int(size.x, 0)) - offset, girdPositionToWorldPosition(new Vector2Int(size.x, size.y)) - offset);
			Gizmos.DrawLine(girdPositionToWorldPosition(new Vector2Int(0, size.y)) - offset, girdPositionToWorldPosition(new Vector2Int(size.x, size.y)) - offset);
		}
	}
}
