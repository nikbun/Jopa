using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Home
	{
		public Circle circle;
		private Dictionary<PlayerPosition, List<ICell>> cells = new Dictionary<PlayerPosition, List<ICell>>();
		
		public Home(Circle circle)
		{
			this.circle = circle;
			circle.home = this;

			var loc = Location.Home;
			var lCells = new List<ICell>();
			for (int z = -5; z <= -2; z++)
				lCells.Add(new Cell(0, z, loc));
			cells.Add(PlayerPosition.Bottom, lCells);
			lCells = new List<ICell>();
			for (int x = -5; x <= -2; x++)
				lCells.Add(new Cell(x, 0, loc));
			cells.Add(PlayerPosition.Left, lCells);
			lCells = new List<ICell>();
			for (int z = 5; z >= 2; z--)
				lCells.Add(new Cell(0, z, loc));
			cells.Add(PlayerPosition.Top, lCells);
			lCells = new List<ICell>();
			for (int x = 5; x >= 2; x--)
				lCells.Add(new Cell(x, 0, loc));
			cells.Add(PlayerPosition.Right, lCells);
		}

		public bool CanMove(Tracker tracker, int steps, Trace trace = null)
		{
			trace = tracker.trace;
			int end = GetEnd(tracker.playerPosition);
			int index = cells[tracker.playerPosition].FindIndex(c => c.tracker == tracker);
			bool canMove = true;
			bool back = false;
			if (index > end)
				return false;
			while (canMove && steps > 0)
			{
				if (index == end)
					back = true;
				if (back && index == 0)
					return circle.CanMove(tracker, steps, trace, true);
				if (back)
					index = --index + cells[tracker.playerPosition].Count;
				else
					index++;
				index %= cells[tracker.playerPosition].Count;
				var cell = cells[tracker.playerPosition][index];
				canMove = cell.CanOccupy(tracker, steps == 1);
				trace.UpdateTrace(cell);
				steps--;
			}

			tracker.SetTrace(canMove, canMove ? trace : null);
			return canMove;
		}

		private int GetEnd(PlayerPosition playerPosition)
		{
			for(int i = 3; i >= 0; i--)
			{
				if (cells[playerPosition][i].tracker == null)
					return i;
			}
			return -1;
		}
	}
}