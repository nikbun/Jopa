using UnityEngine;
using System.Collections.Generic;
using Map.MapObjects;

namespace Map
{
	/// <summary>
	/// Класс для передачи пути
	/// </summary>
	public class Trace
	{
		public List<Point> way = new List<Point>();
		public ICell from;
		public ICell to;

		public Trace(ICell from = null)
		{
			this.from = from;
		}

		/// <summary>
		/// Обновляет трасировку
		/// </summary>
		/// <param name="toCell">Клетка в которую нужно переместиться</param>
		/// <param name="lastCell">Последняя ли это клетка на пути</param>
		public void UpdateTrace(ICell toCell, bool lastCell = false)
		{
			to = toCell;
			var points = toCell.GetWay(lastCell);
			for (var i = 0; i < points.Count; i++) 
			{
				bool last = i + 1 == points.Count;
				way.Add(new Point(points[i], toCell.location, last ? toCell : null));
			}
		}

		/// <summary>
		/// Сбрасывает трасировку с начальным значениям
		/// </summary>
		/// <param name="tracker">Пешка, если установлена колечная клетка, устанавливает в нее пешку и делает его начальной</param>
		/// <param name="updateLocation">Обновляет локацию пешки, беря ее из конечной клетки</param>
		/// <param name="saveFrom">Сохранить начальную клетку</param>
		public void ResetTrace(Tracker tracker = null, bool updateLocation = false, bool saveFrom = false)
		{
			if (from != null)
			{
				if (tracker == from.tracker || from.tracker == null)// TODO Костыль для срезов
					from.tracker = null;
			}
			if (!saveFrom)
				from = null;
			if (to != null)
			{
				from = to;
				to = null;
				if (tracker != null)
				{
					from.tracker = tracker;
					if (updateLocation)
						tracker.location = from.location;
				}
			}
			way.Clear();
		}


		public struct Point
		{
			public Vector3 point;
			public Location location;
			public ICell cell;

			public Point(Vector3 point, Location location, ICell cell = null) 
			{
				this.point = point;
				this.location = location;
				this.cell = cell;
			}
		}
	}
}