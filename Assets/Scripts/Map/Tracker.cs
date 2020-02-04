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

		public bool canHit { get { return location != Location.Jopa && trace.to?.location != Location.Jopa; } }

		public delegate void ShiftMoveDelg(bool withHit);
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
			if (location == Location.Tolchok)
			{
				trace.ResetTrace(saveFrom: true);
				trace = m_Map.GetTolchokTraceToNext(this);
				ShiftMove?.Invoke(true);
			}
			else
			{
				var pos = m_Map.GetJopaPosition(playerPosition);
				trace.ResetTrace(this);
				trace.way.Add(new Trace.Point(pos, Location.Jopa));
				location = Location.Jopa;
				inCircle = false;
				ShiftMove?.Invoke(false);
			}
		}

		public void SetTrace(bool readyStartMoving, Trace trace = null)
		{
			this.readyStartMoving = readyStartMoving;
			this.trace = trace != null ? trace : this.trace;
		}

		public bool CanStartMove(int distance) 
		{
			trace.way.Clear();
			trace.to = null;
			return m_Map.CanMove(this, distance);
		}

		public Trace.Point GetNextTarger() 
		{
			Trace.Point point = null;
			if (HasNextTarget()) 
			{
				point = trace.way[0];
				trace.way.RemoveAt(0);
			}
			if (!inCircle && location == Location.Circle)
				inCircle = true;
			else if (inCircle && location == Location.Origin)
				inCircle = false;
			return point;
		}

		public bool HasNextTarget() 
		{
			return trace.way.Count > 0;
		}
	}
}