using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapObjects
{
	class Origin : AStack
	{
		public Circle circle;

		public Origin(Circle circle)
		{
			this.circle = circle;
			var loc = MapLocations.Origin;
			cells.Add(MapSides.Bottom, new Cell(0, -7.2f, loc, 6));
			cells.Add(MapSides.Left, new Cell(-7.2f, 0, loc, 6));
			cells.Add(MapSides.Top, new Cell(0, 7.2f, loc, 6));
			cells.Add(MapSides.Right, new Cell(7.2f, 0, loc, 6));
		}

		public override ICell GetNextCell(MapSides side)
		{
			return circle.GetCell(0, side);
		}
	}
}