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
		public Location location { get { return tolchekCell.location; } set { } }
		public Vector3 position { get; set; }

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

		public List<Trace.Point> GetWay(bool lastCell = false)
		{
			
			var pos = new List<Trace.Point>() { new Trace.Point(position, location, this) };
			if (lastCell)
				pos.Add( new Trace.Point(tolchekCell.position, location, tolchekCell) );
			return pos;
		}
	}
}