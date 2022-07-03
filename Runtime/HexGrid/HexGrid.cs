using System;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Grids
{
	public class HexGrid<T> : IGrid<T>
	{
		private HexCell<T>[,] cells;
		private Vector3 origin;
		private Vector2Int size;
		private float outerRadius;

		private float innerRadius => outerRadius * 0.866025404f;
		public float CellSize => outerRadius;
		public IGridDrawer GridDrawer { get; }

		public HexGrid(Vector3 origin, Vector2Int size, float cellSize, Func<T> createItemCallback)
		{
			this.origin = origin;
			this.size = size;
			outerRadius = cellSize;

			GridDrawer = new HexGridDrawer(size, outerRadius, GetCellCenter);

			InitializeGrid(createItemCallback);
		}

		public T GetValue(Vector2Int gridPos)
		{
			if (!AreCoordinatesInsideBounds(gridPos))
			{
				return default;
			}

			return cells[gridPos.x, gridPos.y].Value;
		}

		public T GetValue(Vector3 worldPos)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			return GetValue(gridPos);
		}

		public void GetValuesInRow(Vector2Int gridPos, List<T> values)
		{
			if (!AreCoordinatesInsideBounds(gridPos))
			{
				return;
			}

			for (int x = 0; x < size.x; x++)
			{
				values.Add(cells[x, gridPos.y].Value);
			}
		}

		public void GetValuesInRow(Vector3 worldPos, List<T> values)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			GetValuesInRow(gridPos, values);
		}

		public void GetValuesInColumn(Vector2Int gridPos, List<T> values)
		{
			if (!AreCoordinatesInsideBounds(gridPos))
			{
				return;
			}

			for (int y = 0; y < size.x; y++)
			{
				values.Add(cells[gridPos.x, y].Value);
			}
		}

		public void GetValuesInColumn(Vector3 worldPos, List<T> values)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			GetValuesInColumn(gridPos, values);
		}

		public void SetValue(Vector2Int gridPos, T value)
		{
			if (!AreCoordinatesInsideBounds(gridPos))
			{
				return;
			}

			cells[gridPos.x, gridPos.y].Value = value;
		}

		public void SetValue(Vector3 worldPos, T value)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			SetValue(gridPos, value);
		}

		public Vector3 GirdPositionToWorldPosition(Vector2Int gridPos)
		{
			Vector3 cellCenter = Vector3.zero;
			cellCenter.x = (gridPos.x + gridPos.y * 0.5f - gridPos.y / 2) * (innerRadius * 2);
			cellCenter.y = gridPos.y * (outerRadius * 1.5f);
			cellCenter.z = 0f;

			var xOffset = gridPos.y * innerRadius;
			//cellCenter.x += xOffset;

			return origin + cellCenter;
		}

		public Vector2Int WorldPositionToGridPosition(Vector3 worldPos)
		{			
			float x = (worldPos.x - origin.x - innerRadius) / (innerRadius * 2f);
			float y = -x;

			float offset = (worldPos.y - origin.y - outerRadius) / (outerRadius * 3f);
			x -= offset;
			y -= offset;

			int iX = Mathf.RoundToInt(x);
			int iY = Mathf.RoundToInt(y);
			int iZ = Mathf.RoundToInt(-x -y);

			if (iX + iY + iZ != 0)
			{
				float dX = Mathf.Abs(x - iX);
				float dY = Mathf.Abs(y - iY);
				float dZ = Mathf.Abs(-x - y - iZ);

				if (dX > dY && dX > dZ)
				{
					iX = -iY - iZ;
				}
				else if (dZ > dY)
				{
					iZ = -iX - iY;
				}
			}

			return new Vector2Int(iX, iZ);
		}

		public Vector3 GetCellCenter(Vector2Int gridPos)
		{
			var cellPos = GirdPositionToWorldPosition(gridPos);
			cellPos.x += innerRadius;
			cellPos.y += outerRadius;
			return cellPos;
		}

		public Vector3 GetCellCenter(Vector3 worldPos)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			return GetCellCenter(gridPos);
		}

		private void InitializeGrid(Func<T> createItemCallback)
		{
			cells = new HexCell<T>[size.x, size.y];

			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					var cell = new HexCell<T>();
					cell.Center = GirdPositionToWorldPosition(new Vector2Int(x, y));
					cell.Coords = new HexCoords(x - y / 2, y);
					cell.Value = createItemCallback.Invoke();
					cells[x, y] = cell;
				}
			}
		}

		private bool AreCoordinatesInsideBounds(Vector2Int gridPos)
		{
			return gridPos.x >= 0 && gridPos.x < size.x && gridPos.y >= 0 && gridPos.y < size.y;
		}
	}
}
