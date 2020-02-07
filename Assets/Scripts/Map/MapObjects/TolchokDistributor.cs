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
			cells.Add(circle.GetCell(3));
			cells.Add(new Cell(-3, -5, MapLocations.Tolchok, 1));
			cells.Add(new Cell(-4, -5, MapLocations.Tolchok, 3));
			cells.Add(new Cell(-5, -5, MapLocations.Tolchok, 6));
			Tolchok tolchek = new Tolchok(cells, circle.GetCell(6));
			circle.SetTolchek(tolchek, 3);
			tolcheks.Add(tolchek);

			cells = new List<ICell>();
			cells.Add(circle.GetCell(15));
			cells.Add(new Cell(-5, 3, MapLocations.Tolchok, 1));
			cells.Add(new Cell(-5, 4, MapLocations.Tolchok, 3));
			cells.Add(new Cell(-5, 5, MapLocations.Tolchok, 6));
			tolchek = new Tolchok(cells, circle.GetCell(18));
			circle.SetTolchek(tolchek, 15);
			tolcheks.Add(tolchek);
			
			cells = new List<ICell>();
			cells.Add(circle.GetCell(27));
			cells.Add(new Cell(3, 5, MapLocations.Tolchok, 1));
			cells.Add(new Cell(4, 5, MapLocations.Tolchok, 3));
			cells.Add(new Cell(5, 5, MapLocations.Tolchok, 6));
			tolchek = new Tolchok(cells, circle.GetCell(30));
			circle.SetTolchek(tolchek, 27);
			tolcheks.Add(tolchek);

			cells = new List<ICell>();
			cells.Add(circle.GetCell(39));
			cells.Add(new Cell(5, -3, MapLocations.Tolchok, 1));
			cells.Add(new Cell(5, -4, MapLocations.Tolchok, 3));
			cells.Add(new Cell(5, -5, MapLocations.Tolchok, 6));
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

		public ICell GetTolchek(Tracker tracker)
		{
			foreach (var tolchek in tolcheks)
			{
				ICell cell = tolchek.GetNextCell(tracker);
				if (cell != null)
				{
					return cell;
				}
			}
			return null;
		}

		public ICell GetNextCell(ICell cell, MapSides side) 
		{
			ICell nextCell = null;
			foreach (var tolchek in tolcheks)
			{
				nextCell = tolchek.GetNextCell(cell);
				if (nextCell != null) 
					return nextCell;
			}
			if (nextCell == null)
			{
				nextCell = circle.GetNextCell(cell, side);
			}
			return nextCell;
		}

		public List<ICell> GetExtra(ICell cell) 
		{
			foreach (var tolchek in tolcheks)
			{
				var extra = tolchek.GetExtra(cell);
				if (extra.Count > 0) 
					return extra;
			}
			return null;
		}
	}
}