using System.Collections;
using UniRx;
using UnityEngine;

namespace Character.AI {

	public class EnemyAI : AIBehaviour {

        [Range(0.5f, 5.5f)]
        [SerializeField] private float m_firingInterval = 1f;

		private void Awake() {
			if((m_movement == null) || (m_skillDefault == null) || (m_targetDetector == null)) {
				LogUtil.PrintError(this, this.GetType(), "Cannot have a NULL value for " +
                    "any of these: movement, default skill, targetDetector.");
				Destroy(this);
			}
		}

        private void Start() {
            m_targetDetector.m_isTargetDetected
                .Subscribe(hasTarget => {
                    if(hasTarget) {
                        StartCoroutine(CorUseSkill());
                    }
                    else {
                        StopAllCoroutines();
                    }
                })
                .AddTo(this);
        }

        private IEnumerator CorUseSkill() {
            while (m_targetDetector.m_isTargetDetected.Value)
            {
                m_skillDefault.UseSkill();
                yield return new WaitForSeconds(m_firingInterval);
            }
        }

        protected override void SafelyStopExtraComponents() {
            StopAllCoroutines();
		}
	}

}