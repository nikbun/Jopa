using UnityEngine;
using MapSpace;
using System.Collections;

public class Pawn : MonoBehaviour
{
	public SpriteRenderer outline; // Подсветка, в случае, если фишкой можно ходить
	public Map.Sides mapSide;

	public delegate void PawnMovement();
	public event PawnMovement StartMovement;
	public event PawnMovement StopMovement;

	Tracker _tracker;
	bool _moving;

	public Pawn ()
	{
		StartMovement = () => _moving = true;
		StopMovement = () => _moving = false;
	}

	void Start()
	{
		outline.enabled = false;
		_tracker = new Tracker(mapSide, GameData.Instance.mapController);
		_tracker.ShiftMove += StartMove;
	}

	void OnMouseDown()
	{
		if (_tracker.readyStartMoving)
		{
			StartMove();
		}
	}

	public bool CanStartMove(int distance) 
	{
		return outline.enabled = _tracker.CanStartMove(distance);
	}

	public void CancelStartMove()
	{
		_tracker.readyStartMoving = false;
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
		while (_tracker.HasNextTarget())
		{
			var target = _tracker.GetNextTarget();

			float sqrDistance;
			do {
				sqrDistance = (target.position - transform.position).sqrMagnitude;
				transform.position = Vector3.MoveTowards(transform.position, target.position, GameData.Instance.pawnSpeed * Time.deltaTime);
				yield return null;
			} while (sqrDistance > float.Epsilon);
		}
		StopMovement?.Invoke();
	}

	public bool IsMoving()
	{
		return _moving;
	}

	public void DestroyPawn() 
	{
		_tracker.DisposeTracker();
		_tracker = null;
		Destroy(gameObject);
	}
}