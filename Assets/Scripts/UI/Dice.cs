using UnityEngine;
using UnityEngine.UI;


public class Dice : MonoBehaviour
{
	public Text textNumber;
	public Button button;

	public delegate void RollDiceResult(int result);
	public event RollDiceResult RollResult;

	int _number = 1;

	public static Dice Instance { get; private set; }
	bool _blockRoll { get { return !button.interactable; } set { button.interactable = !value; } }

	void Awake()
	{
		Instance = this;
	}

	/// <summary>
	/// Бростить кость
	/// После сброса, возможность бросить еще раз будет заблокирована, пока ее не вернут методом UnlockRoll
	/// </summary>
	/// /// <param name="number">Результат броска</param>
	public void Roll(int number = 0)
	{
		if (!_blockRoll && !GameController.Instance.IsPause() && GameController.Instance.IsPlaying()) 
		{
			_blockRoll = true;
			_number = number > 0 ? number : Random.Range(1, 7);
			textNumber.text = _number.ToString();
			RollResult?.Invoke(_number);
		}
	}

	/// <summary>
	/// Разблокировать возможность бросить кость
	/// </summary>
	/// <param name="canRoll"></param>
	public void Block(bool block)
	{
		_blockRoll = block;
	}

	public void Reset() 
	{
		textNumber.text = "0";
	}
}
