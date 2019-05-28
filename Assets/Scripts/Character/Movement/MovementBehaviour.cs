using UnityEngine;

namespace Character.Movement {

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
	public abstract class MovementBehaviour : MonoBehaviour {

		[Range(1f, 100f)]
		[SerializeField] protected float m_movementSpeed = 5f;

		protected Rigidbody2D m_rigidBody2D;

        protected abstract void SafelyStopMovementComponents();

		private void Awake() {
			m_rigidBody2D = GetComponent<Rigidbody2D>();
		}

		public void StopMoving() {
			LogUtil.PrintInfo(this, this.GetType(), "StopMoving() called.");
            SafelyStopMovementComponents();
            Destroy(this);
		}
			
	}

}