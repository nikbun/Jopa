namespace Map
{
	/// <summary>
	/// Пешка созданная для работы с картой
	/// </summary>
	public interface MapPawn
	{
		/// <summary>
		/// Флаг определяющий, что пешка прошла начальную позицию в круге
		/// Если true, то начальная позиция становиться конечной и из нее переводит пешку в дом
		/// </summary>
		bool inGame { get; set; }
		/// <summary>
		/// Показывает игроку, что пешка может двигаться
		/// </summary>
		bool readyStartMoving { get; set; }
		/// <summary>
		/// Путь движения фишки
		/// </summary>
		Trace trace { get; set; }
		/// <summary>
		///Локация карты в которой сейчас находиться пешка 
		/// </summary>
		Location location { get; set; }
		/// <summary>
		/// Позиция игрока
		/// </summary>
		PlayerPosition playerPosition { get; }
		/// <summary>
		/// Смещение пешки вызываеться, когда пешку смещает другая пешка
		/// </summary>
		void Shift();
		void SetTrace(bool canMove, Trace trace = null);
	}
}