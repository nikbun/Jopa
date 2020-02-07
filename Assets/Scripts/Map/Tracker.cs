using Map.MapObjects;
using System.Collections.Generic;

namespace Map
{
	/// <summary>
	/// Трекер для работы с картой
	/// </summary>
	public class Tracker
	{
		public MapSides mapSide { get; }
		public MapLocations location { get { return m_CurrentCell.location; } }
		public bool inCircle;
		public bool readyStartMoving;

		public delegate void ShiftMoveDelg();
		public event ShiftMoveDelg ShiftMove;

		GameMap m_Map;
		ICell m_CurrentCell;
		List<ICell> m_Way = new List<ICell>();

		public Tracker(MapSides mapSide, GameMap map) 
		{
			this.mapSide = mapSide;
			m_Map = map;
			m_CurrentCell = map.GetOrigin(mapSide);
		}

		/// <summary>
		/// Обновляет трасировку
		/// </summary>
		/// <param name="toCell">Клетка в которую нужно переместиться</param>
		/// <param name="lastCell">Последняя ли это клетка на пути</param>
		public void UpdateWay(params ICell[] cells)
		{
			m_Way.AddRange(cells);
		}

		public void Shift()
		{
			m_Way.Clear();
			if (location == MapLocations.Tolchok)
			{
				m_Way.Add(m_Map.GetNextCell(m_CurrentCell, mapSide));
			}
			else
			{
				m_Way.Add(m_Map.GetJopa(mapSide));
			}
			ShiftMove?.Invoke();
		}

		public bool CanStartMove(int distance) 
		{
			m_Way.Clear();
			bool canMove = true;
			if (m_CurrentCell.exitDistance > 0) 
			{
				canMove = m_CurrentCell.exitDistance == distance;
				distance = canMove?1:0;
			}
			
			ICell curCell = m_CurrentCell;
			ICell nextCell = null;
			List<ICell> extra = new List<ICell>(); 
			bool rollback = false;
			for (var i = distance; i > 0; i--) 
			{
				if (!rollback)
				{
					nextCell = m_Map.GetNextCell(curCell, mapSide, inCircle);
				}
				else 
				{
					nextCell = m_Map.GetPreviousCell(curCell, mapSide);
				}

				if (nextCell == null && curCell.location == MapLocations.Home) 
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
					
				if (i == 1)// Последняя ячейка за ход
				{
					if (nextCell.location == MapLocations.Cut)
					{
						extra = m_Map.GetExtra(nextCell);
						canMove = nextCell.tracker?.mapSide != mapSide && extra[extra.Count - 1].tracker?.mapSide != mapSide;
					}
					else if (nextCell.location == MapLocations.Tolchok)
					{
						extra = m_Map.GetExtra(nextCell);
						canMove = extra.Exists(e => e.tracker == null) || extra[extra.Count - 1].tracker?.mapSide != mapSide;
						extra = new List<ICell>() { extra[0] };
					}
					else
					{
						canMove = nextCell.tracker?.mapSide != mapSide || nextCell.tracker == this; // Если там не свой
					}
				}
				else 
				{
					canMove = nextCell.tracker == null || nextCell.tracker == this;
				}
				if (!canMove)
					break;
				m_Way.Add(nextCell);
				curCell = nextCell;
			}
			m_Way.AddRange(extra);
			return readyStartMoving = canMove;
		}

		public bool HasNextTarget()
		{
			return m_Way.Count > 0;
		}

		public ICell GetNextTarget() 
		{
			if (!inCircle && location == MapLocations.Circle)
				inCircle = true;
			else if (inCircle && location == MapLocations.Jopa)
				inCircle = false;

			ICell cell = null;
			if (HasNextTarget()) 
			{
				cell = m_Way[0];
				m_Way.RemoveAt(0);
				if (cell != null) 
				{
					if (m_CurrentCell.tracker == this)
						m_CurrentCell.tracker = null;
					if (cell.location != MapLocations.Origin && cell.location != MapLocations.Jopa) 
					{
						cell.tracker?.Shift();
						cell.tracker = this;
					}
					m_CurrentCell = cell;
					
				}
			}
			return cell;
		}
	}
}