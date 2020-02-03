using UnityEngine;
using Map;
using System.Collections.Generic;
using System.Collections;
using Map.MapObjects;

public class Pawn : MonoBehaviour
{
	public SpriteRenderer outline; // Подсветка, в случае, если фишкой можно ходить
	public PlayerPosition playerPosition;

	public delegate void PawnMove();
	public event PawnMove StartMove;
	public event PawnMove StopMove;

	bool m_Moving = false;
	Tracker m_Tracker;

	public bool readyStartMoving { 
		get { return outline.enabled; } 
		set 
		{
			outline.enabled = value;
		} 
	}

	void Start()
	{
		readyStartMoving = false;
		m_Tracker = new Tracker(playerPosition);
		m_Tracker.ShiftMove += Shift;
	}

	void OnMouseDown()
	{
		if (readyStartMoving)
		{
			StartMove?.Invoke();
			m_Moving = true;
			StartCoroutine(Move(m_Tracker.location != Location.Jopa && m_Tracker.trace.to?.location != Location.Jopa));
		}
	}

	public bool CanStartMove(int distance) 
	{
		m_Tracker.trace.way.Clear();
		m_Tracker.trace.to = null;
		readyStartMoving = GameData.instance.map.CanMove(m_Tracker, distance);
		return readyStartMoving;
	}

	IEnumerator Move(bool withHit = true)
	{
		for(int i = 0; i < m_Tracker.trace.way.Count; i++)
		{
			if (withHit)
				HitOtherPawn(m_Tracker.trace.way[i].point);
			yield return StartCoroutine( MoveToPoint(m_Tracker.trace.way[i].point));
		}

		if (!m_Tracker.inGame && m_Tracker.location == Location.Circle)
			m_Tracker.inGame = true;
		m_Tracker.trace.ResetTrace(m_Tracker, true);
		m_Moving = false;
		StopMove?.Invoke();
	}

	private IEnumerator MoveToPoint(Vector3 target)
	{
		var sqrDistance = (target - transform.position).sqrMagnitude;
		while (sqrDistance > float.Epsilon)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, GameData.instance.speed * Time.deltaTime);
			sqrDistance = (target - transform.position).sqrMagnitude;
			yield return null;
		}
	}

	public void Shift(bool withHit)
	{
		StartMove?.Invoke();
		m_Moving = true;
		StartCoroutine(Move(withHit));
	}

	private void HitOtherPawn(Vector3 target)
	{
		Vector3 direction = target - transform.position;
		var hits = Physics.RaycastAll(transform.position, direction.normalized, direction.magnitude, 1 << gameObject.layer);
		if (hits.Length > 0)
		{
			hits[0].transform.GetComponent<Pawn>().m_Tracker.Shift();
		}
	}


	public bool IsMoving() 
	{
		return m_Moving;
	}
}