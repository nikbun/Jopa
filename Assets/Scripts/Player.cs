using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MapSpace;

public class Player : MonoBehaviour
{
	[Tooltip("Имя игрока")]
	[SerializeField] string _playerName;
	[Tooltip("Цвет имени игрока")]
	[SerializeField] Color _nameColor;
	[Tooltip("Образец фишек")]
	[SerializeField] GameObject _samplePawn;
	// Список фишек игрока
	List<Pawn> _pawns = new List<Pawn>();

	public delegate void EndTurnDeleg();
	public event EndTurnDeleg EndTurn;

	public string PlayerName { get => _playerName; }

	public Color NameColor { get => _nameColor; }

	public Type PlayerType { get; set; }

	public Map.Sides MapSide { get; set; }

	public bool IsEndInitialization { get => _pawns.Count > 0 && _pawns.TrueForAll(p => p.IsEndInitialization); }

	public bool IsEndTurn { get => !_pawns.Any(p => p.IsMoving); }
	
	public bool IsEndGame { get => _pawns.All(p => p.IsInHome); }


	void Start()
	{
		InitPawns();
	}

	void InitPawns()
	{
		for (int i = 0; i < 4; i++)
		{
			var pawn = Instantiate(_samplePawn, transform.position, Quaternion.Euler(90f, 0, 0));
			pawn.transform.SetParent(transform);
			var sPawn = pawn.GetComponent<Pawn>();
			sPawn.MapSide = MapSide;
			sPawn.StartMovement += CancelStartMovePawns;
			sPawn.StopMovement += EndTurn.Invoke;
			_pawns.Add(sPawn);
		}
	}

	/// <summary>
	/// Начать ход
	/// </summary>
	/// <param name="diceResult">Результат сброса кубика</param>
	/// <returns></returns>
	public void StartTurn(int diceResult)
	{
		var canMovePawns = _pawns.Where(p => p.CanStartMove(diceResult)).ToList();

		if (PlayerType == Type.AI && canMovePawns.Any())
			canMovePawns[Random.Range(0, canMovePawns.Count)].Move();
		if (!canMovePawns.Any())
			EndTurn?.Invoke();
	}

	/// <summary>
	/// Отменить возможность начать движение у всех фишек
	/// </summary>
	void CancelStartMovePawns()
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

	public enum Type
	{
		Player,
		AI
	}
}