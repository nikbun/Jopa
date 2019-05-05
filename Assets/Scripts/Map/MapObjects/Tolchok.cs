namespace Assets.Scripts.Map.MapObjects
{
	class Tolchok
	{
		Cell firstlevel { get; }
		Cell secondLevel { get; }
		Cell thirdLevel { get; }
		Cell exitTolchok { get; }

		public Tolchok(Cell first, Cell second, Cell third, Cell exit)
		{
			this.firstlevel = first;
			this.secondLevel = second;
			this.thirdLevel = third;
			this.exitTolchok = exit;
		}
	}
}