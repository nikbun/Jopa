using UnityEngine;
using System.Collections.Generic;

namespace MapSpace.MapObjects
{
	public class Cut
	{
		Cell _start;
		Cell _end;
		List<Cell> _cut = new List<Cell>();

		public Cut(Cell start, Cell end, params Cell[] cut)
		{
			_start = start;
			_end = end;
			_cut.AddRange(cut);
		}

		public List<Cell> GetExtra(Cell cell) 
		{
			if (cell == _start)
			{
				var extra = new List<Cell>();
				extra.AddRange(_cut);
				extra.Add(_end);
				return extra;
			}
			else 
			{
				return null;
			}
		}
	}
}