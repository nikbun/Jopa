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
		/// <param name="pawn">Пешка, если установлена колечная клетка, устанавливает в нее пешку и делает его начальной</param>
		/// <param name="updateLocation">Обновляет локацию пешки, беря ее из конечной клетки</param>
		/// <param name="saveFrom">Сохранить начальную клетку</param>
		public void ResetTrace(MapPawn pawn = null, bool updateLocation = false, bool saveFrom = false)
		{
			if (from != null)
			{
				if (pawn == from.pawn)
					from.pawn = null;
			}
			if (!saveFrom)
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
}