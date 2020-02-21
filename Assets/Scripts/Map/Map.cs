using System.Collections.Generic;
using MapSpace.MapObjects;

namespace MapSpace
{
	public partial class Map
	{
		List<Cell> _way;
		List<Fen> _fens;
		List<Cut> _cuts;

		Map(List<Cell> way, List<Fen> fens, List<Cut> cuts)
		{
			_way = way;
			_fens = fens;
			_cuts = cuts;
		}

		public Cell GetQuagmire() 
		{
			return _way[0];
		}

		public Cell GetOrigin() 
		{
			return _way[1];
		}

		/// <summary>
		/// Получить следующую ячейку
		/// </summary>
		/// <param name="cell">Текущая ячейка</param>
		/// <returns></returns>
		public Cell GetNext(Cell cell) 
		{
			var index = _way.IndexOf(cell);
			index++;
			if (index <= GetEndIndex()) 
			{
				if (index > 0)
				{
					return _way[index];
				}
				else
				{
					foreach (var fen in _fens)
					{
						var nextCell = fen.GetNextCell(cell);
						if (nextCell != null)
							return nextCell;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Получить предыдущую ячейку
		/// </summary>
		/// <param name="cell">Текущая ячейка</param>
		/// <returns></returns>
		public Cell GetPrevious(Cell cell)
		{
			var index = _way.IndexOf(cell);
			if (index > 0)
			{
				index--;
				return _way[index];
			}
			else 
			{
				return null;
			}
		}

		/// <summary>
		/// Получить дополнительные, специальные ячейки
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		public List<Cell> GetExtra(Cell cell) 
		{
			List<Cell> extra;
			switch (cell.Location) 
			{
				case Locations.Fen:
					foreach (var fen in _fens)
					{
						extra = fen.GetExtra(cell);
						if (extra != null)
							return extra;
					}
					break;
				case Locations.Cut:
					foreach (var cut in _cuts)
					{
						extra = cut.GetExtra(cell);
						if (extra != null)
							return extra;
					}
					break;
			}
			return null;
		}

		/// <summary>
		/// Получить индекс последней пустой ячейки
		/// </summary>
		/// <returns></returns>
		int GetEndIndex()
		{
			int end = _way.Count - 1;
			while (_way[end].Tracker != null)
			{
				end--;
			}
			return end;
		}


		/// <summary>
		/// Расположения на карте
		/// </summary>
		public enum Locations
		{
			Origin,
			Circle,
			Home,
			Quagmire,
			Fen,
			Cut
		}

		/// <summary>
		/// Стороны карты
		/// </summary>
		public enum Sides
		{
			Bottom,
			Left,
			Top,
			Right
		}
	}
}
