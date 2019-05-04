using Game.Cells;

namespace Game.MapObjects
{
	class Home
	{
		public Cell[] homeCells { get; }

		public Home(Cell first, Cell second, Cell third, Cell fourth)
		{
			homeCells = new[] { first, second, third, fourth };
		}
	}
}