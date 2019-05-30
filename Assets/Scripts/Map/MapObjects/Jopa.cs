using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	class Jopa : AStackCells
	{
		public Origin origin;

		public Jopa(Origin origin)
		{
			this.origin = origin;
			var loc = Location.Jopa;
			cells.Add(PlayerPosition.Bottom, new Cell(0, -1f, loc));
			cells.Add(PlayerPosition.Left, new Cell(-1f, 0, loc));
			cells.Add(PlayerPosition.Top, new Cell(0, 1f, loc));
			cells.Add(PlayerPosition.Right, new Cell(1f, 0, loc));
		}

		public override ICell GetTarget(MapPawn pawn, out bool canOccupy)
		{
			canOccupy = true;
			return origin.GetCell(pawn.playerPosition);
		}
	}
}
