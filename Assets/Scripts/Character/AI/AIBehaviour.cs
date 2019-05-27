using Character;
using Character.Health;
using Character.Skill;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.AI {

	public abstract class AIBehaviour : MonoBehaviour {

		[SerializeField] protected Aim m_aim;
		[SerializeField] protected Movement m_movement;
		[Space]
		[SerializeField] protected HealthBehaviour m_health;
		[Space]
		[SerializeField] protected TargetDetector m_targetDetector;
		[SerializeField] protected SkillBehaviour m_skillDefault;

		protected abstract void SafelyStopExtraComponents();

		private void Start() {
			m_health.m_reactiveIsDead
				.Where(isDead => isDead)
				.Subscribe(_ => {
					SafelyStopComponents(); 
					SafelyStopExtraComponents();
				})
				.AddTo(this);
		}

		private void SafelyStopComponents() {
			if(m_movement != null) {
				m_movement.StopMoving();
			}

			if(m_aim != null) {
				m_aim.StopAiming();
			}

			if(m_targetDetector != null) {
				Destroy(m_targetDetector);
			}
		}

	}

}

