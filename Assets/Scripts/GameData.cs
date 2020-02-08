using MapSpace;
/// <summary>
/// Класс для хранения общих данных
/// </summary>
public class GameData
{
	public MapController mapController { get; private set; } // Игровая карта
	public float speed { get; private set; } // Скорость игры
	public static GameData Instanse  { get  { return _instance == null? _instance = new GameData():_instance; } }
	static GameData _instance;
	GameData() 
	{
	}

	public void Update(MapController mapController, float speed = 10f) 
	{
		Instanse.mapController = mapController;
		Instanse.speed = speed;
	}
}