using Character;
using Character.Health;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Skill {

	public class SpotHitSkill : SkillBehaviour {

		[SerializeField] private TargetDetector m_targetDetector;
		[SerializeField] private bool m_isDestroyedOnHit;

		private void Awake() {
			if(m_targetDetector == null) {
				LogUtil.PrintError(this, this.GetType(), "Cannot have a NULL target detector.");
				Destroy(this);
			}
		}

        protected override void ExecuteUseSkill() {
			foreach(HealthBehaviour killable in GetKillables()) {
				killable.ApplyDamage();
			}

			if(m_isDestroyedOnHit) {
				Destroy(this.gameObject);
			}
		}

		private List<HealthBehaviour> GetKillables() {
			List<HealthBehaviour> killables = new List<HealthBehaviour>();

			for(int x=0; x<m_targetDetector.m_targets.Count; x++) {
				if(m_targetDetector.m_targets[x] != null) {
					HealthBehaviour killable = m_targetDetector.m_targets[x].gameObject.GetComponent<HealthBehaviour>();

					if(killable != null) {
						killables.Add(killable);
					}
				}
			}

			return killables;
		}
    }

}