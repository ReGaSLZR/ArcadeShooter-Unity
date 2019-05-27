using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.Health {

	public abstract class HealthBehaviour : MonoBehaviour {

		[Tooltip("Changes to health value will only apply on game restart.")]
		[Range(1, 99)]
		[SerializeField] protected int m_health = 1;
		[SerializeField] protected GameObject m_childMain;

		[Space]
		[SerializeField] protected GameObject m_childFXKilled;
		[Range(0f, 10f)]
		[SerializeField] protected float m_fxDuration = 0f;

		public ReactiveProperty<bool> m_reactiveIsDead { get; private set; }

		public abstract void ApplyDamage();

		private void Awake() {
			m_reactiveIsDead = new ReactiveProperty<bool>(false);

			if(m_childMain == null) {
				LogUtil.PrintError(this, this.GetType(), 
					"Child Main cannot be NULL.");
				return;
			}
		}

		private void Start() {
			this.UpdateAsObservable ()
				.Select(_ => m_health)
				.Where(health => (health == 0))
				.Subscribe(m_health => m_reactiveIsDead.Value = true)
				.AddTo(this);
		}

		protected void ActivateFX() {
			if (m_childFXKilled != null) {
				m_childMain.SetActive (false);
				m_childFXKilled.SetActive (true);
			} else {
				LogUtil.PrintInfo(this, this.GetType(), 
					"ActivateFX() Sorry, no Kill FX set.");
			}
		}

	}

}
