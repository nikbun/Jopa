using UnityEngine;
using System.Collections.Generic;

namespace MapSpace.MapObjects
{
	public class Cell
	{
		public virtual Tracker tracker { get; set; }
		public readonly Map.Locations location;
		public readonly Vector3 position;
		public readonly int exitDistance;

		public Cell(float x, float z, Map.Locations location, int exitDistance = 0)
		{
			position = new Vector3(x, 0, z);
			this.location = location;
			this.exitDistance = exitDistance;
		}
	}
}