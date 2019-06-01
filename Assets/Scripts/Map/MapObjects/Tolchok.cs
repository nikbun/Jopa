using UnityEngine;
using System.Collections.Generic;


namespace Map.MapObjects
{
	public class Tolchok
	{
		public ICell exit;
		private List<ICell> cells;

		public Tolchok(List<ICell> cells, ICell exit)
		{
			this.cells = cells;
			this.exit = exit;
		}

		public bool CanMove(MapPawn pawn, int steps)
		{
			var trace = new Trace(from: pawn.trace?.from);
			var index = cells.FindIndex(c => c.pawn == pawn);
			bool canMove = false;
			switch (index)
			{
				case 0:
					canMove = steps == 1 && CanMove(1);
					trace.UpdateTrace(cells[1], true);
					break;
				case 1:
					canMove = steps == 3 && CanMove(2);
					trace.UpdateTrace(cells[2], true);
					break;
				case 2:
					canMove = steps == 6 && CanMove(3);
					trace.UpdateTrace(exit, true);
					break;
				default:
					canMove = false;
					break;
			}
			 
			pawn.SetTrace(canMove, canMove ? trace : null);
			return canMove;
		}

		public ICell GetCell(int index)
		{
			return cells[index];
		}

		public bool CanMove(int positionTo)
		{
			bool canMove = false;
			for (int i = positionTo; i < 3; i++)
			{
				canMove = cells[i].pawn == null;
				if (canMove)
					break;
			}
			if (!canMove)
				canMove = cells[2]?.pawn?.playerPosition != exit.pawn?.playerPosition 
					|| exit.pawn?.playerPosition == null;

			return canMove;
		}

		public ICell GetNextCell(MapPawn pawn)
		{
			var index = cells.FindIndex(c => c.pawn == pawn);
			if (index < 0)
			{
				return null;
			}
			else if (index >= 2)
			{
				return exit;
			}
			else
			{
				return cells[++index];
			}
		}
	}
}