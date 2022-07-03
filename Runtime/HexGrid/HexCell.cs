using UnityEngine;

namespace Padoru.Grids
{
	public class HexCell<T> : IHexCell
	{
		public T Value { get; set; }
		public Vector3 Center { get; set; }
		public HexCoords Coords { get; set; }
	}
}
