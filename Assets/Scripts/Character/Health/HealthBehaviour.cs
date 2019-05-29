﻿using Injection.Model;
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

        protected ReactiveProperty<bool> m_reactiveIsDead;

        protected abstract void ApplyDamageTick();

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
                ActivateDamageFX();
            }
            else {
                StartCoroutine(CorKill());
            }
        }

        protected void ActivateDamageFX() {
            LogUtil.PrintInfo(this, GetType(), "ActivateDamageFX()");
            m_instantiator.InstantiateInjectPrefab(m_fxModel.GetRandomFXDamage(), this.gameObject);
        }

        protected void ActivateDeathFX() {
            LogUtil.PrintInfo(this, GetType(), "ActivateDeathFX()");
            m_instantiator.InstantiateInjectPrefab(m_fxModel.GetRandomFXDeath(m_fxDeath), this.gameObject);
        }

        protected IEnumerator CorKill() {
            LogUtil.PrintInfo(this, GetType(), "CorKill()");
            ActivateDeathFX();
            m_reactiveIsDead.Value = true;

            //wait for one frame to allow listeners of health value to react
            yield return null;
            Destroy(this.gameObject);
        }

        public ReactiveProperty<bool> GetReactiveIsDead() {
            return m_reactiveIsDead;
        }

	}

}
