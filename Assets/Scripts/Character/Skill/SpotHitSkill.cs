﻿using Character.Health;
using Injection.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Skill {

	public class SpotHitSkill : SkillBehaviour {

		[SerializeField] private TargetDetector m_targetDetector;
        [Space]
		[SerializeField] private bool m_isDestroyedOnHit;
        [SerializeField] private FXModel.FXDeath m_fxDeath;

		private void Awake() {
			if(m_targetDetector == null) {
				LogUtil.PrintError(this, this.GetType(), "Cannot have a NULL target detector.");
				Destroy(this);
			}

            m_isActive = true;
		}

        protected override void ExecuteUseSkill() {
			foreach(HealthBehaviour killable in GetKillables()) {
				killable.ApplyDamage();
			}

			if(m_isDestroyedOnHit) {
                m_spawnParent.ParentThisChild(m_instantiator.InstantiateInjectPrefab(
                    m_fxModel.GetRandomFXDeath(m_fxDeath), this.gameObject));
                
				Destroy(this.gameObject);
			}
		}

		private List<HealthBehaviour> GetKillables() {
			List<HealthBehaviour> killables = new List<HealthBehaviour>();

			for(int x=0; x<m_targetDetector.m_targets.Count; x++) {
				if(m_targetDetector.m_targets[x] != null) {
					HealthBehaviour killable = m_targetDetector.m_targets[x].gameObject.GetComponent<HealthBehaviour>();

                    if (killable != null) {
                        killables.Add(killable);
                    }
                    //else {
                    //    LogUtil.PrintWarning(this, this.GetType(), "No HealthBehaviour attached to detected target.");
                    //}
				}
			}

			return killables;
		}
    }

}