using System.Collections.Generic;
using UnityEngine;
using MapSpace;

public class Player : MonoBehaviour
{
	public string playerName;
	public Color color;
	public Type type;
	public Map.Sides mapSide;
	public GameObject samplePawn;

	public delegate void EndTurnDeleg();
	public event EndTurnDeleg EndTurn;

	List<Pawn> _pawns = new List<Pawn>();

	public bool EndInitialization { get { return _pawns.Count > 0 && _pawns.TrueForAll(p => p.EndInitialization); } }

	void Start()
	{
		InitPawns();
	}

	void InitPawns()
	{
		for (int i = 0; i < 4; i++)
		{
			var pawn = Instantiate(samplePawn, transform.position, Quaternion.Euler(90f, 0, 0));
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
	public void StartMove(int diceResult)
	{
		bool canMove = false;
		var movePawns = new List<Pawn>();
		foreach (var pawn in _pawns)
		{
			canMove = pawn.CanStartMove(diceResult) || canMove;
			if (canMove)
				movePawns.Add(pawn);
		}
		if (type == Type.AI && movePawns.Count > 0)
			movePawns[Random.Range(0, movePawns.Count)].StartMove();
		if (!canMove)
			EndTurn?.Invoke();
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

	public bool IsHome()
	{
		bool isAllPawnsInHome = true;
		foreach (var pawn in _pawns)
		{
			isAllPawnsInHome = isAllPawnsInHome && pawn.IsHome();
		}
		return isAllPawnsInHome;
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

	public enum Type
	{
		Player,
		AI
	}
}