using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Map.MapObjects;

namespace Map
{

	public class GameMap
	{
		private Origin origin = new Origin();
		private Circle circle = new Circle();

		public Vector3 GetOriginPosition(PlayerPosition playerPosition)
		{
			return origin.GetPosition(playerPosition);
		}

		public bool CanMove(MapPawn pawn, int steps)
		{
			return circle.CanMove(pawn, steps);
		}
	}

	public interface MapPawn
	{
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
		Circle
	}
}