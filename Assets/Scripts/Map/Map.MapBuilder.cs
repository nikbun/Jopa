using System.Collections.Generic;
using MapSpace.MapObjects;

namespace MapSpace
{
	public partial class Map
	{
		/// <summary>
		/// Строит карту на основе набора предоставленных ячеек
		/// </summary>
		public class MapBuilder 
		{
			Dictionary<int, Cell> _coordinates;
			Cell _quagmire;
			Cell _origin;
			List<Cell> _circle;
			List<Cell> _home;
			List<Fen> _fens = new List<Fen>();
			List<Cut> _cuts = new List<Cut>();

			public MapBuilder(Dictionary<int, Cell> coordinates) 
			{
				_coordinates = coordinates;
			}

			public MapBuilder Quagmire(int index) 
			{
				_quagmire = _coordinates[index];
				return this;
			}

			public MapBuilder Origin(int index)
			{
				_origin = _coordinates[index];
				return this;
			}

			public MapBuilder Circle(params int[] indexes)
			{
				_circle = new List<Cell>();
				foreach (var index in indexes) 
				{
					_circle.Add(_coordinates[index]);
				}
				return this;
			}

			public MapBuilder Home(params int[] indexes)
			{
				_home = new List<Cell>();
				foreach (var index in indexes)
				{
					_home.Add(_coordinates[index]);
				}
				return this;
			}

			public MapBuilder Fen(int entranceIndex, int exitIndex, params int[] fenIndexes)
			{
				var fen = new List<Cell>();
				foreach (var index in fenIndexes) 
				{
					fen.Add(_coordinates[index]);
				}
				_fens.Add(new Fen(_coordinates[entranceIndex], _coordinates[exitIndex], fen.ToArray()));
				return this;
			}

			public MapBuilder Cut(int entranceIndex, int exitIndex, params int[] cutIndexes)
			{
				var cut = new List<Cell>();
				foreach (var index in cutIndexes)
				{
					cut.Add(_coordinates[index]);
				}
				_cuts.Add(new Cut(_coordinates[entranceIndex], _coordinates[exitIndex], cut.ToArray()));
				return this;
			}

			public Map Build() 
			{
				var way = new List<Cell>();
				way.Add(_quagmire);
				way.Add(_origin);
				way.AddRange(_circle);
				way.AddRange(_home);
				return new Map(way, _fens, _cuts);
			}
		}
	}
}
