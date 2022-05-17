using System;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Grids
{
	public class SquareGrid<T> : IGrid<T>
	{
		private T[,] items;
		private Vector3 origin;
		private Vector2Int size;

        public float CellSize { get; }

        public SquareGrid(Vector3 origin, Vector2Int size, float cellSize, Func<T> createItemCallback)
		{
			items = new T[size.x, size.y];
			this.origin = origin;
			this.size = size;
			CellSize = cellSize;

			InitializeGrid(createItemCallback);
		}

		public T GetValue(Vector2Int gridPos)
		{
			if (gridPos.x < 0 || gridPos.x >= size.x || gridPos.y < 0 || gridPos.y >= size.y)
			{
				Debug.LogError($"Index out of bounds in grid for position {gridPos}. Returning default.");
				return default;
			}

			return items[gridPos.x, gridPos.y];
		}

		public T GetValue(Vector3 worldPos)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			return GetValue(gridPos);
		}

		public void SetValue(Vector2Int gridPos, T value)
		{
			if (gridPos.x < 0 || gridPos.x >= size.x || gridPos.y < 0 || gridPos.y >= size.y)
			{
				Debug.LogError($"Index out of bounds in grid for position {gridPos}. Returning default.");
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

		public Vector3 GirdPositionToWorldPosition(Vector2Int gridPos)
		{
			return origin + new Vector3(gridPos.x, gridPos.y) * CellSize;
		}

		public Vector3 GetCellCenter(Vector2Int gridPos)
		{
			return GirdPositionToWorldPosition(gridPos) + new Vector3(1, 1) * CellSize / 2f;
		}

		public Vector3 GetCellCenter(Vector3 worldPos)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			return GirdPositionToWorldPosition(gridPos) + new Vector3(1, 1) * CellSize / 2f;
		}

		public void DrawGrid()
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					UnityEngine.Debug.DrawLine(GirdPositionToWorldPosition(new Vector2Int(x, y)), GirdPositionToWorldPosition(new Vector2Int(x + 1, y)), Color.white, 1000f);
					UnityEngine.Debug.DrawLine(GirdPositionToWorldPosition(new Vector2Int(x, y)), GirdPositionToWorldPosition(new Vector2Int(x, y + 1)), Color.white, 1000f);
				}
			}

			UnityEngine.Debug.DrawLine(GirdPositionToWorldPosition(new Vector2Int(size.x, 0)), GirdPositionToWorldPosition(new Vector2Int(size.x, size.y)), Color.white, 1000f);
			UnityEngine.Debug.DrawLine(GirdPositionToWorldPosition(new Vector2Int(0, size.y)), GirdPositionToWorldPosition(new Vector2Int(size.x, size.y)), Color.white, 1000f);
		}

		private void InitializeGrid(Func<T> createItemCallback)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					items[x, y] = createItemCallback.Invoke();
				}
			}
		}
	}
}
