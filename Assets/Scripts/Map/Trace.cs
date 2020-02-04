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
			way.AddRange(toCell.GetWay(lastCell));
		}

		/// <summary>
		/// Сбрасывает трасировку с начальным значениям
		/// </summary>
		public void ResetTrace()
		{
			to = null;
			way.Clear();
		}


		public class Point
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