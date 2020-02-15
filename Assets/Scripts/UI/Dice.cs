using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour
{
	public Animator animator;

	public delegate void RollDiceResult(int result);
	public event RollDiceResult RollResult;

	int _number = 1;

	public static Dice Instance { get; private set; }
	bool _blockRoll;

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
			_number = number > 0 ? number : Random.Range(1, 7);
			Block(true);
		}
	}

	/// <summary>
	/// Обновляет кубика на анимации
	/// Вызываеться анимацией
	/// </summary>
	public void UpdateAnimNumber() 
	{
		animator.SetInteger("Number", _number);
	}

	/// <summary>
	/// Резутьтат выполнения броска
	/// Вызываеться анимацией
	/// </summary>
	public void Result()
	{
		RollResult?.Invoke(_number);
	}

	/// <summary>
	/// Разблокировать возможность бросить кость
	/// </summary>
	/// <param name="canRoll"></param>
	public void Block(bool block)
	{
		_blockRoll = block;
		animator.SetBool("Jump", !block);
	}
}
