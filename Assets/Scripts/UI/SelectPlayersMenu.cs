using MapSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayersMenu : MonoBehaviour
{
	[Tooltip("Кнопка начала игры")]
	[SerializeField] Button _startGameButton;
	
	[Tooltip("Списки параметров игроков")]
	[SerializeField] List<PlayerDropdowns> _playerDropdowns;

	readonly List<PlayerOptionData> _samplePlayerDropdownsOptions = new List<PlayerOptionData>();

	public bool IsDisplay { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	void Start()
	{
		_samplePlayerDropdownsOptions.Add(new PlayerOptionData("Отсутствует")); // TODO: Вывести текст в файл локализации
		foreach (var player in GameData.Instance.Players)
		{
			_samplePlayerDropdownsOptions.Add(new PlayerOptionData(player));
		}

		foreach (var pd in _playerDropdowns)
		{
			pd.playerDropdown.options = new List<Dropdown.OptionData>(_samplePlayerDropdownsOptions);
			pd.typeDropdown.options.Clear();
			foreach (var type in Enum.GetValues(typeof(Player.Type)))
			{
				pd.typeDropdown.options.Add(new Dropdown.OptionData(type.ToString()));
			}
		}
		_startGameButton.interactable = false;
	}

	public void Back()
	{
		gameObject.SetActive(false);
		Menu.Instance.Display();
	}
	
	public void StartGame()
	{
		_playerDropdowns.ForEach(pd => 
		{
			if (pd.playerDropdown.value > 0) 
			{
				GameController.Instance.AddPlayer(
					((PlayerOptionData)pd.playerDropdown.options[pd.playerDropdown.value]).player,
					pd.side,
					(Player.Type)Enum.Parse(typeof(Player.Type), pd.typeDropdown.options[pd.typeDropdown.value].text)
				);
			}
		});
		gameObject.SetActive(false);
		GameController.Instance.StartGame();
	}

	/// <summary>
	/// Обновляем списки выбора цвета игроков
	/// </summary>
	public void UpdateColorDropdowns()
	{
		var selectedOptions = _playerDropdowns.Select(pd => pd.playerDropdown).Where(d => d.value != 0).Select(d => d.options[d.value]);
		foreach (var cdd in _playerDropdowns.Select(pd => pd.playerDropdown))
		{
			var currentOption = cdd.options[cdd.value];
			cdd.options = _samplePlayerDropdownsOptions.Where(so => so == currentOption || !selectedOptions.Contains(so)).Cast<Dropdown.OptionData>().ToList();
			cdd.value = cdd.options.IndexOf(currentOption);

			_startGameButton.interactable = selectedOptions.Any();
		}
	}

	[System.Serializable]
	class PlayerDropdowns
	{
		public Map.Sides side;
		public Dropdown playerDropdown;
		public Dropdown typeDropdown;
	}

	class PlayerOptionData : Dropdown.OptionData
	{
		PlayerData _playerData;

		public GameObject player { get => _playerData.SamplePlayer; }

		public PlayerOptionData(PlayerData playersData) 
		{
			_playerData = playersData;
			text = playersData.Name;
			image = playersData.PawnSprite;
		}

		public PlayerOptionData(string text):base(text) {}
	}
}
