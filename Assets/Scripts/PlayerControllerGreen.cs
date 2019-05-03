using UnityEngine;
using System.Collections;

public class PlayerControllerGreen : PlayerController {
	public static int countChips;

	public override void CountPlus ()
	{
		numberChip = countChips++;
	}

	public override int Count ()
	{
		return countChips;
	}

	public override void CounterDecrease(){
		countChips--;
	}

	public override void  CreateMap(){
		map = new Vector3[74];
		int i = 0;
		int x = 0;
		int z = -6;
		// Общая карта
		map[i] = new Vector3(0, 0, -7.2f);
		i++;
		while(x > -6){
			map[i] = new Vector3(x, 0, z);
			i++;
			x--;
		}
		while(z < 6){
			map[i] = new Vector3(x, 0, z);
			i++;
			z++;
		}
		while(x < 6){
			map[i] = new Vector3(x, 0, z);
			i++;
			x++;
		}
		while(z > -6){
			map[i] = new Vector3(x, 0, z);
			i++;
			z--;
		}
		while(x > 0){
			map[i] = new Vector3(x, 0, z);
			i++;
			x--;
		}
		while(z <= -2){
			map[i] = new Vector3(x, 0, z);
			i++;
			z++;
		}

		// Карта тунелей
		map[i++] = new Vector3(-2, 0, -4.5f);
		map[i++] = new Vector3(-4.5f, 0, -2);

		map[i++] = new Vector3(-4.5f, 0, 2);
		map[i++] = new Vector3(-2, 0, 4.5f);

		map[i++] = new Vector3(2, 0, 4.5f);
		map[i++] = new Vector3(4.5f, 0, 2);

		map[i++] = new Vector3(4.5f, 0, -2);
		map[i++] = new Vector3(2, 0, -4.5f);

		// Карта толчков
		x = -3;
		z = -5;
		while (x > -6){
			map[i++] = new Vector3(x, 0, z);
			x--;
		}
		x = -5;
		z = 3;
		while (z < 6){
			map[i++] = new Vector3(x, 0, z);
			z++;
		}
		x = 3;
		z = 5;
		while (x < 6){
			map[i++] = new Vector3(x, 0, z);
			x++;
		}
		x = 5;
		z = -3;
		while (z > -6){
			map[i++] = new Vector3(x, 0, z);
			z--;
		}
	}
}