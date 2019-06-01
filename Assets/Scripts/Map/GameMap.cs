using UnityEngine;
using System.Collections;
using Map.MapObjects;

namespace Map
{

	public class GameMap
	{
		private Circle circle;
		private Origin origin;
		private Home home;
		private Jopa jopa;
		private TolchokDistributor tolchek;

		/// <summary>
		/// Игровая карта
		/// Загружает локации и позволяет пработать с ними
		/// </summary>
		public GameMap()
		{
			circle = new Circle();
			origin = new Origin(circle);
			home = new Home(circle);
			jopa = new Jopa(origin);
			tolchek = new TolchokDistributor(circle);
		}

		public Vector3 GetOriginPosition(PlayerPosition playerPosition)
		{
			return origin.GetPosition(playerPosition);
		}

		public Vector3 GetJopaPosition(PlayerPosition playerPosition)
		{
			return jopa.GetPosition(playerPosition);
		}

		/// <summary>
		/// Получает трасировку к следующей клетке в толчке
		/// </summary>
		/// <param name="pawn">Пешка находящаяся в толчке</param>
		/// <returns></returns>
		public Trace GetTolchokTraceToNext(MapPawn pawn)
		{
			return tolchek.GetTrace(pawn);
		}

		/// <summary>
		/// Определяет может ли пешка ходить на заданное количество шагов
		/// </summary>
		/// <param name="pawn"></param>
		/// <param name="steps"></param>
		/// <returns></returns>
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
				case Location.Tolchok:
					return tolchek.CanMove(pawn, steps);
				default:
					return false;
			}
		}
	}

	public enum Location
	{
		Origin,
		Circle,
		Home,
		Jopa,
		Tolchok
	}
}