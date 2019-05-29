using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Home
	{
		public Circle circle;
		private Dictionary<PlayerPosition, List<Cell>> cells = new Dictionary<PlayerPosition, List<Cell>>();
		
		public Home(Circle circle)
		{
			this.circle = circle;
			circle.home = this;

			var loc = Location.Home;
			var lCells = new List<Cell>();
			for (int z = -5; z <= -2; z++)
				lCells.Add(new Cell(0, z, loc));
			cells.Add(PlayerPosition.Bottom, lCells);
			lCells = new List<Cell>();
			for (int x = -5; x <= -2; x++)
				lCells.Add(new Cell(x, 0, loc));
			cells.Add(PlayerPosition.Left, lCells);
			lCells = new List<Cell>();
			for (int z = 5; z >= 2; z--)
				lCells.Add(new Cell(0, z, loc));
			cells.Add(PlayerPosition.Top, lCells);
			lCells = new List<Cell>();
			for (int x = 5; x >= 2; x--)
				lCells.Add(new Cell(x, 0, loc));
			cells.Add(PlayerPosition.Right, lCells);
		}

		public bool CanMove(MapPawn pawn, int steps, Trace trace = null)
		{
			if (trace == null)
				trace = new Trace(from: pawn.trace?.from);
			int end = GetEnd(pawn.playerPosition);
			int index = cells[pawn.playerPosition].FindIndex(c => c.pawn == pawn);
			bool canMove = true;
			bool back = false;
			if (index > end)
				return false;
			while (canMove && steps > 0)
			{
				if (index == end)
					back = true;
				if (back && index == 0)
					return circle.CanMove(pawn, steps, trace, true);
				if (back)
					index = --index + cells[pawn.playerPosition].Count;
				else
					index++;
				index %= cells[pawn.playerPosition].Count;
				var cell = cells[pawn.playerPosition][index];
				canMove = cell.CanOccupy(pawn, steps == 1);
				trace.UpdateTrace(cell);
				steps--;
			}

			pawn.SetTrace(canMove, canMove ? trace : null);
			return canMove;
		}

		private int GetEnd(PlayerPosition playerPosition)
		{
			for(int i = 3; i > 0; i--)
			{
				if (cells[playerPosition][i].pawn == null)
					return i;
			}
			return 0;
		}
	}
}