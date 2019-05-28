using Injection.Model;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Character.Health {

	[RequireComponent(typeof(Collider2D))]
	public abstract class HealthBehaviour : MonoBehaviour {

		[Tooltip("Changes to health value will only apply on game restart.")]
		[Range(1, 99)]
		[SerializeField] protected int m_health = 1;
        [SerializeField] private FXModel.FXDeath m_fxDeath;

        [Inject] readonly FXModel.IGetter m_fxModel;
        [Inject] readonly Injection.Instantiator m_instantiator;

		public ReactiveProperty<bool> m_reactiveIsDead { get; private set; }

		public abstract void ApplyDamage();

		private void Awake() {
			m_reactiveIsDead = new ReactiveProperty<bool>(false);
		}

		private void Start() {
			this.UpdateAsObservable ()
				.Select(_ => m_health)
				.Where(health => (health == 0))
				.Subscribe(m_health => {
                    ActivateFX();
                    m_reactiveIsDead.Value = true;
                })
				.AddTo(this);
		}

		private void ActivateFX() {
            m_health = -1; //to prevent another call to this method        
            m_instantiator.InstantiateInjectPrefab(m_fxModel.GetRandomFXDeath(m_fxDeath), this.gameObject);
		}

	}

}
