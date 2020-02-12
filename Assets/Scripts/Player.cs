using System.Collections.Generic;
using UnityEngine;
using MapSpace;

public class Player : MonoBehaviour 
{
	public GameObject instancePawns;
	public Map.Sides mapSide;

	public delegate void EndTurnDeleg();
	public event EndTurnDeleg EndTurn;

	List<Pawn> _pawns = new List<Pawn>();

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
			sPawn.mapSide = mapSide;
			sPawn.StartMovement += OffOutlinePawns;
			sPawn.StopMovement += EndTurn.Invoke;
			_pawns.Add(sPawn);
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
		foreach (var pawn in _pawns)
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
		return !_pawns.Exists(p => p.IsMoving());
	}

	/// <summary>
	/// Отключить подсветку фишек, которыми можно ходить
	/// </summary>
	void OffOutlinePawns()
	{
		foreach (var pawn in _pawns)
		{
			pawn.CancelStartMove();
		}
	}

	public void DestroyPlayer() 
	{
		foreach (var pawn in _pawns) 
		{
			pawn.DestroyPawn();
		}
		_pawns.Clear();
		Destroy(gameObject);
	}
}