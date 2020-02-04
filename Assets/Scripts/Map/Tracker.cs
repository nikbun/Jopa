namespace Map
{
	/// <summary>
	/// Пешка созданная для работы с картой
	/// </summary>
	public class Tracker
	{
		GameMap m_Map;

		public Trace trace { get; set; }
		public Location location { get; set; }
		public PlayerPosition playerPosition { get; }
		public bool readyStartMoving { get; set; }
		public bool inCircle { get; set; }

		public delegate void ShiftMoveDelg();
		public event ShiftMoveDelg ShiftMove;

		public Tracker(PlayerPosition playerPosition, GameMap map) 
		{
			this.playerPosition = playerPosition;
			m_Map = map;
			trace = map.GetStartTrace(playerPosition);
			location = Location.Origin;
		}

		public void Shift()
		{
			trace.ResetTrace();
			if (location == Location.Tolchok)
			{
				trace = m_Map.GetTolchokTraceToNext(this);
			}
			else
			{
				var pos = m_Map.GetJopaPosition(playerPosition);
				trace.way.Add(new Trace.Point(pos, Location.Jopa));
				location = Location.Jopa;
			}
			ShiftMove?.Invoke();
		}

		public bool CanStartMove(int distance) 
		{
			trace.ResetTrace();
			return readyStartMoving = m_Map.CanMove(this, distance);
		}

		public Trace.Point GetNextTarger() 
		{
			if (!inCircle && location == Location.Circle)
				inCircle = true;
			else if (inCircle && location == Location.Jopa)
				inCircle = false;

			Trace.Point point = null;
			if (HasNextTarget()) 
			{
				point = trace.way[0];
				trace.way.RemoveAt(0);
				location = point.location;
				if (point.cell != null) 
				{
					if (trace.from.tracker == this)
						trace.from.tracker = null;
					if (point.cell.location != Location.Origin && point.cell.location != Location.Jopa) 
					{
						point.cell.tracker?.Shift();
						point.cell.tracker = this;
					}
					trace.from = point.cell;
					
				}
			}
			return point;
		}

		public bool HasNextTarget() 
		{
			return trace.way.Count > 0;
		}
	}
}