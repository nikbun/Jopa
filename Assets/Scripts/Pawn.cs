using UnityEngine;
using Map;
using System.Collections.Generic;
using System.Collections;
using Map.MapObjects;

public class Pawn : MonoBehaviour, MapPawn
{
	// Визуально показывает, что может ходить
	public SpriteRenderer outline; 
	public Location location { get; set; }
	public PlayerPosition playerPosition { get { return player.playerPosition; } }

	public Player player;
	public bool canMove { get { return outline.enabled; } set { outline.enabled = value; } }
	public Trace trace { get; set; }

	public bool move = false;

	void Start()
	{
		location = Location.Origin;
		canMove = false;
	}

	private void OnMouseDown()
	{
		if (canMove)
		{
			Debug.Log("Фишка начинает движение. Текущая локация - "+location+" Длина пути - "+trace.way.Count);
			player.OffCanMove();
			StartCoroutine(Move());
			move = true;
		}
	}

	private IEnumerator Move()
	{
		for(int i = 0; i < trace.way.Count; i++)
		{
			yield return StartCoroutine( MoveToPoint(trace.way[i]));
		}
		
		if (trace.from != null)
			trace.from.pawn = null;
		if (trace.to != null)
		{
			trace.to.pawn = this;
			location = trace.to.location;
			trace.from = trace.to;
			trace.to = null;
			trace.way = null;
		}
		Debug.Log("Фишка закончила движение. Текущая локация - "+location);
		move = false;
		GameController.instance.EndTurn();
	}

	private IEnumerator MoveToPoint(Vector3 target)
	{
		Debug.Log("Фишка движется к " + target);
		var sqrDistance = (target - transform.position).sqrMagnitude;
		while (sqrDistance > float.Epsilon)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, GameController.instance.gameSpeed * Time.deltaTime);
			sqrDistance = (target - transform.position).sqrMagnitude;
			yield return null;
		}
	}
}