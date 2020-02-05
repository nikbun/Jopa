using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Cell:ICell
	{
		public Tracker tracker { get; set; }
		public MapLocations location { get; set; }
		public Vector3 position { get; set; }
		public Cell(float x, float z, MapLocations location)
		{
			position = new Vector3(x, 0, z);
			this.location = location;
		}

		public Cell(Vector3 position, MapLocations location) 
		{
			this.position = position;
			this.location = location;
		}

		public bool CanOccupy(Tracker tracker, bool lastCell = false)
		{
			return this.tracker == null || this.tracker.Equals(tracker)
				|| lastCell && this.tracker?.mapSide != tracker.mapSide;
		}

		public List<ICell> GetWay(bool lastCell = false)
		{
			return new List<ICell>() { this };
		}
	}
}