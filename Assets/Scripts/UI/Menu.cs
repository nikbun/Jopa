using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
	public static Menu Instance { get; private set; }
	public SelectPlayersMenu selectPlayersMenu;
	public GameObject returnButton;

	void Awake()
	{
		Instance = this;
		returnButton.SetActive(false);
	}

	public void Display()
	{
		GameController.Instance.SetPause(true);
		gameObject.SetActive(true);
		returnButton.SetActive(GameController.Instance.IsPlaying());
	}

	/// <summary>
	/// Выйти из меню, либо на шаг назад в меню
	/// </summary>
	public void Back()
	{
		if (selectPlayersMenu.IsDisplay())
		{
			selectPlayersMenu.Back();
		}
		else if (GameController.Instance.IsPlaying())
		{
			gameObject.SetActive(false);
			GameController.Instance.SetPause(false);
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
		selectPlayersMenu.Display();
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
