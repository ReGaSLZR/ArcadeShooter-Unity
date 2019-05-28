using System.Collections;
using UnityEngine;

namespace Character.Health {

	public class EnemyHealth : HealthBehaviour {

		public override void ApplyDamage() {
			m_health--;

			if (m_health <= 0) {
				StartCoroutine(CorKill());	
			}

		}

		private IEnumerator CorKill() {
            //wait for one frame to allow listeners of health value to react
            yield return null;
			Destroy(this.gameObject);
		}

	}

}
