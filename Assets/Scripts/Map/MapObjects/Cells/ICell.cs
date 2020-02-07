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
	}
}