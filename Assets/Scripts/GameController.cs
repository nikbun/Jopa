using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Map;

namespace Assets.Scripts
{
	public class GameController : MonoBehaviour
	{
		public float gameSpeed; // Скорость игры
		public int countPlayers;
		public GameObject[] instancesPawns; // Экземпляры фишек игроков
		
		Player[] players;
		int currentPlayer;
		Dice dice; // Игровая кость

		public GameMap gameMap { get; } // Игровая карта

		GameController()
		{
			gameMap = new GameMap();
			GlobalGameData.GameController = this;
			GlobalGameData.GameSpeed = gameSpeed;
		}

		void Start() //Выставение фишек на начальные позиции 
		{
			dice = new Dice();
			LoadPlayers();
		}

		void Update()
		{
			//if (Input.GetKeyUp(KeyCode.Space) && !blockButton) StepUp();
		}

		/// <summary>
		/// Загружаем игроков и их фишки на стартовые позиции
		/// </summary>
		void LoadPlayers()
		{
			players = new Player[countPlayers];
			for (int i = 0; i < countPlayers; i++)
			{
				Pawn[] pawns = new Pawn[4];
				for (int p = 0; p < pawns.Length; p++)
				{
					GameObject pawnObject = Instantiate(this.instancesPawns[i], gameMap.GetStartPosition((PlayerPositions)i), Quaternion.Euler(90,0,0));
					pawns[p] = pawnObject.GetComponent<Pawn>();
				}
				players[i] = new Player((PlayerPositions)i, pawns);
			}
		}

		/// <summary>
		/// Выбрать следующего игрока
		/// </summary>
		public void NextPlayer()
		{
			currentPlayer = ++currentPlayer & 3;
		}
	}
}