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

		public bool CanMove(Tracker tracker, int steps)
		{
			var index = cells.FindIndex(c => c.tracker == tracker);
			bool canMove = false;
			switch (index)
			{
				case 0:
					canMove = steps == 1 && CanMove(1);
					tracker.UpdateWay(cells[1].GetWay(true).ToArray());
					break;
				case 1:
					canMove = steps == 3 && CanMove(2);
					tracker.UpdateWay(cells[2].GetWay(true).ToArray());
					break;
				case 2:
					canMove = steps == 6 && CanMove(3);
					tracker.UpdateWay(exit.GetWay(true).ToArray());
					break;
				default:
					canMove = false;
					break;
			}
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
				canMove = cells[i].tracker == null;
				if (canMove)
					break;
			}
			if (!canMove)
				canMove = cells[2]?.tracker?.mapSide != exit.tracker?.mapSide 
					|| exit.tracker?.mapSide == null;

			return canMove;
		}

		public ICell GetNextCell(Tracker tracker)
		{
			var index = cells.FindIndex(c => c.tracker == tracker);
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