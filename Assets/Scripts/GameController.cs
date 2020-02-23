using MapSpace;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
#if UNITY_EDITOR
	[Tooltip("Игроки назначаються автоматически при старте")]
	[SerializeField] bool _quickStart;

	[Range(1, 4)]
	[Tooltip("Количество игроков при быстром старте")]
	[SerializeField] int _countPlayers = 4;
#endif

	readonly List<Player> _players = new List<Player>();
	int _currentPlayerNumber;
	float _timeToReactAI = 0;

	Player CurrentPlayer { get { return _players.Count == 0 ? null : _players[_currentPlayerNumber]; } }

	public static GameController Instance { get; private set; }

	public bool IsPause { get; set; }

	public bool IsGameStart { get; private set; }

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		Instantiate(GameData.Instance.GameUI);
		Dice.Instance.RollResult += StartTurn;
		Dice.Instance.Block(true);

#if UNITY_EDITOR
		// Запуск быстрого старта для отладки
		if (_quickStart)
		{
			for (int i = 0; i < _countPlayers; i++)
			{
				AddPlayer(GameData.Instance.Players[i].SamplePlayer, (Map.Sides)i);
			}
			StartGame();
			Menu.Instance.Back();
		}
#endif
	}

	void Update()
	{
		if (_timeToReactAI <= 0 && CurrentPlayer != null && CurrentPlayer.PlayerType == Player.Type.AI && CurrentPlayer.IsEndInitialization)
		{
			Dice.Instance.Roll();
			_timeToReactAI = GameData.Instance.ReactionAI;
		}
		else
		{
			_timeToReactAI -= Time.deltaTime;
		}
		// Бросок кости
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Dice.Instance.Roll();
		}
		// Кнопка меню
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (IsPause)
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

	public void AddPlayer(GameObject player, Map.Sides side, Player.Type type = Player.Type.Player)
	{
		var oldPlayer = _players.Find(p => p.MapSide == side);
		if (oldPlayer != null)
			oldPlayer.DestroyPlayer();

		var goPlayer = Instantiate(player, GameData.Instance.MapController.GetMap(side).GetOrigin().Position, Quaternion.identity);
		var sPlayer = goPlayer.GetComponent<Player>();
		sPlayer.PlayerType = type;
		sPlayer.MapSide = side;
		sPlayer.EndTurn += EndTurn;
		_players.Add(sPlayer);
	}

	/// <summary>
	/// Устанавливаем игроков на стартовые позиции
	/// </summary>
	public void StartGame()
	{
		_currentPlayerNumber = 0;
		Dice.Instance.Block(false);
		IsGameStart = true;
		IsPause = false;
		GameStatus.Instance.SetCurrentPlayerName(CurrentPlayer.PlayerName, CurrentPlayer.NameColor);
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
		IsGameStart = false;
		GameStatus.Instance.Reset();
	}

	/// <summary>
	/// Начать ход
	/// </summary>
	/// <param name="diceResult">Результат сброса кубика</param>
	void StartTurn(int diceResult)
	{
		CurrentPlayer.StartTurn(diceResult);
	}

	/// <summary>
	/// Закончить ход
	/// </summary>
	void EndTurn()
	{
		// Все игроки закончили ход?
		foreach (var player in _players)
		{
			if (!player.IsEndTurn)
				return;
		}
		// Текущий игрок добрался до дома?
		if (CurrentPlayer.IsEndGame)
		{
			GameStatus.Instance.SetWinner(CurrentPlayer.PlayerName, CurrentPlayer.NameColor);
			IsGameStart = false;
		}
		// Установка следующего игрока
		_currentPlayerNumber = (++_currentPlayerNumber) % _players.Count;
		
		Dice.Instance.Block(false);
		GameStatus.Instance.SetCurrentPlayerName(CurrentPlayer.PlayerName, CurrentPlayer.NameColor);
		_timeToReactAI = GameData.Instance.ReactionAI;
	}
}
