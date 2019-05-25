using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapObjects
{
	class Origin
	{
		private Dictionary<PlayerPosition, Cell> originCells = new Dictionary<PlayerPosition, Cell>();

		public Origin()
		{
			var loc = Location.Origin;
			originCells.Add(PlayerPosition.Bottom, new Cell(0, -7.2f, loc));
			originCells.Add(PlayerPosition.Left, new Cell(-7.2f, 0, loc));
			originCells.Add(PlayerPosition.Top, new Cell(0, 7.2f, loc));
			originCells.Add(PlayerPosition.Right, new Cell(7.2f, 0, loc));
		}

		public Vector3 GetPosition(PlayerPosition playerPosition)
		{
			return originCells[playerPosition].position;
		}
	}
}