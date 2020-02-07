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

		public ICell GetNextCell(ICell cell, MapSides side, bool inCircle = false) 
		{
			int end = GetIndex(0, side);
			int index = cells.FindIndex(c => c == cell);
			if (index == end && inCircle)
				return home.GetNextCell(cell, side);
			index++;
			index %= cells.Count;
			return cells[index];
		}

		public ICell GetPreviousCell(ICell cell, MapSides side) 
		{
			int end = GetIndex(0, side);
			int index = cells.FindIndex(c => c == cell);
			if (index == -1)
				return cells[end];
			index--; // Начальные точки не являються нулями
			return cells[index];
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
		int GetIndex(int index, MapSides mapSide = MapSides.Bottom)
		{
			return (index + shift[mapSide]) % cells.Count;
		}

		void InitCut(int start, int end, List<Vector3> cut)
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