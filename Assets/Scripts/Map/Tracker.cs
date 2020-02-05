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
				m_Way.Add(m_Map.GetNextTolchok(this));
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
			return readyStartMoving = m_Map.CanMove(this, distance);
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