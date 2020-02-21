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

	[Space(order = 0)]
	[Header("Экземпляры игроков: Порядок(цвет) З.К.С.Ж.", order = 1)]
	[Tooltip("Порядок: Зеленый, Красный, Синий, Желтый")]
	[SerializeField] List<GameObject> _samplePlayers;

	public static GameData Instance { get; private set; }

	public float PawnSpeed { get => _pawnSpeed; }

	public float ReactionAI { get => _reactionAI; }

	public MapController MapController { get; }

	public GameObject GameUI { get => _gameUI; }

	public List<GameObject> SamplePlayers { get => _samplePlayers; }


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