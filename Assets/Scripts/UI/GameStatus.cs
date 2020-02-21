using UnityEngine;
using UnityEngine.UI;


public class GameStatus : MonoBehaviour
{
	[SerializeField] Text _playerNameText;
	[SerializeField] Text _winnerNameText;
	[SerializeField] GameObject _winnerMessage;

	public static GameStatus Instance { get; private set; }

	void Awake()
	{
		Instance = this;
		_winnerMessage.SetActive(false);
	}

	public void SetCurrentPlayerName(string name, Color color) 
	{
		_playerNameText.text = name;
		_playerNameText.color = color;
	}

	public void SetWinner(string name, Color color) 
	{
		_winnerNameText.text = name;
		_winnerNameText.color = color;
		_winnerMessage.SetActive(true);
	}

	public void Reset() 
	{
		_winnerMessage.SetActive(false);
	}
}