using UnityEngine;
using UnityEngine.UI;


public class Dice : MonoBehaviour
{
	public static Dice Instance { get; private set; }
	public Text textNumber;
	public Button button;

	int _number = 1;
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
			GameController.Instance.StartTurn(_number);
		}
	}

	/// <summary>
	/// Разблокировать возможность бросить кость
	/// </summary>
	/// <param name="canRoll"></param>
	public void BlockRoll(bool block = true)
	{
		_blockRoll = block;
	}

	public void Reset() 
	{
		textNumber.text = "0";
	}
}
