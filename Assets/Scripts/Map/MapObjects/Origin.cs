using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapObjects
{
	class Origin : AStackCells
	{
		public Circle circle;

		public Origin(Circle circle)
		{
			this.circle = circle;
			var loc = Location.Origin;
			cells.Add(PlayerPosition.Bottom, new Cell(0, -7.2f, loc));
			cells.Add(PlayerPosition.Left, new Cell(-7.2f, 0, loc));
			cells.Add(PlayerPosition.Top, new Cell(0, 7.2f, loc));
			cells.Add(PlayerPosition.Right, new Cell(7.2f, 0, loc));
		}

		public override ICell GetTarget(MapPawn pawn, out bool canOccupy)
		{
			var cell = circle.GetCell(0, pawn.playerPosition);
			canOccupy = cell.CanOccupy(pawn, true);
			return cell;
		}
	}
}