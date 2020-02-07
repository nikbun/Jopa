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