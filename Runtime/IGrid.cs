using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Grids
{
	public interface IGrid<T>
	{
		T this[int x, int y] { get; }
		
		Vector2Int Size { get; }
		
		float CellSize { get; }

		IGridDrawer GridDrawer { get; }

		T GetValue(Vector2Int gridPos);

		T GetValue(Vector3 worldPos);

		void GetValuesInRow(Vector2Int gridPos, List<T> values);
		 
		void GetValuesInRow(Vector3 worldPos, List<T> values);
		 
		void GetValuesInColumn(Vector2Int gridPos, List<T> values);
		 
		void GetValuesInColumn(Vector3 worldPos, List<T> values);
		
		void GetValuesInRadius(Vector2Int gridPos, Vector2Int size, List<T> values);
		
		void GetEdgeCellPositions(List<Vector3> positions);

		void SetValue(Vector2Int gridPos, T value);

		void SetValue(Vector3 worldPos, T value);

		Vector2Int WorldPositionToGridPosition(Vector3 worldPos);

		Vector3 GridPositionToWorldPosition(Vector2Int gridPos);

		Vector3 GetCellCenter(Vector3 worldPos);
	}
}
