using UnityEngine;

namespace Padoru.Grids
{
	public class HexCoords : MonoBehaviour
	{
		private readonly Vector2Int coords;

		public int x => coords.x;
		public int y => coords.y;

		public HexCoords(int x, int y)
		{
			coords = new Vector2Int(x, y);
		}

		public override string ToString()
		{
			return $"{coords.x}, {coords.y}";
		}
	}
}
