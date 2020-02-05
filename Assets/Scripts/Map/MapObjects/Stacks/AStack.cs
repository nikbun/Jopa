using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapObjects
{
	/// <summary>
	/// Абстрактный метод для локаций, в которых пешки наклыдываються друг на друга
	/// </summary>
	abstract class AStack
	{
		public Dictionary<MapSides, ICell> cells = new Dictionary<MapSides, ICell>();

		/// <summary>
		/// Проверка на возможность двигаться
		/// Устанавливает трасировку на пешку в случае возможности движения
		/// </summary>
		/// <param name="tracker">Передвигаемая пешка</param>
		/// <param name="steps">Количество шагов</param>
		/// <returns></returns>
		public bool CanMove(Tracker tracker, int steps)
		{
			if (steps != 6)
				return false;
			bool canMove;
			tracker.UpdateWay(GetTarget(tracker, out canMove).GetWay().ToArray());
			return canMove;
		}
		/// <summary>
		/// Получает позицию в зависимости от расположения игрока(игроки снизу, слева, сверху, справа)
		/// </summary>
		/// <param name="mapSide"></param>
		/// <returns></returns>
		public Vector3 GetPosition(MapSides mapSide)
		{
			return GetCell(mapSide).GetWay()[0].position;
		}

		/// <summary>
		/// Получает клетку в зависимости от расположения игрока
		/// </summary>
		/// <param name="mapSide"></param>
		/// <returns></returns>
		public ICell GetCell(MapSides mapSide)
		{
			return cells[mapSide];
		}

		/// <summary>
		/// Получает клетку выхода из стековой локации 
		/// </summary>
		/// <param name="pawn"></param>
		/// <param name="canOccupy"></param>
		/// <returns></returns>
		public abstract ICell GetTarget(Tracker pawn, out bool canOccupy);
	}
}