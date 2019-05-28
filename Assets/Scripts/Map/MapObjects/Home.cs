using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Home
	{
		public Circle circle;
		private Dictionary<PlayerPosition, List<Cell>> dicCells = new Dictionary<PlayerPosition, List<Cell>>();
		
		public Home(Circle circle)
		{
			this.circle = circle;
			circle.home = this;

			var loc = Location.Home;
			var lCells = new List<Cell>();
			for (int z = -5; z <= -2; z++)
				lCells.Add(new Cell(0, z, loc));
			dicCells.Add(PlayerPosition.Bottom, lCells);
			lCells = new List<Cell>();
			for (int x = -5; x <= -2; x++)
				lCells.Add(new Cell(x, 0, loc));
			dicCells.Add(PlayerPosition.Left, lCells);
			lCells = new List<Cell>();
			for (int z = 5; z >= 2; z--)
				lCells.Add(new Cell(0, z, loc));
			dicCells.Add(PlayerPosition.Top, lCells);
			lCells = new List<Cell>();
			for (int x = 5; x >= 2; x--)
				lCells.Add(new Cell(x, 0, loc));
			dicCells.Add(PlayerPosition.Right, lCells);
		}

		public bool CanMove(MapPawn pawn, int steps, Trace trace = null)
		{
			var cells = dicCells[pawn.playerPosition];
			int iFrom = 0;
			if (trace == null)
			{
				trace = new Trace(new List<Vector3>(), pawn.trace?.from, null);
				iFrom = cells.FindIndex(c => c.pawn == pawn) + 1;
			}
			int end = GetEndNumber(cells);
			int iTo = (iFrom + steps);
			int iBack = iTo - end - 1;
			bool canMove = true;

			if (iFrom > end)
				return false;
			for (int i = iFrom; i < iTo && i <= end && canMove; i++)
			{
				trace.to = cells[i % cells.Count];
				canMove = trace.to.CanOccupy(pawn);
				trace.way.Add(trace.to.position);
			}
			if (iBack > 0)
			{
				for (int i=end-1; i>=end-iBack && i>=0 && canMove; i--)
				{
					trace.to = cells[i % cells.Count];
					canMove = trace.to.CanOccupy(pawn);
					trace.way.Add(trace.to.position);
				}
				if (iBack - end > 0)
					return circle.CanMove(pawn, iBack - end, trace, true);
			}

			if (canMove)
			{
				pawn.trace = trace;
				pawn.canMove = canMove;
			}
			return canMove;
		}

		private int GetEndNumber(List<Cell> cells)
		{
			for(int i = 3; i > 0; i--)
			{
				if (cells[i].pawn == null)
					return i;
			}
			return 0;
		}
	}
}