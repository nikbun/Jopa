using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Dice : MonoBehaviour
{
	public Text textNumber;

	public delegate void RollDiceResult(int result);
	public event RollDiceResult RollResult;

	int m_Number = 1;
	bool m_CanRoll = true;

	void Update()
	{
		if (m_CanRoll && Input.GetKeyUp(KeyCode.Space)) 
		{
			Roll();
		}
#if UNITY_EDITOR
		DebugDice();
#endif
	}

	/// <summary>
	/// Установить возможность бросить кость
	/// </summary>
	/// <param name="canRoll"></param>
	public void SetCanRoll(bool canRoll) 
	{
		m_CanRoll = canRoll;
	}

	/// <summary>
	/// Бростить кость
	/// После сброса, возможность сбросить еще раз будет заблокирована, пока ее не вернут методом SetCanRoll
	/// </summary>
	private void Roll()
	{
		if (m_CanRoll) 
		{
			m_CanRoll = false;
			m_Number = Random.Range(1, 7);
			textNumber.text = m_Number.ToString();
			RollResult?.Invoke(m_Number);
		}
	}

#if UNITY_EDITOR
	/// <summary>
	/// Бросок кости на определенное число
	/// Клавиши от 1 до 6
	/// Выброшенное число соответствует номеру клавиши
	/// </summary>
	public void DebugDice()
	{
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
		if (num >= 0)
		{
			m_CanRoll = false;
			m_Number = num;
			textNumber.text = m_Number.ToString();
			RollResult?.Invoke(m_Number);
		}
	}
#endif
}
