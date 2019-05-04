using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game;

public class GameController : MonoBehaviour {
	public GameObject[] players; //Массив фишек игрока
	public GameObject[] chipsPlayers; //Зеленые фишки
	public PlayerController[] chipsPlayScripts; // Их скрипты
	public int countPlayers; // Количество игроков
	public int currentPlayer; //Текущий игрок
	public int currentChip; // Текущая фишка
	public Text tableNumber; // Подключен внешкий текст для вывода на экран чисел
	public Dice dice; // Игровая кость
	public float speedGame; // Скорость игры
	public bool blockButton;// Блокировка кнопки
	public Map gameMap; // Игровая карта
	
	void Start () //Выставение фишек на начальные позиции 
	{
		gameMap = new Map();
		dice = new Dice();
		LoadStartPosition();
	}

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Space) && !blockButton) StepUp();
	}

	void FixedUpdate(){
		// Если текущий номер не меньше нуля, то движемся пока номер не вернет 0
		if (currentChip >= 0){
			chipsPlayScripts[currentChip].Mover();
		}
	}

	// Загрузка начальных позиций
	void LoadStartPosition(){
		chipsPlayers = new GameObject[4 * countPlayers];
		int currentNumberChip = 0;

		for (int playerNumber = 0; playerNumber < countPlayers; playerNumber++){
			for (int chipNumber = 0; chipNumber < 4; chipNumber++){
				Vector3 spawnPosition = gameMap.GetStartPosition((PlayerPositions)playerNumber);
				Quaternion spawnRotation = new Quaternion ();
				chipsPlayers[currentNumberChip++] =
					Instantiate (players[playerNumber], spawnPosition, spawnRotation);
			}
		}

		// Создает массив скриптов из объектов
		chipsPlayScripts = new PlayerController[chipsPlayers.Length];
		for (int i = 0; i < chipsPlayers.Length; i++)
		{
			chipsPlayScripts[i] = chipsPlayers[i].GetComponent<PlayerController>();
		}
	}

	//Симуляция бросания кубика( выдает цифру от 1 до 6)
	public void StepUp()
	{
		if (!blockButton){
			LockButton();
			tableNumber.text = dice.Throw().ToString();//Вывод на экран

			if (currentPlayer == countPlayers)currentPlayer = 1;
			else currentPlayer++;

			// Какие фишки могут ходить
			bool canMove = false;
			int posChip = 0;
			if (countPlayers <= 2){
				posChip = (currentPlayer - 1) * 4;
			} else {
				switch (currentPlayer){
				case 1: posChip = 0; break;
				case 2: posChip = 4; break;
				case 3: posChip = 8; break;
				case 4: posChip = 12; break;
				}
			}
			for(int i = 0; i < 4; i++ ){
				if (!canMove) canMove = chipsPlayScripts[posChip++].CanMove(dice.GetLastNumber() ,GetAllChipPosition());
				else chipsPlayScripts[posChip++].CanMove(dice.GetLastNumber(), GetAllChipPosition());
			}
			if (!canMove) UnlockButton();
		}
	}
	
	public void LockButton(){
		blockButton = true;
	}

	public void UnlockButton(){
		blockButton = false;
	}

	// Выключает ходилки кроме текущей
	public void NotCanMove(){
		for (int i = 0; i < countPlayers * 4; i++){
			if (i == currentChip)i++;
			else chipsPlayScripts[i].NotCanMove();
		}
	}

	// Возвращает все позиции игроков
	public Vector3[] GetAllChipPosition(){
		Vector3[] vectorPosition = new Vector3[countPlayers * 4];
		for (int i = 0; i < countPlayers * 4; i++)
			vectorPosition[i] = chipsPlayers[i].transform.position;
		return vectorPosition;
	}

	// Возвращает количество игроков
	public int GetCountPlayers(){
		return countPlayers;
	}

	public void SetNullCurrentChip(){
		currentChip = -1;
	}

	public void SetCurrentChip(int chip){
		currentChip = chip;
		NotCanMove();
	}

	
	public void SetCurrentNumberChip(GameObject chip){
		currentChip = GetNumberFromPlayers(chipsPlayers, chip);
	}

	public int GetNumberFromPlayers(GameObject[] fromFind, GameObject toFind){
		for(int i = 0; i < fromFind.Length; i++){
			if (fromFind[i].GetHashCode().Equals(toFind.GetHashCode()))return i;
		}
		return -1;
	}
}
