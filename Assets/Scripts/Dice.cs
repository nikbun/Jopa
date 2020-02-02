using UnityEngine;
using UnityEngine.UI;


public class Dice : MonoBehaviour
{
	public Text textNumber;

	public delegate void RollDiceResult(int result);
	public event RollDiceResult RollResult;

	int m_Number = 1;
	bool m_BlockRoll = false;

	/// <summary>
	/// Бростить кость
	/// После сброса, возможность бросить еще раз будет заблокирована, пока ее не вернут методом UnlockRoll
	/// </summary>
	/// /// <param name="number">Результат броска</param>
	public void Roll(int number = 0)
	{
		if (!m_BlockRoll) 
		{
			m_BlockRoll = true;
			m_Number = number > 0 ? number : Random.Range(1, 7);
			textNumber.text = m_Number.ToString();
			RollResult?.Invoke(m_Number);
		}
	}

	/// <summary>
	/// Разблокировать возможность бросить кость
	/// </summary>
	/// <param name="canRoll"></param>
	public void UnlockRoll()
	{
		m_BlockRoll = false;
	}
}
