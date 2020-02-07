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
			cells[0].location = MapLocations.Tolchok;
			this.cells = cells;
			this.exit = exit;
		}

		public bool CanMove(Tracker tracker, int steps)
		{
			var index = cells.FindIndex(c => c.tracker == tracker);
			bool canMove = false;
			switch (index)
			{
				case 1:
					canMove = steps == 1 && CanMove(2);
					tracker.UpdateWay(cells[2].GetWay(true).ToArray());
					break;
				case 2:
					canMove = steps == 3 && CanMove(3);
					tracker.UpdateWay(cells[3].GetWay(true).ToArray());
					break;
				case 3:
					canMove = steps == 6 && CanMove(4);
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
			for (int i = positionTo; i < 4; i++)
			{
				canMove = cells[i].tracker == null;
				if (canMove)
					break;
			}
			if (!canMove)
				canMove = cells[3]?.tracker?.mapSide != exit.tracker?.mapSide 
					|| exit.tracker?.mapSide == null;

			return canMove;
		}

		public ICell GetNextCell(Tracker tracker)
		{
			var index = cells.FindIndex(c => c.tracker == tracker);
			if (index < 1)
			{
				return null;
			}
			else if (index >= 3)
			{
				return exit;
			}
			else
			{
				return cells[++index];
			}
		}

		public ICell GetNextCell(ICell cell)
		{
			var index = cells.FindIndex(c => c == cell);
			if (index < 1)
			{
				return null;
			}
			else if (index >= 3)
			{
				return exit;
			}
			else
			{
				return cells[++index];
			}
		}

		public List<ICell> GetExtra(ICell cell) 
		{
			var extra = new List<ICell>();
			int index = cells.FindIndex(c => c == cell);
			if (index >= 0) 
			{
				if (index == 0) // Вход не включаем в extra
					index++;
				for (var i = index; i < cells.Count; i++)
					extra.Add(cells[index]);
				extra.Add(exit);
			}
			return extra;
		}
	}
}