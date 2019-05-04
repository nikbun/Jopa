using UnityEngine;
using Game.MapObjects;

namespace Game.Cells
{
	class OuterCircleCell : Cell
	{
		public bool hasShortcut { get { return shortcut != null; } }
		public bool hasTolchok { get { return tolchok != null; } }

		public Tolchok tolchok { get; set; }
		public Shortcut shortcut { get; set; }

		public OuterCircleCell(Vector3 position) : base(position) {}
		public OuterCircleCell(float x, float y) : base(x, y) { }
	}
}