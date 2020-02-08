using UnityEngine;
using MapSpace;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public float gameSpeed = 10f; // Скорость передвижения фишек
	[Range(1, 4)]
	public int countPlayers = 4;
	public List<GameObject> instancePlayers; // Экземпляры игроков
	public Dice dice;

	int _currentPlayer = 0;
	Dictionary<Map.Sides, Player> _players = new Dictionary<Map.Sides, Player>(); // Скрипты управления игроками

	void Start() 
	{
		GameData.Instanse.Update(new MapController(), gameSpeed);
		dice.RollResult += StartTurn;
		InitPlayers();
	}

	void Update() 
	{
		// Бросок кости
		if (Input.GetKeyUp(KeyCode.Space))
		{
			dice.Roll();
		}
#if UNITY_EDITOR
		// Бросок кости на определенное число
		// Клавиши от 1 до 6
		// Выброшенное число соответствует номеру клавиши
		int num = -1;
		if (Input.GetKeyUp(KeyCode.Alpha1))
			num = 1;
		if (Input.GetKeyUp(KeyCode.Alpha2))
			num = 2;
		if (Input.GetKeyUp(KeyCode.Alpha3))
			num = 3;
		if (Input.GetKeyUp(KeyCode.Alpha4))
			num = 4;
		if (Input.GetKeyUp(KeyCode.Alpha5))
			num = 5;
		if (Input.GetKeyUp(KeyCode.Alpha6))
			num = 6;
		if(num > 0)
			dice.Roll(num);
#endif
	}

	/// <summary>
	/// Устанавливаем игроков на стартовые позиции
	/// </summary>
	void InitPlayers()
	{
		for (int i = 0; i < countPlayers; i++)
		{
			var mapSide = (Map.Sides)i;
			var player = Instantiate(instancePlayers[i], GameData.Instanse.mapController.GetMap(mapSide).GetOrigin().position, Quaternion.identity);
			var sPlayer = player.GetComponent<Player>();
			sPlayer.mapSide = mapSide;
			sPlayer.EndTurn += EndTurn;
			_players.Add(mapSide, sPlayer);
		}
	}

	void StartTurn(int diceResult)
	{
		bool canMove = _players[(Map.Sides)_currentPlayer].CanStartMove(diceResult);
		if (!canMove)
			NextTurn();
	}

	void EndTurn()
	{
		foreach (var key in _players.Keys)
		{
			if (!_players[key].IsEndMove())
				return;
		}
		NextTurn();
	}

	/// <summary>
	/// Выбрать следующего игрока
	/// </summary>
	void NextTurn()
	{
		_currentPlayer = (++_currentPlayer) % countPlayers;
		dice.UnlockRoll();
	}
}
