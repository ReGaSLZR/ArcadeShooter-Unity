using Injection.Model;
using System.Collections;
using UnityEngine;
using UniRx;
using Zenject;

namespace Character.Health {

	[RequireComponent(typeof(Collider2D))]
	public abstract class HealthBehaviour : MonoBehaviour {

		[Tooltip("Changes to health value will only apply on game restart.")]
		[Range(1, 99)]
		[SerializeField] protected int m_health = 1;
        [SerializeField] private FXModel.FXDeath m_fxDeath;

        [Inject] protected readonly FXModel.IGetter m_fxModel;
        [Inject] protected readonly Injection.Instantiator m_instantiator;
        [Inject] protected readonly SpawnParentModel.IParent m_spawnParent;

        public ReactiveProperty<bool> m_reactiveIsDead { get; private set; }

        protected abstract void ApplyDamageTick();
        protected abstract void OnDeath();

        public void ApplyDamage() {
            if(m_health <= 0) {
                LogUtil.PrintWarning(this, this.GetType(), "Cannot be killed more than once.");
                return;
            }

            ApplyDamageTick();
            ManageDamage();
        }

		private void Awake() {
			m_reactiveIsDead = new ReactiveProperty<bool>(false);
		}

        private void ManageDamage() {
            if(m_health > 0) {
                ActivateDamageFX(false);
            }
            else {
                StartCoroutine(CorKill());
            }
        }

        private void ActivateDamageFX(bool isFinalHit) {
            m_spawnParent.ParentThisChild(m_instantiator.InstantiateInjectPrefab(
                isFinalHit ? m_fxModel.GetRandomFXDeath(m_fxDeath) 
                    : m_fxModel.GetRandomFXDamage(), 
                this.gameObject));
        }

        private IEnumerator CorKill() {
            LogUtil.PrintInfo(this, GetType(), "CorKill()");
            ActivateDamageFX(true);
            OnDeath();
            m_reactiveIsDead.Value = true;

            //wait for one frame to allow listeners of health value to react
            yield return new WaitForSeconds(0.5f);
            Destroy(this.gameObject);
        }

	}

}
