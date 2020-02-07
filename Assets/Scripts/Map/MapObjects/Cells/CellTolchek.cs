using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	/// <summary>
	/// Клетка входа в толчек
	/// </summary>
	public class CellTolchek:ICell
	{
		public Tracker tracker { get; set; }
		public MapLocations location { get { return tolchekCell.location; } set { } }
		public Vector3 position { get; set; }
		public int exitDistance { get; }

		private ICell tolchekCell { get { return tolchek?.GetCell(0); } }
		private Tolchok tolchek;

		public CellTolchek(Vector3 position, Tolchok tolchek)
		{
			this.position = position;
			this.tolchek = tolchek;
		}

		public bool CanOccupy(Tracker tracker, bool lastCell = false)
		{
			return !lastCell || lastCell && tolchek.CanMove(0);
		}

		public List<ICell> GetWay(bool lastCell = false)
		{
			
			var pos = new List<ICell>() { this };
			if (lastCell)
				pos.Add(tolchekCell);
			return pos;
		}
	}
}