﻿using UnityEngine;
using Game.Cells;

namespace Game.MapObjects
{
	class Shortcut
	{
		Cell startCell { get; }
		Cell endCell { get; }
		Vector3[] shortcutPoints { get; }

		public Shortcut(Cell start, Cell end, Vector3[] shortcutPoints)
		{
			startCell = start;
			endCell = end;
			this.shortcutPoints = shortcutPoints;
		}
	}
}