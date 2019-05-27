﻿using System.Collections;
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
			ActivateFX();

			yield return new WaitForSeconds(m_fxDuration);
			Destroy(this.gameObject);
		}

	}

}