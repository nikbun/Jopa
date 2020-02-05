using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	class Jopa : AStack
	{
		public Origin origin;

		public Jopa(Origin origin)
		{
			this.origin = origin;
			var loc = MapLocations.Jopa;
			cells.Add(MapSides.Bottom, new Cell(0, -1f, loc));
			cells.Add(MapSides.Left, new Cell(-1f, 0, loc));
			cells.Add(MapSides.Top, new Cell(0, 1f, loc));
			cells.Add(MapSides.Right, new Cell(1f, 0, loc));
		}

		public override ICell GetTarget(Tracker tracker, out bool canOccupy)
		{
			canOccupy = true;
			return origin.GetCell(tracker.mapSide);
		}
	}
}
