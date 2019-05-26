using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapObjects
{
	class Origin
	{
		public Circle circle;
		private Dictionary<PlayerPosition, Cell> originCells = new Dictionary<PlayerPosition, Cell>();

		public Origin(Circle circle)
		{
			this.circle = circle;
			var loc = Location.Origin;
			originCells.Add(PlayerPosition.Bottom, new Cell(0, -7.2f, loc));
			originCells.Add(PlayerPosition.Left, new Cell(-7.2f, 0, loc));
			originCells.Add(PlayerPosition.Top, new Cell(0, 7.2f, loc));
			originCells.Add(PlayerPosition.Right, new Cell(7.2f, 0, loc));
		}

		public Vector3 GetPosition(PlayerPosition playerPosition)
		{
			return originCells[playerPosition].position;
		}

		public bool CanMove(MapPawn pawn, int steps)
		{
			var trace = new Trace(new List<Vector3>(), pawn.trace?.from, null);
			bool canMove = steps == 6;
			if (canMove)
			{
				trace.to = circle.GetCell(0, pawn.playerPosition);
				canMove = trace.to.CanOccupy(pawn);
				trace.way.Add(trace.to.position);
			}
			if(canMove) // TODO Стоит переделать метод
			{
				pawn.trace = trace;
				pawn.canMove = canMove;
			}
			return canMove;
		}
	}
}