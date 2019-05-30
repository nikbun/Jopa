using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapObjects
{
	abstract class AStackCells
	{
		public Dictionary<PlayerPosition, ICell> cells = new Dictionary<PlayerPosition, ICell>();

		public bool CanMove(MapPawn pawn, int steps)
		{
			var trace = new Trace(from:pawn.trace?.from);
			if (steps != 6)
				return false;
			bool canMove;
			trace.UpdateTrace(GetTarget(pawn, out canMove));
			pawn.SetTrace(canMove, canMove ? trace : null);
			return canMove;
		}

		public Vector3 GetPosition(PlayerPosition playerPosition)
		{
			return GetCell(playerPosition).GetWay()[0];
		}

		public ICell GetCell(PlayerPosition playerPosition)
		{
			return cells[playerPosition];
		}

		public abstract ICell GetTarget(MapPawn pawn, out bool canOccupy);
	}
}