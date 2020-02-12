using MapSpace;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayersMenu : MonoBehaviour
{
	public List<Dropdown> dropdowns;
	public Button startButton;

	List<Dropdown.OptionData> _sampleDropdownsOptions; // Список опций. Порядок(цвет): З.К.С.Ж.

	void Start()
	{
		gameObject.SetActive(false);
		_sampleDropdownsOptions = new List<Dropdown.OptionData>(dropdowns[0].options);
		foreach (var dd in dropdowns)
		{
			dd.options = new List<Dropdown.OptionData>(_sampleDropdownsOptions);
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
		Dictionary<Map.Sides, GameObject> players = new Dictionary<Map.Sides, GameObject>();
		for (int i = 0; i < dropdowns.Count; i++)
		{
			if (dropdowns[i].value != 0)
			{
				var numberPlayer = _sampleDropdownsOptions.IndexOf(dropdowns[i].options[dropdowns[i].value]) - 1;// Номер игрока в списке
				players.Add((Map.Sides)i, GameData.Instance.samplePlayers[numberPlayer]);
			}
		}
		gameObject.SetActive(false);
		GameController.Instance.StartGame(players);
	}

	/// <summary>
	/// Обновляем списки выбора игроков
	/// </summary>
	public void UpdateDropdowns()
	{
		var selectedOptions = dropdowns.Where(dd => dd.value != 0).Select(dd => dd.options[dd.value]);
		foreach (var dd in dropdowns)
		{
			var currentOption = dd.options[dd.value];
			dd.options = _sampleDropdownsOptions.Where(so => so == currentOption || !selectedOptions.Contains(so)).ToList();
			dd.value = dd.options.IndexOf(currentOption);

			startButton.interactable = selectedOptions.Any();
		}
	}
}
