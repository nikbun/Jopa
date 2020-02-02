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
	public GameObject instancePawns;
	
	List<Pawn> m_Pawns = new List<Pawn>();

	public delegate void EndTurnDeleg();
	public event EndTurnDeleg EndTurn;

	void Start()
	{
		initPawns();
	}

	void initPawns()
	{
		for (int i = 0; i < 4; i++)
		{
			var pawn = Instantiate(instancePawns, transform.position, Quaternion.Euler(90f, 0, 0));
			pawn.transform.SetParent(transform);
			var sPawn = pawn.GetComponent<Pawn>();
			sPawn.number = i;
			sPawn.playerPosition = playerPosition;
			sPawn.StartMove += OffCanMove;
			sPawn.StopMove += EndTurn.Invoke;
			m_Pawns.Add(sPawn);
		}
	}

	public bool CanMovePawns(int steps)
	{
		bool canMove = false;
		foreach (var pawn in m_Pawns)
		{
			canMove = GameData.instance.map.CanMove(pawn, steps) || canMove; 
		}
		return canMove;
	}

	public void OffCanMove()
	{
		foreach (var pawn in m_Pawns)
		{
			pawn.canMove = false;
		}
	}

	public bool IsPawnsEndMove()
	{
		return !m_Pawns.Exists(p => p.move);
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
