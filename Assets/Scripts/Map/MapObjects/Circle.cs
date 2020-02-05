using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Circle
	{
		public Home home;

		private List<ICell> cells = new List<ICell>();
		private Dictionary<MapSides, int> shift = new Dictionary<MapSides, int>(); // Смещение относительно позиции игрока

		public Circle()
		{
			var loc = MapLocations.Circle;
			for (int x = 6; x > -6; x--)
				cells.Add(new Cell(x, -6, loc));
			for (int y = -6; y < 6; y++)
				cells.Add(new Cell(-6, y, loc));
			for (int x = -6; x < 6; x++)
				cells.Add(new Cell(x, 6, loc));
			for (int y = 6; y > -6; y--)
				cells.Add(new Cell(6, y, loc));

			InitCut(8,	16,	new List<Vector3>() { new Vector3(-2, 0, -4.5f), new Vector3(-4.5f, 0, -2) });
			InitCut(20, 28, new List<Vector3>() { new Vector3(-4.5f, 0, 2f), new Vector3(-2, 0, 4.5f) });
			InitCut(32, 40, new List<Vector3>() { new Vector3(2f, 0, 4.5f), new Vector3(4.5f, 0, 2f) });
			InitCut(44, 4,	new List<Vector3>() { new Vector3(4.5f, 0, -2f), new Vector3(2f, 0, -4.5f) });

			shift.Add(MapSides.Bottom, 6);
			shift.Add(MapSides.Left, 18);
			shift.Add(MapSides.Top, 30);
			shift.Add(MapSides.Right, 42);
		}

		public bool CanMove(Tracker tracker, int steps, bool back = false)
		{
			int end = GetIndex(0, tracker.mapSide);
			int index = back?end+1:cells.FindIndex(c => c.tracker == tracker);
			bool canMove = true;

			while (canMove && steps > 0)
			{
				if (tracker.inCircle && index == end && !back)
					return home.CanMove(tracker, steps);
				if (back)
					index = --index + cells.Count;
				else
					index++;
				index %= cells.Count;
				var cell = cells[index];
				canMove = cell.CanOccupy(tracker, steps == 1);
				tracker.UpdateWay(cell.GetWay(steps == 1).ToArray());
				steps--;
			}
			return canMove;
		}

		public ICell GetCell(int cellNumber, MapSides mapSide = MapSides.Bottom)
		{
			return cells[GetIndex(cellNumber, mapSide)];
		}

		/// <summary>
		/// Возвращает индекс ячейки относительно позиции игрока
		/// </summary>
		/// <param name="index"> Индекс игрока </param>
		/// <param name="mapSide"> Позиция игрока </param>
		/// <returns> Настоящий индекс </returns>
		public int GetIndex(int index, MapSides mapSide = MapSides.Bottom)
		{
			return (index + shift[mapSide]) % cells.Count;
		}

		public void SetTolchek(Tolchok tolchek, int index)
		{
			index = GetIndex(index);
			cells[index] = new CellTolchek(cells[index].position, tolchek);
		}

		private void InitCut(int start, int end, List<Vector3> cut)
		{
			var startCell = cells[start];
			var endCell = cells[end];
			CellCut sCellCut = new CellCut(startCell, cut);
			List<Vector3> cutReverse = new List<Vector3>();
			foreach (var pos in cut)
			{
				cutReverse.Add(pos);
			}
			cutReverse.Reverse();
			CellCut eCellCut = new CellCut(endCell, cutReverse, sCellCut);
			cells[start] = sCellCut;
			cells[end] = eCellCut;
		}
	}
}