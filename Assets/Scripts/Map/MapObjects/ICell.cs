using UnityEngine;
using System.Collections.Generic;

namespace Map.MapObjects
{
	public interface ICell
	{
		MapPawn pawn { get; set; }
		Location location { get; set; }
		Vector3 position { get; set; }
		bool CanOccupy(MapPawn pawn, bool lastCell = false);

		List<Vector3> GetWay(bool lastCell = false);
	}
}