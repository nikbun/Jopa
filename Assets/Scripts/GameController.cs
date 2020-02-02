using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Map;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public float gameSpeed = 10f; // Скорость игры. Нельзя поменять в процессе игры
	[Range(1, 4)]
	public int countPlayers = 4;
	public int currentPlayer = 0;
	public List<GameObject> instancePlayers; // Экземпляры игроков
	public Dice dice;

	Dictionary<PlayerPosition, Player> m_Players = new Dictionary<PlayerPosition, Player>(); // Скрипты управления игроками

	void Start() 
	{
		new GameData(new GameMap(), gameSpeed);
		dice.RollResult += CanMovePawns;
		InitPlayers();
	}

	/// <summary>
	/// Загружаем игроков на стартовые позиции
	/// </summary>
	void InitPlayers()
	{
		for (int i = 0; i < countPlayers; i++)
		{
			var playerPos = (PlayerPosition)i;
			var player = Instantiate(instancePlayers[i], GameData.instance.map.GetOriginPosition(playerPos), Quaternion.identity);
			var sPlayer = player.GetComponent<Player>();
			sPlayer.playerPosition = playerPos;
			sPlayer.EndTurn += EndTurn;
			m_Players.Add(playerPos, sPlayer);
		}
	}

	public void CanMovePawns(int steps)
	{
		bool canMove = m_Players[(PlayerPosition)currentPlayer].CanMovePawns(steps);
		if (!canMove)
			NextTurn();
	}

	public void EndTurn()
	{
		foreach (var key in m_Players.Keys)
		{
			if (!m_Players[key].IsPawnsEndMove())
				return;
		}
		NextTurn();
	}

	/// <summary>
	/// Выбрать следующего игрока
	/// </summary>
	public int NextTurn()
	{
		currentPlayer = (++currentPlayer) % countPlayers;
		dice.SetCanRoll(true);
		return currentPlayer;
	}
}

/// <summary>
/// Класс для хранения общих данных
/// </summary>
public class GameData
{
	public static GameData instance;
	public GameMap map { get; } // Игровая карта
	public float speed { get; } // Скорость игры

	public GameData(GameMap map, float speed = 10f) 
	{
		this.map = map;
		this.speed = speed;
		if (instance == null)
			instance = this;
	}
}