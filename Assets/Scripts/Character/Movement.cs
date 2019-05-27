using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character {

	[RequireComponent(typeof(Rigidbody2D))]
	public class Movement : MonoBehaviour {

		[Tooltip("If !isAuto, movement is based on keyboard input. " +
			"*Changes to value only apply on game restart.")]
		[SerializeField] private bool m_isAuto;
		[Range(1f, 100f)]
		[SerializeField] private float m_movementSpeed = 5f;

		private Rigidbody2D m_rigidBody2D;

		private void Awake() {
			m_rigidBody2D = GetComponent<Rigidbody2D>();
		}

		private void Start() {
			//TODO apply value from isAuto for autopilot characters
			SetInputObservers();
		}

		private void SetInputObservers() {
			this.FixedUpdateAsObservable()
				.Select(_ => Input.GetAxis("Horizontal"))
				.Subscribe(x => {
					Vector2 movement = Vector2.zero;
					movement.x = x;

					m_rigidBody2D.position = (
						m_rigidBody2D.position + (
							movement * m_movementSpeed * Time.fixedDeltaTime));
				})
				.AddTo(this);

			this.FixedUpdateAsObservable()
				.Select(_ => Input.GetAxis("Vertical"))
				.Subscribe(y => {
					Vector2 movement = Vector2.zero;
					movement.y = y;

					m_rigidBody2D.position = (
						m_rigidBody2D.position + (
							movement * m_movementSpeed * Time.fixedDeltaTime));
				})
				.AddTo(this);
		}
			
	}

}