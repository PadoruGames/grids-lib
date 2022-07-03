using System;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Grids
{
	public class HexGridDrawer : IGridDrawer
	{
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
		private List<Edge> edgesToDraw;
		private List<IHexCell> cells;

		public HexGridDrawer(List<IHexCell> cells, float outerRadius, float innerRadius)
		{
			this.cells = cells;

			CreateCorners(outerRadius, innerRadius);

			CreateEdges();

			coordsStyle = new GUIStyle();
			coordsStyle.alignment = TextAnchor.MiddleCenter;
			coordsStyle.fontStyle = FontStyle.Bold;
			coordsStyle.normal.textColor = Color.white;
		}

		public void Draw()
		{
			Gizmos.color = Color.white;

			foreach (var edge in edgesToDraw)
			{
				Gizmos.DrawLine(edge.from, edge.to);
			}

#if UNITY_EDITOR
			foreach (var cell in cells)
			{
				UnityEditor.Handles.Label(cell.Center, $"{cell.Coords.x},{cell.Coords.y}", coordsStyle);
			}
#endif
		}

		private void CreateCorners(float outerRadius, float innerRadius)
		{
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

		private void CreateEdges()
		{
			edgesToDraw = new List<Edge>();

			foreach (var cell in cells)
			{
				var cellEdges = GetCellEdges(cell.Center);

				foreach (var edge in cellEdges)
				{
					if (!edgesToDraw.Contains(edge))
					{
						edgesToDraw.Add(edge);
					}
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
