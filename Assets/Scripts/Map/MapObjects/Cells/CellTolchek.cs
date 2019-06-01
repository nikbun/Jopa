using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	/// <summary>
	/// Клетка входа в толчек
	/// </summary>
	public class CellTolchek:ICell
	{
		public MapPawn pawn
		{
			get { return null; }
			set {
				value.trace.from = tolchekCell;
				tolchekCell.pawn = value;
			}
		}
		public Location location { get { return tolchekCell.location; } set { } }
		public Vector3 position { get; set; }

		private ICell tolchekCell { get { return tolchek?.GetCell(0); } }
		private Tolchok tolchek;

		public CellTolchek(Vector3 position, Tolchok tolchek)
		{
			this.position = position;
			this.tolchek = tolchek;
		}

		public bool CanOccupy(MapPawn pawn, bool lastCell = false)
		{
			return !lastCell || lastCell && tolchek.CanMove(0);
		}

		public List<Vector3> GetWay(bool lastCell = false)
		{
			
			var pos = new List<Vector3>() { position };
			if (lastCell)
				pos.Add(tolchekCell.position);
			return pos;
		}
	}
}