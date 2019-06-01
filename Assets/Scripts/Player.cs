using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public PlayerPosition playerPosition;
	public GameObject pawnInstance;
	public List<Pawn> pawns = new List<Pawn>();
	public GameMap gameMap;

	private void Start()
	{
		gameMap = GameController.instance.gameMap;
		initPawns();
	}

	void initPawns()
	{
		for (int i = 0; i < 4; i++)
		{
			var pawn = Instantiate(pawnInstance, transform.position, Quaternion.Euler(90f, 0, 0));
			pawn.transform.SetParent(transform);
			var sPawn = pawn.GetComponent<Pawn>();
			sPawn.player = this;
			sPawn.number = i;
			pawns.Add(sPawn);
		}
	}

	public bool CanMovePawns(int steps)
	{
		bool canMove = false;
		foreach (var pawn in pawns)
		{
			canMove = gameMap.CanMove(pawn, steps) || canMove; 
		}
		return canMove;
	}

	public void OffCanMove()
	{
		foreach (var pawn in pawns)
		{
			pawn.canMove = false;
		}
	}

	public bool IsPawnsEndMove()
	{
		return !pawns.Exists(p => p.move);
	}
}

/// <summary>
/// Расположение игроков на карте
/// </summary>
public enum PlayerPosition
{
	Bottom,
	Left,
	Top,
	Right
}
