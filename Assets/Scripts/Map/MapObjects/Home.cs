using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Home
	{
		public Circle circle;
		private Dictionary<MapSides, List<ICell>> cells = new Dictionary<MapSides, List<ICell>>();
		
		public Home(Circle circle)
		{
			this.circle = circle;
			circle.home = this;

			var loc = MapLocations.Home;
			var lCells = new List<ICell>();
			for (int z = -5; z <= -2; z++)
				lCells.Add(new Cell(0, z, loc));
			cells.Add(MapSides.Bottom, lCells);
			lCells = new List<ICell>();
			for (int x = -5; x <= -2; x++)
				lCells.Add(new Cell(x, 0, loc));
			cells.Add(MapSides.Left, lCells);
			lCells = new List<ICell>();
			for (int z = 5; z >= 2; z--)
				lCells.Add(new Cell(0, z, loc));
			cells.Add(MapSides.Top, lCells);
			lCells = new List<ICell>();
			for (int x = 5; x >= 2; x--)
				lCells.Add(new Cell(x, 0, loc));
			cells.Add(MapSides.Right, lCells);
		}

		public ICell GetNextCell(ICell cell, MapSides side) 
		{
			int end = GetEnd(side);
			int index = cells[side].FindIndex(c => c == cell);
			if (index >= end)
				return null;
			index++;
			return cells[side][index];
		}

		public ICell GetPreviousCell(ICell cell, MapSides side) 
		{
			int index = cells[side].FindIndex(c => c == cell);
			index--;
			if (index < 0)
				return circle.GetPreviousCell(cell, side);
			return cells[side][index];
		}

		private int GetEnd(MapSides mapSide)
		{
			for(int i = 3; i >= 0; i--)
			{
				if (cells[mapSide][i].tracker == null)
					return i;
			}
			return -1;
		}
	}
}