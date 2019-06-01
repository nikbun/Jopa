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
#if UNITY_EDITOR
		DebugDice();
#endif
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
#if UNITY_EDITOR
			if (GameController.instance.DebugDiceNumber > 0)
				number = GameController.instance.DebugDiceNumber;
#endif
			textNumber.text = number.ToString();
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

#if UNITY_EDITOR
	public void DebugDice()
	{
		int num = -1;
		if (Input.GetKeyUp(KeyCode.Alpha0))
			num = 0;
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
		if (num >= 0)
		{
			GameController.instance.DebugDiceNumber = num;
			if (num > 0)
				Roll();
		}
	}
#endif
}
