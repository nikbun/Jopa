using UnityEngine;
using Map;
using System.Collections;

public class Pawn : MonoBehaviour
{
	public SpriteRenderer outline; // Подсветка, в случае, если фишкой можно ходить
	public PlayerPosition playerPosition;

	public delegate void PawnMove();
	public event PawnMove StartMove;
	public event PawnMove StopMove;

	Tracker m_Tracker;
	bool m_Moving;

	public Pawn ()
	{
		StartMove = () => m_Moving = true;
		StopMove = () => m_Moving = false;
	}

	void Start()
	{
		outline.enabled = false;
		m_Tracker = new Tracker(playerPosition, GameData.instance.map);
		m_Tracker.ShiftMove += Shift;
	}

	void OnMouseDown()
	{
		if (m_Tracker.readyStartMoving)
		{
			StartCoroutine(Move(m_Tracker.canHit));
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

	IEnumerator Move(bool withHit = true)
	{
		StartMove?.Invoke();
		while (m_Tracker.HasNextTarget())
		{
			var target = m_Tracker.GetNextTarger();
			if (withHit) HitOtherPawn(target.point);

			float sqrDistance;
			do {
				sqrDistance = (target.point - transform.position).sqrMagnitude;
				transform.position = Vector3.MoveTowards(transform.position, target.point, GameData.instance.speed * Time.deltaTime);
				yield return null;
			} while (sqrDistance > float.Epsilon);
		}

		m_Tracker.trace.ResetTrace(m_Tracker, true);
		StopMove?.Invoke();
	}

	/// <summary>
	/// Сместить другую пешку
	/// </summary>
	/// <param name="target"> Предположительное местонахождение пешки </param>
	private void HitOtherPawn(Vector3 target)
	{
		Vector3 direction = target - transform.position;
		var hits = Physics.RaycastAll(transform.position, direction.normalized, direction.magnitude, 1 << gameObject.layer);
		if (hits.Length > 0)
		{
			hits[0].transform.GetComponent<Pawn>().m_Tracker.Shift();
		}
	}

	/// <summary>
	///  Другая пешка смещает нашу пешку
	/// </summary>
	/// <param name="canHit"></param>
	public void Shift(bool canHit)
	{
		StartCoroutine(Move(canHit));
	}
}