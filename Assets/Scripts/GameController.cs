using MapSpace;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
#if UNITY_EDITOR
	public bool quickStart; // Игроки назначаються автоматически
	[Range(1, 4)]
	public int countPlayers = 4;
#endif

	List<Player> _players = new List<Player>(); // Скрипты управления игроками
	int _currentPlayerNumber;
	bool _isPlaying; // Идет игра
	bool _pause; // Пауза игры

	public static GameController Instance { get; private set; }
	Player CurrentPlayer { get { return _players.Count == 0 ? null : _players[_currentPlayerNumber]; } }

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		Instantiate(GameData.Instance.gameUI);
		Dice.Instance.RollResult += StartTurn;
		Dice.Instance.Block(true);

#if UNITY_EDITOR
		// Запуск быстрого старта для отладки
		if (quickStart)
		{
			for (int i = 0; i < countPlayers; i++)
			{
				AddPlayer(GameData.Instance.samplePlayers[i], (Map.Sides)i);
			}
			StartGame();
			Menu.Instance.Back();
		}
#endif
	}

	void Update()
	{
		if (CurrentPlayer != null && CurrentPlayer.type == Player.Type.AI && CurrentPlayer.EndInitialization)
		{
			Dice.Instance.Roll();
		}
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
	public void StartGame()
	{
		_currentPlayerNumber = 0;
		Dice.Instance.Block(false);
		_isPlaying = true;
		_pause = false;
		GameStatus.Instance.SetCurrentPlayerName(CurrentPlayer.playerName, CurrentPlayer.color);
	}

	public void AddPlayer(GameObject player, Map.Sides side, Player.Type type = Player.Type.Player) 
	{
		var oldPlayer = _players.Find(p => p.mapSide == side);
		if (oldPlayer != null)
			oldPlayer.DestroyPlayer();

		var goPlayer = Instantiate(player, GameData.Instance.mapController.GetMap(side).GetOrigin().position, Quaternion.identity);
		var sPlayer = goPlayer.GetComponent<Player>();
		sPlayer.type = type;
		sPlayer.mapSide = side;
		sPlayer.EndTurn += EndTurn;
		_players.Add(sPlayer);
	}

	/// <summary>
	/// Сбросить игру до начального состояния
	/// </summary>
	public void ResetGame()
	{
		foreach (var player in _players)
		{
			player.DestroyPlayer();
		}
		_players.Clear();
		_isPlaying = false;
		Dice.Instance.Reset();
		GameStatus.Instance.Reset();
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
	void StartTurn(int diceResult)
	{
		CurrentPlayer.StartMove(diceResult);
	}

	/// <summary>
	/// Закончить ход
	/// </summary>
	void EndTurn()
	{
		// Все игроки закончили ход?
		foreach (var player in _players)
		{
			if (!player.IsEndMove())
				return;
		}
		// Текущий игрок добрался до дома?
		if (CurrentPlayer.IsHome())
		{
			GameStatus.Instance.SetWinner(CurrentPlayer.playerName, CurrentPlayer.color);
			_isPlaying = false;
		}
		// Установка следующего игрока
		_currentPlayerNumber = (++_currentPlayerNumber) % _players.Count;
		
		Dice.Instance.Block(false);
		GameStatus.Instance.SetCurrentPlayerName(CurrentPlayer.playerName, CurrentPlayer.color);

		if (CurrentPlayer.type == Player.Type.AI)
		{
			Dice.Instance.Roll();
		}
	}
}
