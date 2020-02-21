namespace MapSpace.MapObjects
{
	/// <summary>
	/// Ячейка с ячейкой внутри, нужна чтобы разделить ячейки, но создать общий трекер
	/// Необходим в случае пересечения двух ячеек в одной точке
	/// </summary>
	public sealed class CellNested : Cell
	{
		Cell _innerCell;

		public override Tracker Tracker { get { return _innerCell.Tracker; } set { _innerCell.Tracker = value; } }

		public CellNested(Cell innerCell) 
			: base(innerCell.Position.x, innerCell.Position.z, innerCell.Location, innerCell.ExitDistance)
		{
			_innerCell = innerCell;
		}
	}
}