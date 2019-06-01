﻿using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public interface ICell
	{
		/// <summary>
		/// Пешка игрока
		/// </summary>
		MapPawn pawn { get; set; }
		/// <summary>
		/// Расположение на карте (Старт, дом, круг, толчек)
		/// </summary>
		Location location { get; set; }
		/// <summary>
		/// Координаты пешки
		/// </summary>
		Vector3 position { get; set; }
		/// <summary>
		/// Может ли пешка занять клетку
		/// </summary>
		/// <param name="pawn">Пешка пытающаяся занять клетку</param>
		/// <param name="lastCell">Полседняя ли это клетка на пути пешки</param>
		/// <returns></returns>
		bool CanOccupy(MapPawn pawn, bool lastCell = false);
		/// <summary>
		/// Получение точек пути
		/// </summary>
		/// <param name="lastCell">Последняя ли это клетка</param>
		/// <returns>Список проходимых точек</returns>
		List<Vector3> GetWay(bool lastCell = false);
	}
}