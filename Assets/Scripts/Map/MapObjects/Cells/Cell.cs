using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Cell:ICell
	{
		public Tracker tracker { get; set; }
		public Location location { get; set; }
		public Vector3 position { get; set; }
		public Cell(float x, float z, Location location)
		{
			position = new Vector3(x, 0, z);
			this.location = location;
		}

		public bool CanOccupy(Tracker tracker, bool lastCell = false)
		{
			return this.tracker == null || this.tracker.Equals(tracker)
				|| lastCell && this.tracker?.playerPosition != tracker.playerPosition;
		}

		public List<Vector3> GetWay(bool lastCell = false)
		{
			return new List<Vector3>() { position };
		}
	}
}