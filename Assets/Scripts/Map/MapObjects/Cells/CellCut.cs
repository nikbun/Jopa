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
		public MapLocations location { get { return MapLocations.Cut; } set { startCell.location = value; } }
		public Vector3 position { get { return startCell.position; } set { startCell.position = value; } }
		public int exitDistance { get; }

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

		public List<ICell> GetExtra() 
		{
			var extra = new List<ICell>(); 
			foreach (var c in cut)
			{
				extra.Add(new Cell(c, location));
			}
			extra.Add(endCell);
			return extra;
		}
	}
}