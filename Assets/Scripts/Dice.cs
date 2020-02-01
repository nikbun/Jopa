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
	}

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
}
