using MapSpace.MapObjects;
using System.Collections.Generic;

namespace MapSpace
{
	public class MapController
	{
		Dictionary<int, Cell> _coordinates;
		Dictionary<Map.Sides, Map> _maps;

		/// <summary>
		/// Игровая карта
		/// Загружает локации и позволяет пработать с ними
		/// </summary>
		public MapController()
		{
			InitCoordinates();
			InitMaps();
		}

		#region Initialization
		/// <summary>
		/// Создаем набор ячеек из которых будем стоить карты
		/// </summary>
		void InitCoordinates()
		{
			_coordinates = new Dictionary<int, Cell>();
			// Круг
			// Нижняя сторона
			_coordinates.Add(0, new Cell(6, -6, Map.Locations.Circle));// Толчек 4 выход
			_coordinates.Add(1, new Cell(5, -6, Map.Locations.Circle));
			_coordinates.Add(2, new Cell(4, -6, Map.Locations.Circle));
			_coordinates.Add(3, new Cell(3, -6, Map.Locations.Circle));
			_coordinates.Add(4, new Cell(2, -6, Map.Locations.Cut)); // Срез 4
			_coordinates.Add(5, new Cell(1, -6, Map.Locations.Circle));
			_coordinates.Add(6, new Cell(0, -6, Map.Locations.Circle)); // Нижний вход в круг
			_coordinates.Add(7, new Cell(-1, -6, Map.Locations.Circle));
			_coordinates.Add(8, new Cell(-2, -6, Map.Locations.Cut)); // Срез 1
			_coordinates.Add(9, new Cell(-3, -6, Map.Locations.Tolchok)); // Толчек 1 вход
			_coordinates.Add(10, new Cell(-4, -6, Map.Locations.Circle));
			_coordinates.Add(11, new Cell(-5, -6, Map.Locations.Circle));
			// Левая сторона
			_coordinates.Add(12, new Cell(-6, -6, Map.Locations.Circle));// Толчек 1 выход
			_coordinates.Add(13, new Cell(-6, -5, Map.Locations.Circle));
			_coordinates.Add(14, new Cell(-6, -4, Map.Locations.Circle));
			_coordinates.Add(15, new Cell(-6, -3, Map.Locations.Circle));
			_coordinates.Add(16, new Cell(-6, -2, Map.Locations.Cut)); // Срез 1
			_coordinates.Add(17, new Cell(-6, -1, Map.Locations.Circle));
			_coordinates.Add(18, new Cell(-6, 0, Map.Locations.Circle)); // Левый вход в круг
			_coordinates.Add(19, new Cell(-6, 1, Map.Locations.Circle));
			_coordinates.Add(20, new Cell(-6, 2, Map.Locations.Cut)); // Срез 2
			_coordinates.Add(21, new Cell(-6, 3, Map.Locations.Tolchok)); // Толчек 2 вход
			_coordinates.Add(22, new Cell(-6, 4, Map.Locations.Circle));
			_coordinates.Add(23, new Cell(-6, 5, Map.Locations.Circle));
			// Верхняя сторона
			_coordinates.Add(24, new Cell(-6, 6, Map.Locations.Circle));// Толчек 2 выход
			_coordinates.Add(25, new Cell(-5, 6, Map.Locations.Circle));
			_coordinates.Add(26, new Cell(-4, 6, Map.Locations.Circle));
			_coordinates.Add(27, new Cell(-3, 6, Map.Locations.Circle));
			_coordinates.Add(28, new Cell(-2, 6, Map.Locations.Cut)); // Срез 2
			_coordinates.Add(29, new Cell(-1, 6, Map.Locations.Circle));
			_coordinates.Add(30, new Cell(0, 6, Map.Locations.Circle)); // Верхний вход в круг
			_coordinates.Add(31, new Cell(1, 6, Map.Locations.Circle));
			_coordinates.Add(32, new Cell(2, 6, Map.Locations.Cut)); // Срез 3
			_coordinates.Add(33, new Cell(3, 6, Map.Locations.Tolchok)); // Толчек 3 вход
			_coordinates.Add(34, new Cell(4, 6, Map.Locations.Circle));
			_coordinates.Add(35, new Cell(5, 6, Map.Locations.Circle));
			// Правая сторона
			_coordinates.Add(36, new Cell(6, 6, Map.Locations.Circle));// Толчек 3 выход
			_coordinates.Add(37, new Cell(6, 5, Map.Locations.Circle));
			_coordinates.Add(38, new Cell(6, 4, Map.Locations.Circle));
			_coordinates.Add(39, new Cell(6, 3, Map.Locations.Circle));
			_coordinates.Add(40, new Cell(6, 2, Map.Locations.Cut)); // Срез 3
			_coordinates.Add(41, new Cell(6, 1, Map.Locations.Circle));
			_coordinates.Add(42, new Cell(6, -0, Map.Locations.Circle)); // Правый вход в круг
			_coordinates.Add(43, new Cell(6, -1, Map.Locations.Circle));
			_coordinates.Add(44, new Cell(6, -2, Map.Locations.Cut)); // Срез 4
			_coordinates.Add(45, new Cell(6, -3, Map.Locations.Tolchok)); // Толчек 4 вход
			_coordinates.Add(46, new Cell(6, -4, Map.Locations.Circle));
			_coordinates.Add(47, new Cell(6, -5, Map.Locations.Circle));

			// Толчек 1
			_coordinates.Add(48, new Cell(-3, -5, Map.Locations.Tolchok, 1));
			_coordinates.Add(49, new Cell(-4, -5, Map.Locations.Tolchok, 3));
			_coordinates.Add(50, new Cell(-5, -5, Map.Locations.Tolchok, 6));
			// Толчек 2
			_coordinates.Add(51, new Cell(-5, 3, Map.Locations.Tolchok, 1));
			_coordinates.Add(52, new Cell(-5, 4, Map.Locations.Tolchok, 3));
			_coordinates.Add(53, new Cell(-5, 5, Map.Locations.Tolchok, 6));
			// Толчек 3
			_coordinates.Add(54, new Cell(3, 5, Map.Locations.Tolchok, 1));
			_coordinates.Add(55, new Cell(4, 5, Map.Locations.Tolchok, 3));
			_coordinates.Add(56, new Cell(5, 5, Map.Locations.Tolchok, 6));
			// Толчек 4
			_coordinates.Add(57, new Cell(5, -3, Map.Locations.Tolchok, 1));
			_coordinates.Add(58, new Cell(5, -4, Map.Locations.Tolchok, 3));
			_coordinates.Add(59, new Cell(5, -5, Map.Locations.Tolchok, 6));

			// Срез 1
			_coordinates.Add(60, new Cell(-2, -4.5f, Map.Locations.Tolchok));
			_coordinates.Add(61, new Cell(-4.5f, -2, Map.Locations.Tolchok));
			// Срез 2
			_coordinates.Add(62, new Cell(-4.5f, 2, Map.Locations.Tolchok));
			_coordinates.Add(63, new Cell(-2, 4.5f, Map.Locations.Tolchok));
			// Срез 3
			_coordinates.Add(64, new Cell(2, 4.5f, Map.Locations.Tolchok));
			_coordinates.Add(65, new Cell(4.5f, 2, Map.Locations.Tolchok));
			// Срез 4
			_coordinates.Add(66, new Cell(4.5f, -2, Map.Locations.Tolchok));
			_coordinates.Add(67, new Cell(2, -4.5f, Map.Locations.Tolchok));

			// Нижняя сторона
			// Начальная точка
			_coordinates.Add(68, new Cell(0, -7.2f, Map.Locations.Origin, 6));
			// Дом
			_coordinates.Add(69, new Cell(0, -5, Map.Locations.Home));
			_coordinates.Add(70, new Cell(0, -4, Map.Locations.Home));
			_coordinates.Add(71, new Cell(0, -3, Map.Locations.Home));
			_coordinates.Add(72, new Cell(0, -2, Map.Locations.Home));
			// Жопа
			_coordinates.Add(73, new Cell(0, -1, Map.Locations.Jopa, 6));

			// Левая сторона
			// Начальная точка
			_coordinates.Add(74, new Cell(-7.2f, 0, Map.Locations.Origin, 6));
			// Дом
			_coordinates.Add(75, new Cell(-5, 0, Map.Locations.Home));
			_coordinates.Add(76, new Cell(-4, 0, Map.Locations.Home));
			_coordinates.Add(77, new Cell(-3, 0, Map.Locations.Home));
			_coordinates.Add(78, new Cell(-2, 0, Map.Locations.Home));
			// Жопа
			_coordinates.Add(79, new Cell(-1, 0, Map.Locations.Jopa, 6));

			// Верхняя сторона
			// Начальная точка
			_coordinates.Add(80, new Cell(0, 7.2f, Map.Locations.Origin, 6));
			// Дом
			_coordinates.Add(81, new Cell(0, 5, Map.Locations.Home));
			_coordinates.Add(82, new Cell(0, 4, Map.Locations.Home));
			_coordinates.Add(83, new Cell(0, 3, Map.Locations.Home));
			_coordinates.Add(84, new Cell(0, 2, Map.Locations.Home));
			// Жопа
			_coordinates.Add(85, new Cell(0, 1, Map.Locations.Jopa, 6));

			// Правая сторона
			// Начальная точка
			_coordinates.Add(86, new Cell(7.2f, 0, Map.Locations.Origin, 6));
			// Дом
			_coordinates.Add(87, new Cell(5, 0, Map.Locations.Home));
			_coordinates.Add(88, new Cell(4, 0, Map.Locations.Home));
			_coordinates.Add(89, new Cell(3, 0, Map.Locations.Home));
			_coordinates.Add(90, new Cell(2, 0, Map.Locations.Home));
			// Жопа
			_coordinates.Add(91, new Cell(1, 0, Map.Locations.Jopa, 6));

			//Выходы из круга
			_coordinates.Add(92, new CellNested(_coordinates[6])); // Нижний
			_coordinates.Add(93, new CellNested(_coordinates[18])); // Левый
			_coordinates.Add(94, new CellNested(_coordinates[30])); // Верхний
			_coordinates.Add(95, new CellNested(_coordinates[42])); // Правый
		}

		void InitMaps()
		{
			_maps = new Dictionary<Map.Sides, Map>();
			// Нижняя
			_maps.Add(
				Map.Sides.Bottom,
				new Map.MapBuilder(_coordinates)
				.Circle(6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17,
					18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 
					30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 
					42, 43, 44, 45, 46, 47, 0, 1, 2, 3, 4, 5, 92)
				.Origin(68)
				.Home(69, 70, 71, 72)
				.Jopa(73)
				.Tolchok(9, 12, 48, 49, 50)
				.Tolchok(21, 24, 51, 52, 53)
				.Tolchok(33, 36, 54, 55, 56)
				.Tolchok(45, 0, 57, 58, 59)
				.Cut(8, 16, 60, 61).Cut(16, 8, 61, 60)
				.Cut(20, 28, 62, 63).Cut(28, 20, 63, 62)
				.Cut(32, 40, 64, 65).Cut(40, 32, 65, 64)
				.Cut(44, 4, 66, 67).Cut(4, 44, 67, 66)
				.Build()
				);
			// Левая
			_maps.Add(
				Map.Sides.Left,
				new Map.MapBuilder(_coordinates)
				.Circle(18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
					30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 
					42, 43, 44, 45, 46, 47, 0, 1, 2, 3, 4, 5,
					6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 93)
				.Origin(74)
				.Home(75, 76, 77, 78)
				.Jopa(79)
				.Tolchok(9, 12, 48, 49, 50)
				.Tolchok(21, 24, 51, 52, 53)
				.Tolchok(33, 36, 54, 55, 56)
				.Tolchok(45, 0, 57, 58, 59)
				.Cut(8, 16, 60, 61).Cut(16, 8, 61, 60)
				.Cut(20, 28, 62, 63).Cut(28, 20, 63, 62)
				.Cut(32, 40, 64, 65).Cut(40, 32, 65, 64)
				.Cut(44, 4, 66, 67).Cut(4, 44, 67, 66)
				.Build()
				);
			// Верх
			_maps.Add(
				Map.Sides.Top,
				new Map.MapBuilder(_coordinates)
				.Circle(30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41,
					42, 43, 44, 45, 46, 47, 0, 1, 2, 3, 4, 5,
					6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 
					18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 94)
				.Origin(80)
				.Home(81, 82, 83, 84)
				.Jopa(85)
				.Tolchok(9, 12, 48, 49, 50)
				.Tolchok(21, 24, 51, 52, 53)
				.Tolchok(33, 36, 54, 55, 56)
				.Tolchok(45, 0, 57, 58, 59)
				.Cut(8, 16, 60, 61).Cut(16, 8, 61, 60)
				.Cut(20, 28, 62, 63).Cut(28, 20, 63, 62)
				.Cut(32, 40, 64, 65).Cut(40, 32, 65, 64)
				.Cut(44, 4, 66, 67).Cut(4, 44, 67, 66)
				.Build()
				);

			_maps.Add(
				Map.Sides.Right,
				new Map.MapBuilder(_coordinates)
				.Circle(42, 43, 44, 45, 46, 47, 0, 1, 2, 3, 4, 5,
					6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17,
					18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 
					30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 95)
				.Origin(86)
				.Home(87, 88, 89, 90)
				.Jopa(91)
				.Tolchok(9, 12, 48, 49, 50)
				.Tolchok(21, 24, 51, 52, 53)
				.Tolchok(33, 36, 54, 55, 56)
				.Tolchok(45, 0, 57, 58, 59)
				.Cut(8, 16, 60, 61).Cut(16, 8, 61, 60)
				.Cut(20, 28, 62, 63).Cut(28, 20, 63, 62)
				.Cut(32, 40, 64, 65).Cut(40, 32, 65, 64)
				.Cut(44, 4, 66, 67).Cut(4, 44, 67, 66)
				.Build()
				);
		}
		#endregion

		/// <summary>
		/// Получить карту одной из сторон
		/// </summary>
		/// <param name="side"></param>
		/// <returns></returns>
		public Map GetMap(Map.Sides side) 
		{
			return _maps[side];
		}
	}
}