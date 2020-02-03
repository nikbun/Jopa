namespace Map
{
	/// <summary>
	/// Пешка созданная для работы с картой
	/// </summary>
	public class Tracker
	{
		public bool inGame { get; set; }
		public bool readyStartMoving { get; set; }
		public Trace trace { get; set; }
		public Location location { get; set; }
		public PlayerPosition playerPosition { get; }

		public delegate void ShiftMoveDelg(bool withHit);
		public event ShiftMoveDelg ShiftMove;

		public Tracker(PlayerPosition playerPosition) 
		{
			this.playerPosition = playerPosition;
			trace = GameData.instance.map.GetStartTrace(playerPosition);
			location = Location.Origin;
		}

		public void Shift()
		{
			if (location == Location.Tolchok)
			{
				trace.ResetTrace(saveFrom: true);
				trace = GameData.instance.map.GetTolchokTraceToNext(this);
				ShiftMove?.Invoke(true);
			}
			else
			{
				var pos = GameData.instance.map.GetJopaPosition(playerPosition);
				trace.ResetTrace(this);
				trace.way.Add(new Trace.Point(pos, Location.Jopa));
				location = Location.Jopa;
				inGame = false;
				ShiftMove?.Invoke(false);
			}
		}

		public void SetTrace(bool canMove, Trace trace = null)
		{
			this.readyStartMoving = canMove;
			this.trace = trace != null ? trace : this.trace;
		}
	}
}