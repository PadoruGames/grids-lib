using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Grids
{
	public interface IHexCell
	{
		Vector3 Center { get; set; }
		HexCoords Coords { get; set; }
	}
}
