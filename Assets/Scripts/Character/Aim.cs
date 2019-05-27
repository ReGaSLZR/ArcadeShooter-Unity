using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character {
	
	public class Aim : MonoBehaviour {

		[Tooltip("If !isAuto, aiming is set to follow mouse position.")]
		[SerializeField] private bool m_isAuto;

		private Transform m_targetObject; //TODO to code target detection later.
		//TODO remember to give this one an initial value to avoid null errors.

		void Start() {
			this.UpdateAsObservable()
				.Select(_ => getDirection())
				.Subscribe(direction => {
					transform.up = direction;
				})
				.AddTo(this);
		}

		private Vector2 getDirection() {
			return (m_isAuto) ?
				detectNearbyTarget() :
				getMouseDirection();
		}

		//TODO code more logic for this
		private Vector2 detectNearbyTarget() {
			return new Vector2 (m_targetObject.transform.position.x,
				m_targetObject.transform.position.y);
		}

		private Vector2 getMouseDirection() {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
				Input.mousePosition);
			return new Vector2(
				mousePosition.x - transform.position.x,
				mousePosition.y - transform.position.y
			);
		}

		public void StopAiming() {
			LogUtil.PrintInfo(this, this.GetType(), "StopAiming() called.");
			Destroy(this);
		}

	}

}