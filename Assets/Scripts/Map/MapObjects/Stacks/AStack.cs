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
		/// Получает клетку в зависимости от расположения игрока
		/// </summary>
		/// <param name="mapSide"></param>
		/// <returns></returns>
		public ICell GetCell(MapSides mapSide)
		{
			return cells[mapSide];
		}

		public abstract ICell GetNextCell(MapSides side);
	}
}