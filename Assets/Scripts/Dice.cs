using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
	public class Dice
	{
		private int lastNumber;

		public Dice()
		{
			lastNumber = 1;
		}

		/// <summary>
		/// Бросить кость
		/// </summary>
		/// <returns>Число от 1 до 6</returns>
		public int Throw()
		{
			lastNumber = Random.Range(1, 7);
			return lastNumber;
		}

		/// <summary>
		/// получить последнне выбрашенное число
		/// </summary>
		/// <returns></returns>
		public int GetLastNumber()
		{
			return lastNumber;
		}
	}
}