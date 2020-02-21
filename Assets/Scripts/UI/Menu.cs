using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
	[Tooltip("Кнопка возврата в игру")]
	[SerializeField] GameObject _returnButton;

	[Tooltip("Меню выбора персонажа")]
	[SerializeField] SelectPlayersMenu _selectPlayersMenu;

	public static Menu Instance { get; private set; }

	void Awake()
	{
		Instance = this;
		_returnButton.SetActive(false);
	}

	public void Display()
	{
		GameController.Instance.IsPause = true;
		gameObject.SetActive(true);
		_returnButton.SetActive(GameController.Instance.IsGameStart);
	}

	/// <summary>
	/// Выйти из меню, либо на шаг назад в меню
	/// </summary>
	public void Back()
	{
		if (_selectPlayersMenu.IsDisplay)
		{
			_selectPlayersMenu.Back();
		}
		else if (GameController.Instance.IsGameStart)
		{
			gameObject.SetActive(false);
			GameController.Instance.IsPause = false;
		}
	}

	/// <summary>
	/// Выбрать игроков(Начать игру)
	/// Сбрасывается предыдущая игра
	/// </summary>
	public void ChoosePlayers()
	{
		GameController.Instance.ResetGame();
		gameObject.SetActive(false);
		_selectPlayersMenu.IsDisplay = true;
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
