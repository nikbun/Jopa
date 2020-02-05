using UnityEngine;
using Map;
using System.Collections;

public class Pawn : MonoBehaviour
{
	public SpriteRenderer outline; // Подсветка, в случае, если фишкой можно ходить
	public MapSides mapSide;

	public delegate void PawnMovement();
	public event PawnMovement StartMovement;
	public event PawnMovement StopMovement;

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
		m_Tracker = new Tracker(mapSide, GameData.instance.map);
		m_Tracker.ShiftMove += StartMove;
	}

	void OnMouseDown()
	{
		if (m_Tracker.readyStartMoving)
		{
			StartMove();
		}
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
			var target = m_Tracker.GetNextTarget();

			float sqrDistance;
			do {
				sqrDistance = (target.position - transform.position).sqrMagnitude;
				transform.position = Vector3.MoveTowards(transform.position, target.position, GameData.instance.speed * Time.deltaTime);
				yield return null;
			} while (sqrDistance > float.Epsilon);
		}
		StopMovement?.Invoke();
	}

	public bool IsMoving()
	{
		return m_Moving;
	}
}