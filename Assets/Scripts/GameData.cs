using MapSpace;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для хранения общих данных
/// </summary>
public sealed class GameData : MonoBehaviour
{
	[Tooltip("Скорость передвижения фишек")]
	[SerializeField] float _pawnSpeed = 10f;

	[Tooltip("Реакция AI (sec)")]
	[SerializeField] float _reactionAI = 0.5f;

	[Tooltip("Интерфейс пользователя")]
	[SerializeField] GameObject _gameUI;

	[Tooltip("Информация об игроках")]
	[SerializeField] List<PlayerData> _players;

	public static GameData Instance { get; private set; }

	public float PawnSpeed { get => _pawnSpeed; }

	public float ReactionAI { get => _reactionAI; }

	public MapController MapController { get; }

	public GameObject GameUI { get => _gameUI; }

	public List<PlayerData> Players { get => _players; }

	GameData()
	{
		MapController = new MapController(); 
	}

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else 
		{
			Destroy(gameObject);
		}
	}
}


[System.Serializable]
public class PlayerData
{
	[SerializeField] string _name;
	[SerializeField] Color _color;
	[SerializeField] GameObject _samplePlayer;
	[SerializeField] GameObject _samplePawn;
	[SerializeField] Sprite _pawnSprite;

	public string Name { get => _name; }

	public Color Color { get => _color; }

	public GameObject SamplePlayer { get => _samplePlayer; }

	public GameObject SamplePawn { get => _samplePawn; }

	public Sprite PawnSprite { get => _pawnSprite; }
}