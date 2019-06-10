using UnityEngine;
using Map;
using System.Collections.Generic;
using System.Collections;
using Map.MapObjects;

public class Pawn : MonoBehaviour, MapPawn
{
	// Визуально показывает, что может ходить
	public SpriteRenderer outline;
	public int number;
	public Location location { get; set; }
	public PlayerPosition playerPosition { get { return player.playerPosition; } }

	public Player player;
	public bool canMove { get { return outline.enabled; } set { outline.enabled = value; } }
	public bool inGame { get; set; }
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
			player.OffCanMove();
			move = true;
			StartCoroutine(Move(location != Location.Jopa && trace.to?.location != Location.Jopa));
		}
	}

	private IEnumerator Move(bool withHit = true)
	{
		for(int i = 0; i < trace.way.Count; i++)
		{
			if (withHit)
				HitOtherPawn(trace.way[i]);
			yield return StartCoroutine( MoveToPoint(trace.way[i]));
		}

		if (!inGame && location == Location.Circle)
			inGame = true;
		trace.ResetTrace(this, true);
		move = false;
		GameController.instance.EndTurn();
	}

	private IEnumerator MoveToPoint(Vector3 target)
	{
		var sqrDistance = (target - transform.position).sqrMagnitude;
		while (sqrDistance > float.Epsilon)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, GameController.instance.gameSpeed * Time.deltaTime);
			sqrDistance = (target - transform.position).sqrMagnitude;
			yield return null;
		}
	}

	public void Shift()
	{
		if (location == Location.Tolchok)
		{
			trace.ResetTrace(saveFrom:true);
			trace = player.gameMap.GetTolchokTraceToNext(this);
			move = true;
			StartCoroutine(Move(true));
		}
		else
		{
			var pos = player.gameMap.GetJopaPosition(playerPosition);
			trace.ResetTrace(this);
			trace.way.Add(pos);
			location = Location.Jopa;
			inGame = false;
			move = true;
			StartCoroutine(Move(false));
		}
	}

	private void HitOtherPawn(Vector3 target)
	{
		Vector3 direction = target - transform.position;
		var hits = Physics.RaycastAll(transform.position, direction.normalized, direction.magnitude, 1 << gameObject.layer);
		if (hits.Length > 0)
		{
			hits[0].transform.GetComponent<MapPawn>().Shift();
		}
	}

	public void SetTrace(bool canMove, Trace trace = null)
	{
		this.canMove = canMove;
		this.trace = trace!=null?trace:this.trace;
	}
}