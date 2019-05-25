using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Circle
	{
		private List<Cell> cells = new List<Cell>();
		private Dictionary<PlayerPosition, int> shift = new Dictionary<PlayerPosition, int>();

		public Circle()
		{
			var loc = Location.Circle;
			for (int x = 6; x > -6; x--)
				cells.Add(new Cell(x, -6, loc));
			for (int y = -6; y < 6; y++)
				cells.Add(new Cell(-6, y, loc));
			for (int x = -6; x < 6; x++)
				cells.Add(new Cell(x, 6, loc));
			for (int y = 6; y > -6; y--)
				cells.Add(new Cell(6, y, loc));

			shift.Add(PlayerPosition.Bottom, 6);
			shift.Add(PlayerPosition.Left, 18);
			shift.Add(PlayerPosition.Top, 30);
			shift.Add(PlayerPosition.Right, 42);
		}

		public bool CanMove(MapPawn pawn, int steps)
		{
			Debug.Log("Пешка может ходить на " + steps + " шагов?");
			bool canMove = false;
			List<Vector3> way = new List<Vector3>();
			Cell to = new Cell();

			switch (pawn.location)
			{
				case Location.Origin:
					canMove = steps == 6;
					if (canMove)
					{
						to = cells[GetRealIndex(0, pawn.playerPosition)];
						canMove = to.CanOccupy(pawn);
						way.Add(to.position);
					}
					break;

				case Location.Circle:
					int iFrom = cells.FindIndex(c => c.pawn == pawn) + 1;
					int iTo = (iFrom + steps);
					canMove = true;
					for (int i = iFrom; i < iTo && canMove; i++)
					{
						to = cells[i%cells.Count];
						canMove = to.CanOccupy(pawn);
						way.Add(to.position);
					}
					Debug.Log("Поиск ячеек от " + iFrom + " до " + iTo);
					break;
			}

			if (canMove)
			{
				pawn.trace = new Trace(way, pawn.trace?.from, to);
				pawn.canMove = canMove;
				Debug.Log("Пешка может ходить.");
			}
			return canMove;
		}


		/// <summary>
		/// Возвращает индекс ячейки относительно позиции игрока
		/// </summary>
		/// <param name="index"> Индекс игрока </param>
		/// <param name="playerPosition"> Позиция игрока </param>
		/// <returns> Настоящий индекс </returns>
		public int GetRealIndex(int index, PlayerPosition playerPosition)
		{
			return (index + shift[playerPosition]) % cells.Count;
		}
	}
}