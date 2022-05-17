using UnityEngine;

namespace Padoru.Grids
{
	public interface IGrid<T>
	{
		float CellSize { get; }

		T GetValue(Vector2Int gridPos);

		T GetValue(Vector3 worldPos);

		void SetValue(Vector2Int gridPos, T value);

		void SetValue(Vector3 worldPos, T value);

		Vector2Int WorldPositionToGridPosition(Vector3 worldPos);

		Vector3 GirdPositionToWorldPosition(Vector2Int gridPos);

		public Vector3 GetCellCenter(Vector2Int gridPos);

		public Vector3 GetCellCenter(Vector3 worldPos);

		void DrawGrid();
	}
}
