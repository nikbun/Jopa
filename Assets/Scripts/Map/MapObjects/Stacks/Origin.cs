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
			cells.Add(MapSides.Bottom, new Cell(0, -7.2f, loc));
			cells.Add(MapSides.Left, new Cell(-7.2f, 0, loc));
			cells.Add(MapSides.Top, new Cell(0, 7.2f, loc));
			cells.Add(MapSides.Right, new Cell(7.2f, 0, loc));
		}

		public override ICell GetTarget(Tracker tracker, out bool canOccupy)
		{
			var cell = circle.GetCell(0, tracker.mapSide);
			canOccupy = cell.CanOccupy(tracker, true);
			return cell;
		}
	}
}