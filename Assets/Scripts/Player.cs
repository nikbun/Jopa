using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public GameObject instancePawns;
	public PlayerPosition playerPosition;

	public delegate void EndTurnDeleg();
	public event EndTurnDeleg EndTurn;

	List<Pawn> m_Pawns = new List<Pawn>();

	void Start()
	{
		InitPawns();
	}

	void InitPawns()
	{
		for (int i = 0; i < 4; i++)
		{
			var pawn = Instantiate(instancePawns, transform.position, Quaternion.Euler(90f, 0, 0));
			pawn.transform.SetParent(transform);
			var sPawn = pawn.GetComponent<Pawn>();
			sPawn.playerPosition = playerPosition;
			sPawn.StartMove += OffCanMove;
			sPawn.StopMove += EndTurn.Invoke;
			m_Pawns.Add(sPawn);
		}
	}

	/// <summary>
	/// Может ли игрок начать ход
	/// </summary>
	/// <param name="diceResult">Результат сброса кубика</param>
	/// <returns></returns>
	public bool CanStartMove(int diceResult)
	{
		bool canMove = false;
		foreach (var pawn in m_Pawns)
		{
			canMove = pawn.CanStartMove(diceResult) || canMove; 
		}
		return canMove;
	}

	/// <summary>
	/// Закончил ли игрок ход
	/// </summary>
	/// <returns></returns>
	public bool IsEndMove()
	{
		return !m_Pawns.Exists(p => p.IsMoving());
	}

	/// <summary>
	/// Отключить подсветку фишек, которыми можно ходить
	/// </summary>
	void OffCanMove()
	{
		foreach (var pawn in m_Pawns)
		{
			pawn.canStartMoving = false;
		}
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
