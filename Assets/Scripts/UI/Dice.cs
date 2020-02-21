using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public sealed class Dice : MonoBehaviour
{
	[SerializeField] Animator _animator;
	int _number = 1;
	bool _blockRoll;

	public delegate void RollDiceResult(int result);
	public event RollDiceResult RollResult;

	public static Dice Instance { get; private set; }

	void Awake()
	{
		Instance = this;
	}

	/// <summary>
	/// Бростить кость
	/// После сброса, возможность бросить еще раз будет заблокирована, пока ее не вернут методом UnlockRoll
	/// </summary>
	/// <param name="number">Результат броска</param>
	public void Roll(int number = 0)
	{
		if (!_blockRoll && !GameController.Instance.IsPause && GameController.Instance.IsGameStart)
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
		_animator.SetInteger("Number", _number);
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
		_animator.SetBool("Jump", !block);
	}
}