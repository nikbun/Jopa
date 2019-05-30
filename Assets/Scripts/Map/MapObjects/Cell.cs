using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public class Cell:ICell
	{
		public MapPawn pawn { get; set; }
		public Location location { get; set; }
		public Vector3 position { get; set; }

		public Cell(){}

		public Cell(float x, float z, Location location)
		{
			position = new Vector3(x, 0, z);
			this.location = location;
		}

		public bool CanOccupy(MapPawn pawn, bool lastCell = false)
		{
			return this.pawn == null || this.pawn.Equals(pawn)
				|| lastCell && this.pawn?.playerPosition != pawn.playerPosition;
		}

		public List<Vector3> GetWay(bool lastCell = false)
		{
			return new List<Vector3>() { position };
		}
	}
}