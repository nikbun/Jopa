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

		public bool CanMove(MapPawn pawn, int steps)
		{
			int end = GetRealIndex(0, pawn.playerPosition);
			Trace trace = new Trace(new List<Vector3>(), pawn.trace?.from, null);
			int iFrom = cells.FindIndex(c => c.pawn == pawn);
			int iTo = (iFrom + steps);
			bool canMove = true;
			for (int i = iFrom + 1; i <= iTo && canMove; i++)
			{
				if (pawn.inGame && (end == iFrom || i - 1 == end))
					return home.CanMove(pawn, iTo - i + 1, trace);
				trace.to = cells[i % cells.Count];
				canMove = trace.to.CanOccupy(pawn);
				trace.way.Add(trace.to.position);
			}

			if (canMove)
			{
				pawn.trace = trace;
				pawn.canMove = canMove;
			}
			return canMove;
		}

		public bool CanMoveBack(MapPawn pawn, int steps, Trace trace)
		{
			int iFrom = cells.Count + GetRealIndex(0, pawn.playerPosition);
			int iTo = (iFrom - steps);
			bool canMove = true;
			for (int i = iFrom; i > iTo && canMove; i--)
			{
				trace.to = cells[i % cells.Count];
				canMove = trace.to.CanOccupy(pawn);
				trace.way.Add(trace.to.position);
			}

			if (canMove)
			{
				pawn.trace = trace;
				pawn.canMove = canMove;
			}
			return canMove;
		}

		public Cell GetCell(int cellNumber, PlayerPosition playerPosition)
		{
			return cells[GetRealIndex(cellNumber, playerPosition)];
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