using UnityEngine;
using Map;
using System.Collections;

public class Pawn : MonoBehaviour
{
	public SpriteRenderer outline; // Подсветка, в случае, если фишкой можно ходить
	public PlayerPosition playerPosition;

	public delegate void PawnMove();
	public event PawnMove StartMovement;
	public event PawnMove StopMovement;

	Tracker m_Tracker;
	bool m_Moving;

	public Pawn ()
	{
		StartMovement = () => m_Moving = true;
		StopMovement = () => m_Moving = false;
	}

	void Start()
	{
		outline.enabled = false;
		m_Tracker = new Tracker(playerPosition, GameData.instance.map);
		m_Tracker.ShiftMove += StartMove;
	}

	void OnMouseDown()
	{
		if (m_Tracker.readyStartMoving)
		{
			StartMove();
		}
	}

	public bool IsMoving()
	{
		return m_Moving;
	}

	public bool CanStartMove(int distance) 
	{
		return outline.enabled = m_Tracker.CanStartMove(distance);
	}

	public void CancelStartMove()
	{
		m_Tracker.readyStartMoving = false;
		outline.enabled = false;
	}

	/// <summary>
	///  начать движение
	/// </summary>
	/// <param name="canHit"></param>
	public void StartMove()
	{
		StartCoroutine(Move());
	}

	IEnumerator Move()
	{
		StartMovement?.Invoke();
		while (m_Tracker.HasNextTarget())
		{
			var target = m_Tracker.GetNextTarger();

			float sqrDistance;
			do {
				sqrDistance = (target.point - transform.position).sqrMagnitude;
				transform.position = Vector3.MoveTowards(transform.position, target.point, GameData.instance.speed * Time.deltaTime);
				yield return null;
			} while (sqrDistance > float.Epsilon);
		}
		StopMovement?.Invoke();
	}
}