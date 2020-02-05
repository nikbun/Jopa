using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Map;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public float gameSpeed = 10f; // Скорость передвижения фишек
	[Range(1, 4)]
	public int countPlayers = 4;
	public List<GameObject> instancePlayers; // Экземпляры игроков
	public Dice dice;

	int m_CurrentPlayer = 0;
	Dictionary<MapSides, Player> m_Players = new Dictionary<MapSides, Player>(); // Скрипты управления игроками

	void Start() 
	{
		new GameData(new GameMap(), gameSpeed);
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
			var mapSide = (MapSides)i;
			var player = Instantiate(instancePlayers[i], GameData.instance.map.GetOrigin(mapSide).position, Quaternion.identity);
			var sPlayer = player.GetComponent<Player>();
			sPlayer.mapSide = mapSide;
			sPlayer.EndTurn += EndTurn;
			m_Players.Add(mapSide, sPlayer);
		}
	}

	void StartTurn(int diceResult)
	{
		bool canMove = m_Players[(MapSides)m_CurrentPlayer].CanStartMove(diceResult);
		if (!canMove)
			NextTurn();
	}

	void EndTurn()
	{
		foreach (var key in m_Players.Keys)
		{
			if (!m_Players[key].IsEndMove())
				return;
		}
		NextTurn();
	}

	/// <summary>
	/// Выбрать следующего игрока
	/// </summary>
	void NextTurn()
	{
		m_CurrentPlayer = (++m_CurrentPlayer) % countPlayers;
		dice.UnlockRoll();
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