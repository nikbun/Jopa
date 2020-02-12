using MapSpace;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }
#if UNITY_EDITOR
	public bool quickStart; // Игроки назначаються автоматически
	[Range(1, 4)]
	public int countPlayers = 4;
#endif

	Dictionary<Map.Sides, Player> _players = new Dictionary<Map.Sides, Player>(); // Скрипты управления игроками
	List<Map.Sides> _playersSides;
	int _currentPlayerNumber;
	Player _currentPlayer { get { return _players[_playersSides[_currentPlayerNumber]]; } }
	int _countPlayers;
	bool _isPlaying; // Идет игра
	bool _pause; // Пауза игры

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		Instantiate(GameData.Instance.gameUI);
		Dice.Instance.BlockRoll();

#if UNITY_EDITOR
		// Запуск быстрого старта для отладки
		if (quickStart)
		{
			var players = new Dictionary<Map.Sides, GameObject>();
			var instPlayers = GameData.Instance.samplePlayers;
			for (int i = 0; i < countPlayers; i++)
			{
				players.Add((Map.Sides)i, instPlayers[i]);
			}
			StartGame(players);
			Menu.Instance.Back();
		}
#endif
	}

	void Update()
	{
		// Бросок кости
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Dice.Instance.Roll();
		}
		// Кнопка меню
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (_pause)
			{
				Menu.Instance.Back();
			}
			else
			{
				Menu.Instance.Display();
			}
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
		if (num > 0)
			Dice.Instance.Roll(num);
#endif
	}

	/// <summary>
	/// Устанавливаем игроков на стартовые позиции
	/// </summary>
	public void StartGame(Dictionary<Map.Sides, GameObject> samplePlayers)
	{
		_currentPlayerNumber = 0;
		_countPlayers = samplePlayers.Count;
		_playersSides = new List<Map.Sides>();
		foreach (var instPlayer in samplePlayers)
		{
			var mapSide = instPlayer.Key;
			var player = Instantiate(instPlayer.Value, GameData.Instance.mapController.GetMap(mapSide).GetOrigin().position, Quaternion.identity);
			var sPlayer = player.GetComponent<Player>();
			sPlayer.mapSide = mapSide;
			sPlayer.EndTurn += EndTurn;
			_players.Add(mapSide, sPlayer);
			_playersSides.Add(mapSide);
		}
		Dice.Instance.BlockRoll(false);
		_isPlaying = true;
		_pause = false;
		GameStatus.Instance.SetCurrentPlayerName(_currentPlayer.playerName, _currentPlayer.color);
	}

	public bool IsPlaying()
	{
		return _isPlaying;
	}

	public bool IsPause()
	{
		return _pause;
	}

	public void SetPause(bool pause)
	{
		_pause = pause;
	}

	/// <summary>
	/// Начать ход
	/// </summary>
	/// <param name="diceResult">Результат сброса кубика</param>
	public void StartTurn(int diceResult)
	{
		bool canMove = _currentPlayer.CanStartMove(diceResult);
		if (!canMove)
			EndTurn();
	}

	/// <summary>
	/// Закончить ход
	/// </summary>
	void EndTurn()
	{
		if (_currentPlayer.IsHome()) 
		{
			GameStatus.Instance.SetWinner(_currentPlayer.playerName, _currentPlayer.color);
			_isPlaying = false;
		}
		foreach (var key in _players.Keys)
		{
			if (!_players[key].IsEndMove())
				return;
		}
		_currentPlayerNumber = (++_currentPlayerNumber) % _countPlayers;
		Dice.Instance.BlockRoll(false);
		GameStatus.Instance.SetCurrentPlayerName(_currentPlayer.playerName, _currentPlayer.color);
	}

	/// <summary>
	/// Сбросить игру до начального состояния
	/// </summary>
	public void ResetGame()
	{
		foreach (var player in _players)
		{
			player.Value.DestroyPlayer();
		}
		_players.Clear();
		_isPlaying = false;
		Dice.Instance.Reset();
		GameStatus.Instance.Reset();
	}
}
