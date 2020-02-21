using UnityEngine;
using System.Collections.Generic;

namespace MapSpace.MapObjects
{
	public class Cell
	{
		public virtual Tracker Tracker { get; set; }

		public Map.Locations Location { get; }

		public Vector3 Position { get; }

		public int ExitDistance { get; }

		public Cell(float x, float z, Map.Locations location, int exitDistance = 0)
		{
			Position = new Vector3(x, 0, z);
			Location = location;
			ExitDistance = exitDistance;
		}
	}
}