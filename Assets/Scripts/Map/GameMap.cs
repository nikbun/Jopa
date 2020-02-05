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

		public ICell GetOrigin(MapSides mapSide)
		{
			return origin.GetCell(mapSide);
		}

		public ICell GetJopa(MapSides mapSide)
		{
			return jopa.GetCell(mapSide);
		}

		/// <summary>
		/// Получает следующую клетку в толчке по трекеру
		/// </summary>
		/// <param name="tracker"></param>
		/// <returns></returns>
		public ICell GetNextTolchok(Tracker tracker)
		{
			return tolchek.GetTolchek(tracker);
		}

		/// <summary>
		/// Определяет может ли пешка ходить на заданное количество шагов
		/// </summary>
		/// <param name="tracker"></param>
		/// <param name="steps"></param>
		/// <returns></returns>
		public bool CanMove(Tracker tracker, int steps)
		{
			switch (tracker.location)
			{
				case MapLocations.Origin:
					return origin.CanMove(tracker, steps);
				case MapLocations.Circle:
					return circle.CanMove(tracker, steps);
				case MapLocations.Home:
					return home.CanMove(tracker, steps);
				case MapLocations.Jopa:
					return jopa.CanMove(tracker, steps);
				case MapLocations.Tolchok:
					return tolchek.CanMove(tracker, steps);
				default:
					return false;
			}
		}
	}

	/// <summary>
	/// Расположения на карте
	/// </summary>
	public enum MapLocations
	{
		Origin,
		Circle,
		Home,
		Jopa,
		Tolchok
	}

	/// <summary>
	/// Стороны карты
	/// </summary>
	public enum MapSides
	{
		Bottom,
		Left,
		Top,
		Right
	}
}