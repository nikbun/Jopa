using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts
{
	public class Player
	{
		public PlayerPositions playerPositions { get; }
		public MapAdapter mapAdapter { get; }
		public Pawn[] pawns { get; set; }

		public Player(PlayerPositions playerPositions, Pawn[] pawns)
		{
			this.pawns = pawns;
			this.playerPositions = playerPositions;
			mapAdapter = new MapAdapter(this, GlobalGameData.GameController.gameMap);
		}
	}
}
