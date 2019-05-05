using UnityEngine;
using System.Collections;
namespace Assets.Scripts
{
	public class Pawn : MonoBehaviour
	{
		private bool canMove = false;
		private int numberMovePoint = 0;
		private Vector3[] movePoints;

		void Start()
		{

		}

		void Update()
		{

		}

		private void FixedUpdate()
		{
			Move();
		}

		public void Move(Vector3[] movePoints)
		{
			this.movePoints = movePoints;
			numberMovePoint = 0;
			canMove = true;
		}

		private void Move()
		{
			if (!canMove)
				return;

			if (MoveToPoint(movePoints[numberMovePoint]))
			{
				canMove = movePoints.Length > ++numberMovePoint;
			}
		}

		private bool MoveToPoint(Vector3 targetPoint)
		{
			bool targetReached = false;
			Vector3 moveDirection = targetPoint - transform.position;
			Vector3 moveVector = moveDirection.normalized * GlobalGameData.GameSpeed * Time.deltaTime;
			if (moveDirection.magnitude < moveVector.magnitude)
			{
				moveVector = moveDirection;
				targetReached = true;
			}
			transform.Translate(moveVector);
			return targetReached;
		}
	}
}