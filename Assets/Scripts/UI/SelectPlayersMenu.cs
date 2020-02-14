using MapSpace;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayersMenu : MonoBehaviour
{
	[Header("Порядок списков(side) B.L.T.R.")]
	[Tooltip("Порядок: Bottom, Left, Top, Right")]
	public List<Dropdown> colorDropdowns;
	[Tooltip("Порядок: Bottom, Left, Top, Right")]
	public List<Dropdown> typeDropdowns;
	public Button startButton;

	List<Dropdown.OptionData> _sampleDropdownsOptions; // Список опций. Порядок(цвет): З.К.С.Ж.

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	void Start()
	{
		_sampleDropdownsOptions = new List<Dropdown.OptionData>(colorDropdowns[0].options);
		foreach (var cdd in colorDropdowns)
		{
			cdd.options = new List<Dropdown.OptionData>(_sampleDropdownsOptions);
		}
		startButton.interactable = false;
	}

	public void Display()
	{
		gameObject.SetActive(true);
	}

	public bool IsDisplay()
	{
		return gameObject.activeSelf;
	}

	public void Back()
	{
		gameObject.SetActive(false);
		Menu.Instance.Display();
	}
	
	public void StartGame()
	{
		for (int i = 0; i < colorDropdowns.Count; i++)
		{
			if (colorDropdowns[i].value != 0)
			{
				var numberPlayer = _sampleDropdownsOptions.IndexOf(colorDropdowns[i].options[colorDropdowns[i].value]) - 1;// Номер игрока в списке
				GameController.Instance.AddPlayer(GameData.Instance.samplePlayers[numberPlayer], (Map.Sides)i, (Player.Type)typeDropdowns[i].value);
			}
		}
		gameObject.SetActive(false);
		GameController.Instance.StartGame();
	}

	/// <summary>
	/// Обновляем списки выбора цвета игроков
	/// </summary>
	public void UpdateColorDropdowns()
	{
		var selectedOptions = colorDropdowns.Where(dd => dd.value != 0).Select(dd => dd.options[dd.value]);
		foreach (var cdd in colorDropdowns)
		{
			var currentOption = cdd.options[cdd.value];
			cdd.options = _sampleDropdownsOptions.Where(so => so == currentOption || !selectedOptions.Contains(so)).ToList();
			cdd.value = cdd.options.IndexOf(currentOption);

			startButton.interactable = selectedOptions.Any();
		}
	}
}
