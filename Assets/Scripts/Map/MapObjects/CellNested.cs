namespace MapSpace.MapObjects
{
	/// <summary>
	/// Ячейка с ячейкой внутри, нужна чтобы разделить ячейки, но создать общий трекер
	/// Необходим в случае пересечения двух ячеек в одной точке
	/// </summary>
	public class CellNested : Cell
	{
		public override Tracker tracker { get { return _innerCell.tracker; } set { _innerCell.tracker = value; } }
		Cell _innerCell;
		public CellNested(Cell innerCell) 
			: base(innerCell.position.x, innerCell.position.z, innerCell.location, innerCell.exitDistance)
		{
			_innerCell = innerCell;
		}
	}
}