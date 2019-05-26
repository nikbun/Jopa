using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapObjects
{
	abstract class AStackCells
	{
		public Dictionary<PlayerPosition, Cell> cells = new Dictionary<PlayerPosition, Cell>();

		public bool CanMove(MapPawn pawn, int steps)
		{
			var trace = new Trace(new List<Vector3>(), pawn.trace?.from, null);
			if (steps != 6)
				return false;
			bool canMove;
			trace.to = GetTarget(pawn, out canMove);
			trace.way.Add(trace.to.position);

			if (canMove) // TODO Стоит переделать метод
			{
				pawn.trace = trace;
				pawn.canMove = canMove;
			}
			return canMove;
		}

		public Vector3 GetPosition(PlayerPosition playerPosition)
		{
			return GetCell(playerPosition).position;
		}

		public Cell GetCell(PlayerPosition playerPosition)
		{
			return cells[playerPosition];
		}

		public abstract Cell GetTarget(MapPawn pawn, out bool canOccupy);
	}
}