using System;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Grids
{
	public class SquareGrid<T> : IGrid<T>
	{
		private T[,] items;
		private Vector3 origin;
		
		public Vector2Int Size { get; }
		public float CellSize { get; }
		public IGridDrawer GridDrawer { get; }

        public SquareGrid(Vector3 origin, Vector2Int size, float cellSize, Func<T> createItemCallback)
		{
			items = new T[size.x, size.y];
			this.origin = origin;
			Size = size;
			CellSize = cellSize;

			GridDrawer = new SquareGridDrawer(size, cellSize, GridPositionToWorldPosition);

			InitializeGrid(createItemCallback);
		}

		public T GetValue(Vector2Int gridPos)
		{
			if (!AreCoordinatesInsideBounds(gridPos))
			{
				return default;
			}

			return items[gridPos.x, gridPos.y];
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

			for (int x = 0; x < Size.x; x++)
			{
				values.Add(items[x, gridPos.y]);
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

			for (int y = 0; y < Size.x; y++)
			{
				values.Add(items[gridPos.x, y]);
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

			items[gridPos.x, gridPos.y] = value;
		}

		public void SetValue(Vector3 worldPos, T value)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			SetValue(gridPos, value);
		}

		public Vector2Int WorldPositionToGridPosition(Vector3 worldPos)
		{
			var x = Mathf.FloorToInt((worldPos.x - origin.x) / CellSize);
			var y = Mathf.FloorToInt((worldPos.y - origin.y) / CellSize);

			return new Vector2Int(x, y);
		}

		public Vector3 GridPositionToWorldPosition(Vector2Int gridPos)
		{
			return origin + new Vector3(gridPos.x, gridPos.y) * CellSize + new Vector3(1, 1) * CellSize / 2f;
		}

		public Vector3 GetCellCenter(Vector3 worldPos)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			return GridPositionToWorldPosition(gridPos);
		}

		private void InitializeGrid(Func<T> createItemCallback)
		{
			for (int x = 0; x < Size.x; x++)
			{
				for (int y = 0; y < Size.y; y++)
				{
					items[x, y] = createItemCallback.Invoke();
				}
			}
		}

		private bool AreCoordinatesInsideBounds(Vector2Int gridPos)
		{
			return gridPos.x >= 0 && gridPos.x < Size.x && gridPos.y >= 0 && gridPos.y < Size.y;
		}
	}
}
