using UnityEngine;

namespace Map.MapObjects
{
	public class Cell
	{
		public MapPawn pawn;
		public Location location;
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
	}
}