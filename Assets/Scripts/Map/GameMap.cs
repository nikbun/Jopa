using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Map.MapObjects;

namespace Map
{

	public class GameMap
	{
		private Circle circle;
		private Origin origin;
		private Home home;
		private Jopa jopa;

		public GameMap()
		{
			circle = new Circle();
			origin = new Origin(circle);
			home = new Home(circle);
			jopa = new Jopa(origin);
		}

		public Vector3 GetOriginPosition(PlayerPosition playerPosition)
		{
			return origin.GetPosition(playerPosition);
		}

		public Vector3 GetJopaPosition(PlayerPosition playerPosition)
		{
			return jopa.GetPosition(playerPosition);
		}

		public bool CanMove(MapPawn pawn, int steps)
		{
			switch (pawn.location)
			{
				case Location.Origin:
					return origin.CanMove(pawn, steps);
				case Location.Circle:
					return circle.CanMove(pawn, steps);
				case Location.Home:
					return home.CanMove(pawn, steps);
				case Location.Jopa:
					return jopa.CanMove(pawn, steps);
				default:
					return false;
			}
		}
	}

	public interface MapPawn
	{
		bool inGame { get; set; }
		bool canMove { get; set; }
		Trace trace { get; set; }
		// Расположение фишки на карте
		Location location { get; set; }
		// Позиция игрока
		PlayerPosition playerPosition { get; }

		void Shift();

		void SetTrace(bool canMove, Trace trace = null);
	}

	/// <summary>
	/// Класс для передачи пути
	/// </summary>
	public class Trace
	{
		public List<Vector3> way;
		public ICell from;
		public ICell to;

		public Trace(List<Vector3> way = null, ICell from = null, ICell to = null)
		{
			if (way == null)
				way = new List<Vector3>();
			this.way = way;
			this.from = from;
			this.to = to;
		}

		public void UpdateTrace(ICell cell, bool lastCell = false)
		{
			to = cell;
			way.AddRange(cell.GetWay(lastCell));
		}

		public void ResetTrace(MapPawn pawn = null, bool updateLocation = false)
		{
			if (from != null && pawn != null)
				from.pawn = null;
			from = null;
			if (to != null)
			{
				from = to;
				to = null;
				if (pawn != null)
				{
					from.pawn = pawn;
					if (updateLocation)
						pawn.location = from.location;
				}
			}
			if (way != null)
				way.Clear();
			else
				way = new List<Vector3>();
		}
	}

	public enum Location
	{
		Origin,
		Circle,
		Home,
		Jopa
	}
}