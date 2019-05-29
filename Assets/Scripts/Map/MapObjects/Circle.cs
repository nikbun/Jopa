using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Circle
	{
		public Home home;

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

		public bool CanMove(MapPawn pawn, int steps, Trace trace = null, bool back = false)
		{
			if (trace == null)
				trace = new Trace(from: pawn.trace?.from);
			int end = GetIndex(0, pawn.playerPosition);
			int index = back?end+1:cells.FindIndex(c => c.pawn == pawn);
			bool canMove = true;

			while (canMove && steps > 0)
			{
				if (pawn.inGame && index == end && !back)
					return home.CanMove(pawn, steps, trace);
				if (back)
					index = --index + cells.Count;
				else
					index++;
				index %= cells.Count;
				var cell = cells[index];
				canMove = cell.CanOccupy(pawn, steps == 1);
				trace.UpdateTrace(cell);
				steps--;
			}

			pawn.SetTrace(canMove, canMove?trace:null);
			return canMove;
		}

		public Cell GetCell(int cellNumber, PlayerPosition playerPosition)
		{
			return cells[GetIndex(cellNumber, playerPosition)];
		}

		/// <summary>
		/// Возвращает индекс ячейки относительно позиции игрока
		/// </summary>
		/// <param name="index"> Индекс игрока </param>
		/// <param name="playerPosition"> Позиция игрока </param>
		/// <returns> Настоящий индекс </returns>
		public int GetIndex(int index, PlayerPosition playerPosition)
		{
			return (index + shift[playerPosition]) % cells.Count;
		}
	}
}