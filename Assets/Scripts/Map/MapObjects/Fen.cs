using UnityEngine;
using System.Collections.Generic;


namespace MapSpace.MapObjects
{
	public class Fen
	{
		Cell _entrance;
		Cell _exit;
		List<Cell> _cells = new List<Cell>();

		public Fen(Cell entrance, Cell exit, params Cell[] cells)
		{
			_entrance = entrance;
			_exit = exit;
			_cells.AddRange(cells);
		}

		public Cell GetNextCell(Cell cell)
		{
			var index = _cells.IndexOf(cell);
			if (index < 0)
			{
				return null;
			}
			else if (index >= 2)
			{
				return _exit;
			}
			else
			{
				return _cells[++index];
			}
		}

		public List<Cell> GetExtra(Cell cell) 
		{
			var extra = new List<Cell>();
			if (_entrance == cell)
			{
				extra.AddRange(_cells);
				extra.Add(_exit);
				return extra;
			}
			else 
			{
				int index = _cells.IndexOf(cell);
				if (index >= 0)
				{
					for (var i = index; i < _cells.Count; i++)
						extra.Add(_cells[index]);
					extra.Add(_exit);
					return extra;
				}
			}
			return null;
		}
	}
}