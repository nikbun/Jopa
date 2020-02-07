using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public interface ICell
	{
		/// <summary>
		/// Пешка игрока
		/// </summary>
		Tracker tracker { get; set; }
		/// <summary>
		/// Расположение на карте (Старт, дом, круг, толчек)
		/// </summary>
		MapLocations location { get; set; }
		/// <summary>
		/// Координаты пешки
		/// </summary>
		Vector3 position { get; set; }
		/// <summary>
		/// Нужно число(которое выпало на кубике), чтобы перейти на следующую ячейку
		/// 0 - Число не нужно
		/// </summary>
		int exitDistance { get; }
		/// <summary>
		/// Может ли пешка занять клетку
		/// </summary>
		/// <param name="tracker">Пешка пытающаяся занять клетку</param>
		/// <param name="lastCell">Полседняя ли это клетка на пути пешки</param>
		/// <returns></returns>
		bool CanOccupy(Tracker tracker, bool lastCell = false);
		/// <summary>
		/// Получение точек пути
		/// </summary>
		/// <param name="lastCell">Последняя ли это клетка</param>
		/// <returns>Список проходимых точек</returns>
		List<ICell> GetWay(bool lastCell = false);
	}
}