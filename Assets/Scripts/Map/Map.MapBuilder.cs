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
			Cell _jopa;
			Cell _origin;
			List<Cell> _circle;
			List<Cell> _home;
			List<Tolchok> _tolchoks = new List<Tolchok>();
			List<Cut> _cuts = new List<Cut>();

			public MapBuilder(Dictionary<int, Cell> coordinates) 
			{
				_coordinates = coordinates;
			}

			public MapBuilder Jopa(int index) 
			{
				_jopa = _coordinates[index];
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

			public MapBuilder Tolchok(int entranceIndex, int exitIndex, params int[] tolchockIndexes)
			{
				var tolcheck = new List<Cell>();
				foreach (var index in tolchockIndexes) 
				{
					tolcheck.Add(_coordinates[index]);
				}
				_tolchoks.Add(new Tolchok(_coordinates[entranceIndex], _coordinates[exitIndex], tolcheck.ToArray()));
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
				way.Add(_jopa);
				way.Add(_origin);
				way.AddRange(_circle);
				way.AddRange(_home);
				return new Map(way, _tolchoks, _cuts);
			}
		}
	}
}
