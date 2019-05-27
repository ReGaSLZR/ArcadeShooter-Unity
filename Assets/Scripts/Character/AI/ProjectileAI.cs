using Character;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.AI {

	public class ProjectileAI : AIBehaviour {

		private void Awake() {
			if(m_targetDetector == null) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot have a NULL target detector.");
				Destroy(this);
			}

			if(m_skillDefault == null) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot have a NULL default skill.");
				Destroy(this);
			}
		}

		private void Start() {
			m_targetDetector.m_isTargetDetected
				.Where(isDetected => isDetected)
				.Subscribe(m_targetDetector => {
					m_skillDefault.UseSkill();
				})
				.AddTo(this);
		}

		protected override void SafelyStopExtraComponents() {
			LogUtil.PrintInfo(this, this.GetType(), "No extra components to stop.");
		}

			
	}


}