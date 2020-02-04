using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	/// <summary>
	/// Клетка среза
	/// Если пешка попадает на нее ее переносит на другой конец среза
	/// </summary>
	public class CellCut : ICell
	{
		public Tracker tracker { get { return startCell.tracker; } set { startCell.tracker = value; } }
		public Location location { get { return startCell.location; } set { startCell.location = value; } }
		public Vector3 position { get { return startCell.position; } set { startCell.position = value; } }

		private ICell startCell;
		private List<Vector3> cut;
		private CellCut endCell;

		public CellCut(ICell startCell, List<Vector3> cut = null, CellCut endCell = null)
		{
			this.startCell = startCell;
			this.cut = cut != null ? cut : new List<Vector3>();
			SetEndCell(endCell);
			if (endCell != null)
				endCell.SetEndCell(this);
		}

		/// <summary>
		/// Устанавливает клетку в конец среза
		/// </summary>
		/// <param name="endCell">Конечная клетка среза</param>
		public void SetEndCell(CellCut endCell)
		{
			this.endCell = endCell;
		}

		public bool CanOccupy(Tracker tracker, bool lastCell = false)
		{
			return (lastCell || startCell.CanOccupy(tracker)) && (!lastCell || endCell.CanOccupyEnd(tracker));
		}

		/// <summary>
		/// Проверка на незанятость конечной ячейки
		/// Необходима, для избежания зациклености
		/// </summary>
		/// <param name="tracker"></param>
		/// <returns></returns>
		public bool CanOccupyEnd(Tracker tracker)
		{
			return startCell.CanOccupy(tracker, true);
		}

		public List<Trace.Point> GetWay(bool lastCell = false)
		{
			var way = new List<Trace.Point>() { new Trace.Point(position, location, this) };
			if (lastCell)
			{
				foreach (var c in cut) 
				{
					way.Add(new Trace.Point(c, location));
				}
				way.Add(new Trace.Point(endCell.position, location, endCell));
			}
			return way;
		}

		/// <summary>
		/// Устанавливает пешку в начальную клетку
		/// Так же решает проблемы зацикленности
		/// Устанавливает пешку в начало у конечной клетки
		/// </summary>
		/// <param name="tracker"></param>
		public void SetPawn(Tracker tracker)
		{
			startCell.tracker = tracker;
		}
	}
}