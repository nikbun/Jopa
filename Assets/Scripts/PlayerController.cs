using UnityEngine;
using System.Collections;
using System;

abstract public class PlayerController : MonoBehaviour {
	public int numberChip; // Номер фишки передает контроллеру если может ходить
	public Vector3[] map; // Карта у каждого игрока своя
	public int cNumber; // Расстояние, которое должна пройти фишка
	public int nextPosition; // Следующая позиция
	public int currentPosition; // Текущая позиция
	public bool recoil; // Откат игрока
	public bool home; // Дома ли фишка
	public int ExitTunel; // Номер клетки выхода из тунеля
	public int tolkan; // Номер толкана
	public bool canMove; // Может ли двигаться фишка
	public GameObject gameController;
	public GameController gameControllerScript;

	// Use this for initialization
	void Start () {
		CreateMap();
		gameController = GameObject.Find ("GameController");
		gameControllerScript = gameController.GetComponent<GameController>();
		CountPlus();
	}

    // При нашатии на фишку
	void OnMouseDown(){
		if (canMove){
			gameControllerScript.SetCurrentChip(numberChip);
			cNumber = gameControllerScript.dice.GetLastNumber();
		}
	}

	// Проверка на возможность ходить 
	public bool CanMove(int number ,Vector3[] allChipPosition){
		// Исключаем позицию текущего игрока
		allChipPosition[numberChip] = new Vector3(10, 0, 10);
		// Проверка на дом 
		if (home){
			canMove = false;
			return canMove;
		}
		// Заполняем список шагов
		int[] stepsPos = GetSteps(number);
		// Есть ли шаги
		if (stepsPos[0] < 0){
			canMove = false;
			return canMove;
		}
		// Проверка на толкан
		if (tolkan > 0){
			switch(tolkan){
			case 1: 
				if (number == 1) canMove = true;
				else {
					canMove = false;
					return canMove;
				}
				break;
			case 3:
				if (number == 3) canMove = true;
				else {
					canMove = false;
					return canMove;
				}
				break;
			case 6:
				if (number == 6) canMove = true;
				else {
					canMove = false;
					return canMove;
				}
				break;
			default: 
				canMove = false;
				return canMove;
			}
		}
		// Проверка шагов
		for (int i = 0; i < stepsPos.Length; i++){
			if (Array.IndexOf<Vector3>(allChipPosition, map[stepsPos[i]]) >= 0){
				canMove = false;
				break;
			}else canMove = true;
		}
		return canMove;
	}

	// Возвращает массив ходов
	int[] GetSteps(int number){
		int position = currentPosition; // Позиция игрока
		int posAddition = 0; // Дополнительная позиция (тунель, либо толчек)
		int[] steps; // Масив ходов
		bool otcat = false;
		// Первый ход
		if (currentPosition == 0 && number == 6) return new int[] {1};
		if (currentPosition == 0 && number != 6) return new int[] {-1};
		if (tolkan > 0) return new int[] {nextPosition};
		// Проверка на толчки и тунели
		switch (currentPosition + number)
		{
			// Тунели
		case 3:
		case 15:
		case 27:
		case 39: posAddition = currentPosition + number + 8; break;
		case 11:
		case 23:
		case 35:
		case 47: posAddition = currentPosition + number  - 8; break;
			// Толчки
		case 4: posAddition = 62; break;
		case 16: posAddition = 65; break;
		case 28: posAddition = 68; break;
		case 40: posAddition = 71; break;
		}
		// Обычный ход
		if (posAddition > 0){
			steps = new int[number + 1];
			steps[number] = posAddition;
		}else steps = new int[number];
		for (int i = 0; i < number; i++){ 
			if (position == 49 + Count()) otcat = true;
			if (otcat) position--;
			else position++;
			steps[i] = position;
		}
		return steps;
	}

	// Отключает возможнось движения
	public bool NotCanMove(){
		canMove = false;
		return canMove;
	}

	// Двигает фишку если есть ходы
	public void Mover(){
			if (cNumber > 0) MoveController();
			else StopController();
	}

	
	// Проверяет где остановилась фишка
	void StopController(){
		// Проверка тунелей
		if (ExitTunel.Equals(0)){
			switch(currentPosition){
			case 3:
				cNumber = 3;
				nextPosition = 54;
				ExitTunel = currentPosition + 8;
				return;
			case 11:
				cNumber = 3;
				nextPosition = 55;
				ExitTunel = currentPosition - 8;
				return;
			case 15:
				cNumber = 3;
				nextPosition = 56;
				ExitTunel = currentPosition + 8;
				return;
			case 23:
				cNumber = 3;
				nextPosition = 57;
				ExitTunel = currentPosition - 8;
				return;
			case 27:
				cNumber = 3;
				nextPosition = 58;
				ExitTunel = currentPosition + 8;
				return;
			case 35:
				cNumber = 3;
				nextPosition = 59;
				ExitTunel = currentPosition - 8;
				return;
			case 39:
				cNumber = 3;
				nextPosition = 60;
				ExitTunel = currentPosition + 8;
				return;
			case 47:
				cNumber = 3;
				nextPosition = 61;
				ExitTunel = currentPosition - 8;
				return;
			}
		}
		//проверка толчков
		if (tolkan.Equals(0)){
			switch(currentPosition){
			case 4:
				cNumber = 1;
				nextPosition = 62;
				tolkan = -1;
				return;
			case 16:
				cNumber = 1;
				nextPosition = 65;
				tolkan = -1;
				return;
			case 28:
				cNumber = 1;
				nextPosition = 68;
				tolkan = -1;
				return;
			case 40:
				cNumber = 1;
				nextPosition = 71;
				tolkan = -1;
				return;
			}
		}
		// Если все проверки пройдены возвращает управление игроку
		gameControllerScript.UnlockButton();
		ExitTunel = 0;
		canMove = false;
		gameControllerScript.SetNullCurrentChip();
		if (recoil){
			nextPosition = nextPosition + 2;
			recoil = false;
		}
		if (currentPosition.Equals(49 + Count())){
			home = true;
			CounterDecrease();
		}
	}
	
	// Контролирует движение фишки на поле
	void MoveController(){
		float speed = gameControllerScript.speedGame;

		if (home)return;

		// Проверка находиться ли фишка на старте
		if (currentPosition.Equals(0) && (Mathf.Abs(transform.position.x) == 7.2f || Mathf.Abs(transform.position.z) == 7.2f)){
			if (cNumber.Equals(6)) {
				cNumber = 1;
				if (MovePlayer(map[nextPosition], speed)){
					cNumber = 0;
					currentPosition = nextPosition;
					nextPosition++;
				}
			}
			else cNumber = 0;
			return;
		}
		// Проверка на толчек
		if (!tolkan.Equals(0)){
			switch (tolkan){
			case -1:
				if (MovePlayer(map[nextPosition], speed)){
					cNumber = 0;
					nextPosition++;
					tolkan = 1;
				}
				return;
			case 1: 
				if (tolkan.Equals(cNumber)){
					if (MovePlayer(map[nextPosition], speed)){
						cNumber = 0;
						nextPosition++;
						tolkan = 3;
					}
				}else cNumber = 0;
				return;
			case 3:
				if (tolkan.Equals(cNumber)){
					if (MovePlayer(map[nextPosition], speed)){
						cNumber = 0;
						nextPosition = currentPosition + 3;
						tolkan = 6;
					}
				}else cNumber = 0;
				return;
			case 6:
				if (tolkan.Equals(cNumber)){
					if (MovePlayer(map[nextPosition], speed)){
						cNumber = 0;
						currentPosition = nextPosition;
						nextPosition++;
						tolkan = 0;
					}
				}else cNumber = 0;
				return;
			default:
				cNumber = 0;
				return;
			}
		}
		// Проверка на тунель
		if (ExitTunel > 0){
			speed *= 3;
			if (transform.position.x.Equals(map[ExitTunel].x) || transform.position.z.Equals(map[ExitTunel].z)){
				if (MovePlayer(map[ExitTunel], speed)){
					cNumber--;
					currentPosition = ExitTunel;
					nextPosition = ExitTunel + 1;
				}
			}else if (MovePlayer(map[nextPosition], speed)){
				cNumber--;
				if (currentPosition < ExitTunel) nextPosition++;
				else nextPosition--;
			}
			return;
		}
		//Дошла ли фишка до конца
		if (nextPosition.Equals(50 + Count()) && currentPosition.Equals(49 + Count())){
			nextPosition = nextPosition - 2;
			recoil = true;
		}
		// Если фишка в поле
		if (MovePlayer(map[nextPosition], speed))
		{
			cNumber--;
			currentPosition = nextPosition;
			if (recoil)nextPosition--;
			else nextPosition++;
		}
		
	}
	
	// Передвигает фишку к заданому вектору
	bool MovePlayer(Vector3 vector, float speed){
		// Передвижение по позиции z
		if (vector.z > transform.position.z){
			if (vector.z < transform.position.z + speed * Time.deltaTime) 
				transform.position = new Vector3(transform.position.x, 0f, vector.z);
			else
				transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}else if (vector.z < transform.position.z){
			if (vector.z > transform.position.z - speed * Time.deltaTime) 
				transform.position = new Vector3(transform.position.x, 0f, vector.z);
			else
				transform.Translate(-Vector3.forward * speed * Time.deltaTime);
		}
		// Передвижение по позиции x
		if (vector.x > transform.position.x){
			if (vector.x < transform.position.x + speed*Time.deltaTime) 
				transform.position = new Vector3(vector.x, 0f, transform.position.z);
			else
				transform.Translate(Vector3.right * speed * Time.deltaTime);
		}else if (vector.x < transform.position.x){
			if (vector.x > transform.position.x - speed*Time.deltaTime) 
				transform.position = new Vector3(vector.x, 0f, transform.position.z);
			else
				transform.Translate(Vector3.left * speed * Time.deltaTime);
		}
		// Если объект пришел к точке
		if (transform.position.Equals(vector))return true;
		else return false;
	}

	abstract public void CreateMap();
	abstract public void CountPlus();
	abstract public void CounterDecrease();
	abstract public int Count();
}
