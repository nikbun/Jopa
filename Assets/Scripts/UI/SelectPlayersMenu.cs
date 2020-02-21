using MapSpace;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayersMenu : MonoBehaviour
{
	[Tooltip("Кнопка начала игры")]
	[SerializeField] Button _startGameButton;

	[Header("Порядок списков(side) B.L.T.R.")]
	[Tooltip("Списки выбора цвета игроков. Порядок: Bottom, Left, Top, Right")]
	[SerializeField] List<Dropdown> _playerColorDropdowns;

	[Tooltip("Списки выбора типа игрока. Порядок: Bottom, Left, Top, Right")]
	[SerializeField] List<Dropdown> _playerTypeDropdowns;

	List<Dropdown.OptionData> _sampleDropdownsOptions; // Список опций. Порядок(цвет): З.К.С.Ж.

	public bool IsDisplay { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	void Start()
	{
		_sampleDropdownsOptions = new List<Dropdown.OptionData>(_playerColorDropdowns[0].options);
		foreach (var cdd in _playerColorDropdowns)
		{
			cdd.options = new List<Dropdown.OptionData>(_sampleDropdownsOptions);
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
		for (int i = 0; i < _playerColorDropdowns.Count; i++)
		{
			if (_playerColorDropdowns[i].value != 0)
			{
				var numberPlayer = _sampleDropdownsOptions.IndexOf(_playerColorDropdowns[i].options[_playerColorDropdowns[i].value]) - 1;// Номер игрока в списке
				GameController.Instance.AddPlayer(GameData.Instance.SamplePlayers[numberPlayer], (Map.Sides)i, (Player.Type)_playerTypeDropdowns[i].value);
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
		var selectedOptions = _playerColorDropdowns.Where(dd => dd.value != 0).Select(dd => dd.options[dd.value]);
		foreach (var cdd in _playerColorDropdowns)
		{
			var currentOption = cdd.options[cdd.value];
			cdd.options = _sampleDropdownsOptions.Where(so => so == currentOption || !selectedOptions.Contains(so)).ToList();
			cdd.value = cdd.options.IndexOf(currentOption);

			_startGameButton.interactable = selectedOptions.Any();
		}
	}
}
