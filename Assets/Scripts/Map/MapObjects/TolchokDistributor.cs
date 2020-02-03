using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class TolchokDistributor
	{
		Circle circle;
		List<Tolchok> tolcheks = new List<Tolchok>();

		public TolchokDistributor(Circle circle)
		{
			this.circle = circle;

			List<ICell> cells = new List<ICell>();
			cells.Add(new Cell(-3, -5, Location.Tolchok));
			cells.Add(new Cell(-4, -5, Location.Tolchok));
			cells.Add(new Cell(-5, -5, Location.Tolchok));
			Tolchok tolchek = new Tolchok(cells, circle.GetCell(6));
			circle.SetTolchek(tolchek, 3);
			tolcheks.Add(tolchek);

			cells = new List<ICell>();
			cells.Add(new Cell(-5, 3, Location.Tolchok));
			cells.Add(new Cell(-5, 4, Location.Tolchok));
			cells.Add(new Cell(-5, 5, Location.Tolchok));
			tolchek = new Tolchok(cells, circle.GetCell(18));
			circle.SetTolchek(tolchek, 15);
			tolcheks.Add(tolchek);
			
			cells = new List<ICell>();
			cells.Add(new Cell(3, 5, Location.Tolchok));
			cells.Add(new Cell(4, 5, Location.Tolchok));
			cells.Add(new Cell(5, 5, Location.Tolchok));
			tolchek = new Tolchok(cells, circle.GetCell(30));
			circle.SetTolchek(tolchek, 27);
			tolcheks.Add(tolchek);

			cells = new List<ICell>();
			cells.Add(new Cell(5, -3, Location.Tolchok));
			cells.Add(new Cell(5, -4, Location.Tolchok));
			cells.Add(new Cell(5, -5, Location.Tolchok));
			tolchek = new Tolchok(cells, circle.GetCell(42));
			circle.SetTolchek(tolchek, 39);
			tolcheks.Add(tolchek);
		}

		public bool CanMove(Tracker tracker, int steps)
		{
			bool canMove = false;
			foreach (var tolchek in tolcheks)
			{
				canMove = canMove || tolchek.CanMove(tracker, steps);
			}
			return canMove;
		}

		public Trace GetTrace(Tracker tracker)
		{
			foreach (var tolchek in tolcheks)
			{
				ICell cell = tolchek.GetNextCell(tracker);
				if (cell != null)
				{
					tracker.trace.UpdateTrace(cell);
					break;
				}
					
			}
			return tracker.trace;
		}
	}
}