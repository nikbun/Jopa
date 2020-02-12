using UnityEngine;
using UnityEngine.UI;


public class GameStatus : MonoBehaviour
{
	public static GameStatus Instance { get; private set; }
	public Text playerNameText;
	public GameObject winnerMessage;
	public Text winnerNameText;

	void Awake()
	{
		Instance = this;
		winnerMessage.SetActive(false);
	}

	public void SetCurrentPlayerName(string name, Color color) 
	{
		playerNameText.text = name;
		playerNameText.color = color;
	}

	public void SetWinner(string name, Color color) 
	{
		winnerNameText.text = name;
		winnerNameText.color = color;
		winnerMessage.SetActive(true);
	}

	public void Reset() 
	{
		winnerMessage.SetActive(false);
	}
}