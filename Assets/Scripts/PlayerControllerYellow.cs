using UnityEngine;
using System.Collections;

public class PlayerControllerYellow : PlayerController {
	
	public static int countChips;
	
	public override void CountPlus ()
	{
		numberChip = countChips++;
		numberChip += 12;
	}
	
	public override int Count ()
	{
		return countChips;
	}
	
	public override void CounterDecrease(){
		countChips--;
	}
	
	public override void CreateMap(){
		map = new Vector3[74];
		int i = 0;
		int x = 6;
		int z = 0;
		// Общая карта
		map[i++] = new Vector3(7.2f, 0, 0);
		while(z > -6) map[i++] = new Vector3(x, 0, z--);
		while(x > -6) map[i++] = new Vector3(x--, 0, z);
		while(z < 6) map[i++] = new Vector3(x, 0, z++);
		while(x < 6) map[i++] = new Vector3(x++, 0, z);
		while(z > 0) map[i++] = new Vector3(x, 0, z--);
		while(x >= 2) map[i++] = new Vector3(x--, 0, z);
		
		// Карта тунелей
		map[i++] = new Vector3(4.5f, 0, -2);
		map[i++] = new Vector3(2, 0, -4.5f);
		
		map[i++] = new Vector3(-2, 0, -4.5f);
		map[i++] = new Vector3(-4.5f, 0, -2);

		map[i++] = new Vector3(-4.5f, 0, 2);
		map[i++] = new Vector3(-2, 0, 4.5f);
		
		map[i++] = new Vector3(2, 0, 4.5f);
		map[i++] = new Vector3(4.5f, 0, 2);
		
		// Карта толчков
		x = 5;
		z = -3;
		while (z > -6){
			map[i++] = new Vector3(x, 0, z);
			z--;
		}
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
	}
}

