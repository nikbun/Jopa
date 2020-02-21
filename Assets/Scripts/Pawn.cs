using UnityEngine;
using MapSpace;
using System.Collections;

public class Pawn : MonoBehaviour
{
	[Tooltip("Обводка фишки")]
	[SerializeField] SpriteRenderer _outlineSprite; // Подсветка. В случае, если фишкой можно ходить

	// Трекер для работы с картой
	Tracker _tracker;

	public delegate void PawnMovement();
	public event PawnMovement StartMovement;
	public event PawnMovement StopMovement;

	public Map.Sides MapSide { get; set; }

	public bool IsMoving { get; private set; }
	
	public bool IsInHome { get => _tracker.IsHome(); }

	public bool IsEndInitialization { get; private set; }


	public Pawn ()
	{
		StartMovement = () => IsMoving = true;
		StopMovement = () => IsMoving = false;
	}

	void Start()
	{
		_outlineSprite.enabled = false;
		_tracker = new Tracker(MapSide, GameData.Instance.MapController);
		_tracker.ShiftMove += Move;
		IsEndInitialization = true;
	}

	void OnMouseDown()
	{
		if (_tracker.ReadyStartMoving)
		{
			Move();
		}
	}

	public bool CanStartMove(int distance) 
	{
		return _outlineSprite.enabled = _tracker.CanStartMove(distance);
	}

	public void CancelStartMove()
	{
		_tracker.ReadyStartMoving = false;
		_outlineSprite.enabled = false;
	}

	/// <summary>
	///  начать движение
	/// </summary>
	/// <param name="canHit"></param>
	public void Move()
	{
		StartCoroutine(MoveCoroutine());
	}

	IEnumerator MoveCoroutine()
	{
		StartMovement?.Invoke();
		while (_tracker.HasNextTarget())
		{
			var target = _tracker.GetNextTarget();

			float sqrDistance;
			do {
				sqrDistance = (target.Position - transform.position).sqrMagnitude;
				transform.position = Vector3.MoveTowards(transform.position, target.Position, GameData.Instance.PawnSpeed * Time.deltaTime);
				yield return null;
			} while (sqrDistance > float.Epsilon);
		}
		StopMovement?.Invoke();
	}

	public void DestroyPawn() 
	{
		_tracker.DisposeTracker();
		_tracker = null;
		Destroy(gameObject);
	}
}