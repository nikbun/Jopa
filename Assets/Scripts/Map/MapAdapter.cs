using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Map
{
	public class MapAdapter
	{
		Player player { get; }
		PlayerPositions playerPositions { get; }

		public MapAdapter(Player player, GameMap gameMap)
		{
			this.player = player;
			this.playerPositions = player.playerPositions;
		}
	}
}
