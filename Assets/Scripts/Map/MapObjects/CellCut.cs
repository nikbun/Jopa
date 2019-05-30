using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class CellCut : ICell
	{
		public MapPawn pawn { get { return startCell.pawn; } set { endCell.SetPawn(value); } }
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

		public void SetEndCell(CellCut endCell)
		{
			this.endCell = endCell;
		}

		public bool CanOccupy(MapPawn pawn, bool lastCell = false)
		{
			return startCell.CanOccupy(pawn) && (!lastCell || endCell.CanOccupyEnd(pawn));
		}

		public bool CanOccupyEnd(MapPawn pawn)
		{
			return startCell.CanOccupy(pawn, true);
		}

		public List<Vector3> GetWay(bool lastCell = false)
		{
			List<Vector3> way = new List<Vector3>() { position };
			if (lastCell)
			{
				way.AddRange(cut);
				way.Add(endCell.position);
			}
			return way;
		}

		public void SetPawn(MapPawn pawn)
		{
			startCell.pawn = pawn;
		}
	}
}