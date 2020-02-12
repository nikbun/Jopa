using MapSpace;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для хранения общих данных
/// </summary>
public class GameData : MonoBehaviour
{
	public static GameData Instance { get; private set; }
	public float pawnSpeed = 10f; // Скорость передвижения фишек
	public GameObject gameUI; // Пользовательский интерфейс
	public readonly MapController mapController; // Игровая карта
	[Space(order = 0)]
	[Header("Экземпляры игроков: Порядок(цвет) З.К.С.Ж.", order = 1)]
	[Tooltip("Порядок: Зеленый, Красный, Синий, Желтый")]
	public List<GameObject> samplePlayers; // Экземпляры игроков

	GameData()
	{
		mapController = new MapController(); 
	}

	void Awake()
	{
		Instance = this;
	}
}