using Misc;
using UnityEngine;

namespace Character.Health {

	public class EnemyHealth : HealthBehaviour {
       
        [SerializeField] private SpawnableObject m_onDeathSpawn;

		protected override void ApplyDamageTick() {
			m_health--;
        }

        protected override void OnDeath() {
            if(m_onDeathSpawn != null) {
                m_onDeathSpawn.TrySpawn(m_instantiator, gameObject);
            }
        }

    }

}
