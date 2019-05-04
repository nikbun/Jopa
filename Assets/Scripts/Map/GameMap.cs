using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Cells;
using Game.MapObjects;

namespace Game
{
	public class Map
	{
		private OuterCircleCell[] outerCircle { get; set; } // Внешний круг, общая зона
		private Dictionary<PlayerPositions, Vector3> startPositions { get; set; } // Стартовые позиции игроков
		private Dictionary<PlayerPositions, Home> home { get; set; } // Дома игроков
		private Dictionary<PlayerPositions, Vector3> jopa { get; set; } // Жопа

		public Map()
		{
			InitStartPositions();
			InitOuterCircle();
			InitHome();
			InitJopa();
		}

		/// <summary>
		/// Инициализация всех расположений объектов на карте
		/// </summary>
		#region Initialization

		private void InitStartPositions()
		{
			startPositions = new Dictionary<PlayerPositions, Vector3>()
			{
				{PlayerPositions.Bottom, new Vector3(0, 0, -7.2f)},
				{PlayerPositions.Left, new Vector3(-7.2f, 0, 0)},
				{PlayerPositions.Top, new Vector3(0, 0, 7.2f)},
				{PlayerPositions.Right, new Vector3(7.2f, 0, 0)}
			};
		}

		private void InitOuterCircle()
		{
			outerCircle = new OuterCircleCell[49];
			int x = 0;
			int y = -6;
			int i = 0;
			do
			{
				outerCircle[i++] = new OuterCircleCell(x, y);
				x--;
			} while (x > -6);
			do
			{
				outerCircle[i++] = new OuterCircleCell(x, y);
				y++;
			} while (y < 6);
			do
			{
				outerCircle[i++] = new OuterCircleCell(x, y);
				x++;
			} while (x < 6);
			do
			{
				outerCircle[i++] = new OuterCircleCell(x, y);
				y--;
			} while (y > -6);
			do
			{
				outerCircle[i++] = new OuterCircleCell(x, y);
				x--;
			} while (x > 0);

			InitTolchok();
			InitShortcut();
		}

		private void InitTolchok()
		{
			outerCircle[3].tolchok = new Tolchok(new Cell(-3, -5), new Cell(-4, -5), new Cell(-5, -5), outerCircle[6]);
			outerCircle[15].tolchok = new Tolchok(new Cell(-5, 3), new Cell(-5, 4), new Cell(-5, 5), outerCircle[18]);
			outerCircle[27].tolchok = new Tolchok(new Cell(3, 5), new Cell(4, 5), new Cell(5, 5), outerCircle[30]);
			outerCircle[39].tolchok = new Tolchok(new Cell(5, -3), new Cell(5, -4), new Cell(5, -5), outerCircle[42]);
		}

		private void InitShortcut()
		{
			outerCircle[2].shortcut = new Shortcut(outerCircle[2], outerCircle[10], new []{ new Vector3(-2, 0, -4.5f), new Vector3(-4.5f, 0, -2) });
			outerCircle[14].shortcut = new Shortcut(outerCircle[14], outerCircle[22], new[] { new Vector3(-4.5f, 0, 2), new Vector3(-2, 0, -4.5f) });
			outerCircle[26].shortcut = new Shortcut(outerCircle[26], outerCircle[34], new[] { new Vector3(2, 0, 4.5f), new Vector3(4.5f, 0, 2) });
			outerCircle[38].shortcut = new Shortcut(outerCircle[38], outerCircle[46], new[] { new Vector3(4.5f, 0, -2), new Vector3(2, 0, -4.5f) });
		}

		private void InitHome()
		{
			home = new Dictionary<PlayerPositions, Home>()
			{
				{PlayerPositions.Bottom, new Home(new Cell(-5,0), new Cell(-4,0), new Cell(-3,0), new Cell(-2,0))},
				{PlayerPositions.Left, new Home(new Cell(0,-5), new Cell(0,-4), new Cell(0,-3), new Cell(0,-2))},
				{PlayerPositions.Top, new Home(new Cell(5,0), new Cell(4,0), new Cell(3,0), new Cell(2,0))},
				{PlayerPositions.Right, new Home(new Cell(0,5), new Cell(0,4), new Cell(0,3), new Cell(0,2))}
			};
		}

		private void InitJopa()
		{
			jopa = new Dictionary<PlayerPositions, Vector3>()
			{
				{PlayerPositions.Bottom, new Vector3(-1, 0, 0)},
				{PlayerPositions.Left, new Vector3(0, 0, -1)},
				{PlayerPositions.Top, new Vector3(1, 0, 0)},
				{PlayerPositions.Right, new Vector3(0, 0, 1)}
			};
		}



		#endregion

		/// <summary>
		/// Получить стартовую позицию игрока
		/// </summary>
		/// <param name="playerPositions"> Расположение игрока на поле </param>
		/// <returns> Стартовая позиция игрока относительно расположения</returns>
		public Vector3 GetStartPosition(PlayerPositions playerPositions)
		{
			return startPositions[playerPositions];
		}
	}

	/// <summary>
	/// Расположение игроков на карте
	/// </summary>
	public enum PlayerPositions
	{
		Bottom,
		Left,
		Top,
		Right
	}
}