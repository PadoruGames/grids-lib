using System;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Grids
{
	public class HexGridDrawer : IGridDrawer
	{
		private class Cell
		{
			public List<Edge> edges;
			public Vector3 center;
			public Vector2Int coords;
		}

		private class Edge : IEquatable<Edge>
		{
			public Vector3 from;
			public Vector3 to;

			public bool Equals(Edge other)
			{
				return (from == other.from && to == other.to) || (from == other.to && to == other.from);
			}
		}

		private Vector3[] corners;
		private GUIStyle coordsStyle;
		private Vector2Int size;
		private List<Cell> cells;
		private List<Edge> alreadyDrawnEdges = new List<Edge>();
		private Func<Vector2Int, Vector3> girdPositionToWorldPosition;

		public HexGridDrawer(Vector2Int size, float outerRadius, Func<Vector2Int, Vector3> girdPositionToWorldPosition)
		{
			this.size = size;
			this.girdPositionToWorldPosition = girdPositionToWorldPosition;

			CreateCorners(outerRadius);

			CreateCells();

			coordsStyle = new GUIStyle();
			coordsStyle.alignment = TextAnchor.MiddleCenter;
			coordsStyle.fontStyle = FontStyle.Bold;
			coordsStyle.normal.textColor = Color.white;
		}

		public void Draw()
		{
			Gizmos.color = Color.white;

			alreadyDrawnEdges.Clear();

			foreach (var cell in cells)
			{
				DrawCell(cell);
			}
		}

		private void DrawCell(Cell cell)
		{
			foreach (var edge in cell.edges)
			{
				if (alreadyDrawnEdges.Contains(edge))
				{
					continue;
				}

				alreadyDrawnEdges.Add(edge);

				Gizmos.DrawLine(edge.from, edge.to);
			}

			Gizmos.color = Color.white;

#if UNITY_EDITOR
			UnityEditor.Handles.Label(cell.center, $"{cell.coords.x},{cell.coords.y}", coordsStyle);
#endif
		}

		private void CreateCorners(float outerRadius)
		{
			var innerRadius = outerRadius * 0.866025404f;

			corners = new Vector3[]
			{
				new Vector3(0f, outerRadius, 0),
				new Vector3(innerRadius, 0.5f * outerRadius, 0),
				new Vector3(innerRadius, -0.5f * outerRadius, 0),
				new Vector3(0f, -outerRadius, 0),
				new Vector3(-innerRadius, -0.5f * outerRadius, 0),
				new Vector3(-innerRadius, 0.5f * outerRadius, 0)
			};
		}

		private void CreateCells()
		{
			cells = new List<Cell>();

			for (int y = 0; y < size.y; y++)
			{
				for (int x = 0; x < size.x; x++)
				{
					var cell = new Cell();
					cell.center = girdPositionToWorldPosition(new Vector2Int(x, y));
					cell.coords = new Vector2Int(x - y / 2, y);
					cell.edges = GetCellEdges(cell.center);
					cells.Add(cell);
				}
			}
		}

		private List<Edge> GetCellEdges(Vector3 cellCenter)
		{
			var edges = new List<Edge>();

			edges.Add(new Edge()
			{
				from = cellCenter + corners[0],
				to = cellCenter + corners[1],
			});
			edges.Add(new Edge()
			{
				from = cellCenter + corners[1],
				to = cellCenter + corners[2],
			});
			edges.Add(new Edge()
			{
				from = cellCenter + corners[2],
				to = cellCenter + corners[3],
			});
			edges.Add(new Edge()
			{
				from = cellCenter + corners[3],
				to = cellCenter + corners[4],
			});
			edges.Add(new Edge()
			{
				from = cellCenter + corners[4],
				to = cellCenter + corners[5],
			});
			edges.Add(new Edge()
			{
				from = cellCenter + corners[5],
				to = cellCenter + corners[0],
			});

			return edges;
		}
	}
}
