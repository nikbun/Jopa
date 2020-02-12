using MapSpace.MapObjects;
using System.Collections.Generic;

namespace MapSpace
{
	/// <summary>
	/// Трекер для работы с картой
	/// </summary>
	public class Tracker
	{
		public Map.Sides side { get; }
		public bool readyStartMoving;

		public delegate void ShiftMoveDelg();
		public event ShiftMoveDelg ShiftMove;

		Map _map;
		Cell _currentCell;
		List<Cell> _way = new List<Cell>();

		public Tracker(Map.Sides side, MapController mapController) 
		{
			this.side = side;
			_map = mapController.GetMap(side);
			_currentCell = _map.GetOrigin();
		}

		void Shift()
		{
			_way.Clear();
			if (_currentCell.location == Map.Locations.Tolchok)
			{
				_way.Add(_map.GetNext(_currentCell));
			}
			else
			{
				_way.Add(_map.GetJopa());
			}
			ShiftMove?.Invoke();
		}

		public bool CanStartMove(int distance) 
		{
			_way.Clear();
			bool canMove = true;
			if (_currentCell.exitDistance > 0) 
			{
				canMove = _currentCell.exitDistance == distance;
				distance = canMove?1:0;
			}
			
			Cell curCell = _currentCell;
			Cell nextCell = null;
			List<Cell> extra = new List<Cell>(); 
			bool rollback = false;
			for (var i = distance; i > 0; i--) 
			{
				// Берем следующую ячейку
				if (!rollback)
				{
					nextCell = _map.GetNext(curCell);
				}
				else 
				{
					nextCell = _map.GetPrevious(curCell);
				}

				// Проверяем на дом
				if (nextCell == null && curCell.location == Map.Locations.Home) 
				{
					if (distance == i) // Если зафиксирована в доме
					{
						canMove = false;
						break;
					}
					else 
					{
						rollback = true;
						i++;
						continue;
					}
				}

				// Строим путь
				if (i == 1)// Последняя ячейка за ход
				{
					if (nextCell.location == Map.Locations.Cut)
					{
						extra = _map.GetExtra(nextCell);
						canMove = nextCell.tracker?.side != side && extra[extra.Count - 1].tracker?.side != side;
					}
					else if (nextCell.location == Map.Locations.Tolchok)
					{
						extra = _map.GetExtra(nextCell);
						canMove = extra.Exists(e => e.tracker == null) || extra[extra.Count - 1].tracker?.side != side;
						extra = nextCell == extra[0]? new List<Cell>() : new List<Cell>() { extra[0] };
					}
					else
					{
						canMove = nextCell.tracker?.side != side || nextCell.tracker == this; // Если там не свой
					}
				}
				else // Не последняя ячейка за ход
				{
					canMove = nextCell.tracker == null || nextCell.tracker == this;
				}
				if (!canMove)
					break;
				_way.Add(nextCell);
				curCell = nextCell;
			}
			_way.AddRange(extra);
			return readyStartMoving = canMove;
		}

		public bool HasNextTarget()
		{
			return _way.Count > 0;
		}

		public Cell GetNextTarget() 
		{
			Cell cell = null;
			if (HasNextTarget()) 
			{
				cell = _way[0];
				_way.RemoveAt(0);
				if (cell != null) 
				{
					if (_currentCell.tracker == this)
						_currentCell.tracker = null;
					if (cell.location != Map.Locations.Origin && cell.location != Map.Locations.Jopa) 
					{
						cell.tracker?.Shift();
						cell.tracker = this;
					}
					_currentCell = cell;
					
				}
			}
			return cell;
		}

		/// <summary>
		/// Убирает трекер с карты
		/// </summary>
		public void DisposeTracker() 
		{
			_currentCell.tracker = null;
		}
	}
}