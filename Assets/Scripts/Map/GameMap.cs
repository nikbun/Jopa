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

		public GameMap()
		{
			circle = new Circle();
			origin = new Origin(circle);
			home = new Home(circle);
		}

		public Vector3 GetOriginPosition(PlayerPosition playerPosition)
		{
			return origin.GetPosition(playerPosition);
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
	}

	/// <summary>
	/// Класс для передачи пути
	/// </summary>
	public class Trace
	{
		public List<Vector3> way;
		public Cell from;
		public Cell to;

		public Trace(List<Vector3> way, Cell from, Cell to)
		{
			this.way = way;
			this.from = from;
			this.to = to;
		}
	}

	public enum Location
	{
		Origin,
		Circle,
		Home
	}
}