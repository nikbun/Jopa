using UnityEngine;

namespace Assets.Scripts.Map.MapObjects
{
	class Cell
	{
		public Vector3 position { get; set; }
		public bool busy { get; set; }

		public Cell(Vector3 position)
		{
			this.position = position;
		}

		public Cell(float x, float y)
		{
			this.position = new Vector3(x, 0, y);
		}
	}
}