using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Grids
{
	public class SquareGrid<T> : IGrid<T>
	{
		private readonly T[,] items;
		private readonly Vector3 origin;
		private readonly Vector3 gridForward;
		private readonly Vector3 gridRight;
		private readonly Vector3 gridOffset;
		
		public T this[int x, int y] => items[x, y];
		public Vector2Int Size { get; }
		public float CellSize { get; }
		public IGridDrawer GridDrawer { get; }

        public SquareGrid(Vector3 origin, Vector3 gridForward, Vector3 gridRight, Vector2Int size, float cellSize, Func<Vector2Int, T> createItemCallback)
		{
			items = new T[size.x, size.y];
			this.origin = origin;
			this.gridForward = gridForward;
			this.gridRight = gridRight;
			Size = size;
			CellSize = cellSize;

			gridOffset = (gridRight + gridForward) * CellSize / 2f;
			
			GridDrawer = new SquareGridDrawer(size, gridOffset, cellSize, GridPositionToWorldPosition);

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

		public void GetValuesInRadius(Vector2Int gridPos, Vector2Int size, List<T> values)
		{
			if (!AreCoordinatesInsideBounds(gridPos))
			{
				return;
			}

			var halfSize = size / 2;
			var min = gridPos - halfSize;
			var max = gridPos + halfSize;
			for (int x = min.x; x <= max.x; x++)
			{
				for (int y = min.y; y <= max.y; y++)
				{
					if (!AreCoordinatesInsideBounds(new Vector2Int(x, y)))
					{
						continue;
					}
					
					values.Add(items[x, y]);
				}
			}
		}

		public void GetEdgeCellPositions(List<Vector3> positions)
		{
			for (int x = 0; x < items.GetLength(0); x++)
			{
				for (int y = 0; y < items.GetLength(1); y++)
				{
					var coords = new Vector2Int(x, y);
					
					if (AreCoordinatesAnEdge(coords))
					{
						var worldPos = GridPositionToWorldPosition(coords);
						positions.Add(worldPos);
					}
				}
			}
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
			var displacement = worldPos - origin;
			var forwardDot = Vector3.Dot(displacement, gridForward);
			var rightDot = Vector3.Dot(displacement, gridRight);
			var forwardCoordinate = (int)(forwardDot / CellSize);
			var rightCoordinate = (int)(rightDot / CellSize);
			
			return new Vector2Int(rightCoordinate, forwardCoordinate);
		}

		public Vector3 GridPositionToWorldPosition(Vector2Int gridPos)
		{	
			var cellCoordinates = gridRight * gridPos.x + gridForward * gridPos.y;
			var cellPos = cellCoordinates * CellSize;
			
			return origin + cellPos + gridOffset;
		}

		public Vector3 GetCellCenter(Vector3 worldPos)
		{
			var gridPos = WorldPositionToGridPosition(worldPos);
			return GridPositionToWorldPosition(gridPos);
		}

		private void InitializeGrid(Func<Vector2Int, T> createItemCallback)
		{
			for (int x = 0; x < Size.x; x++)
			{
				for (int y = 0; y < Size.y; y++)
				{
					items[x, y] = createItemCallback.Invoke(new Vector2Int(x, y));
				}
			}
		}

		private bool AreCoordinatesInsideBounds(Vector2Int gridPos)
		{
			return gridPos.x >= 0 && gridPos.x < Size.x && gridPos.y >= 0 && gridPos.y < Size.y;
		}

		private bool AreCoordinatesAnEdge(Vector2Int gridPos)
		{
			return gridPos.x == 0 || gridPos.x == Size.x - 1 || gridPos.y == 0 || gridPos.y == Size.y - 1;
		}
	}
}
