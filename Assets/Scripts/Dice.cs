using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Dice : MonoBehaviour
{
	public Text textNumber;
	private int number;
	public static Dice instance = null;

	public Dice()
	{
		number = 1;
	}

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
			Roll();
	}

	/// <summary>
	/// Бросить кость
	/// </summary>
	/// <returns>Число от 1 до 6</returns>
	public void Roll()
	{
		if (GameController.instance.gameState == GameStates.RollDice)
		{
			number = Random.Range(1, 7);
			// TODO Удалить или обернуть директиву дебага
			if (GameController.instance.DebugDiceNumber > 0)
				number = GameController.instance.DebugDiceNumber;
			textNumber.text = number.ToString();
			Debug.Log("Игрок кинул кубик. Выпало число " + number);
			GameController.instance.CanMovePawns(number);
		}
	}

	/// <summary>
	/// получить последнне выбрашенное число
	/// </summary>
	/// <returns></returns>
	public int GetNumber()
	{
		return number;
	}
}
