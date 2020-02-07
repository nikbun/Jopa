using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Cell:ICell
	{
		public Tracker tracker { get; set; }
		public MapLocations location { get; set; }
		public Vector3 position { get; set; }

		public int exitDistance { get; }

		public Cell(float x, float z, MapLocations location, int exitNumber = 0)
		{
			position = new Vector3(x, 0, z);
			this.location = location;
			this.exitDistance = exitNumber;
		}

		public Cell(Vector3 position, MapLocations location) 
		{
			this.position = position;
			this.location = location;
		}
	}
}