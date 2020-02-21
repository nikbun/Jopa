using MapSpace.MapObjects;
using System.Collections.Generic;

namespace MapSpace
{
	/// <summary>
	/// Трекер для работы с картой
	/// </summary>
	public class Tracker
	{
		Map _map;
		Cell _currentCell;
		List<Cell> _way = new List<Cell>();

		public delegate void ShiftMoveDelg();
		public event ShiftMoveDelg ShiftMove;

		public Map.Sides Side { get; }

		public bool ReadyStartMoving { get; set; }

		public Tracker(Map.Sides side, MapController mapController) 
		{
			Side = side;
			_map = mapController.GetMap(side);
			_currentCell = _map.GetOrigin();
		}

		void Shift()
		{
			_way.Clear();
			if (_currentCell.Location == Map.Locations.Fen)
			{
				_way.Add(_map.GetNext(_currentCell));
			}
			else
			{
				_way.Add(_map.GetQuagmire());
			}
			ShiftMove?.Invoke();
		}

		public bool CanStartMove(int distance) // TODO: Разбить метод на построить путь и узнать можно ли двигаться
		{
			_way.Clear();
			bool canMove = true;
			if (_currentCell.ExitDistance > 0) 
			{
				canMove = _currentCell.ExitDistance == distance;
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
				if (nextCell == null && curCell.Location == Map.Locations.Home) 
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
					if (nextCell.Location == Map.Locations.Cut)
					{
						extra = _map.GetExtra(nextCell);
						canMove = nextCell.Tracker?.Side != Side && extra[extra.Count - 1].Tracker?.Side != Side;
					}
					else if (nextCell.Location == Map.Locations.Fen)
					{
						extra = _map.GetExtra(nextCell);
						canMove = extra.Exists(e => e.Tracker == null) || extra[extra.Count - 1].Tracker?.Side != Side;
						extra = nextCell == extra[0]? new List<Cell>() : new List<Cell>() { extra[0] };
					}
					else
					{
						canMove = nextCell.Tracker?.Side != Side || nextCell.Tracker == this; // Если там не свой
					}
				}
				else // Не последняя ячейка за ход
				{
					canMove = nextCell.Tracker == null || nextCell.Tracker == this;
				}
				if (!canMove)
					break;
				_way.Add(nextCell);
				curCell = nextCell;
			}
			_way.AddRange(extra);
			return ReadyStartMoving = canMove;
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
					if (_currentCell.Tracker == this)
						_currentCell.Tracker = null;
					if (cell.Location != Map.Locations.Origin && cell.Location != Map.Locations.Quagmire) 
					{
						cell.Tracker?.Shift();
						cell.Tracker = this;
					}
					_currentCell = cell;
					
				}
			}
			return cell;
		}

		public bool IsHome() 
		{
			return _currentCell.Location == Map.Locations.Home;
		}

		/// <summary>
		/// Убирает трекер с карты
		/// </summary>
		public void DisposeTracker() 
		{
			_currentCell.Tracker = null;
		}
	}
}