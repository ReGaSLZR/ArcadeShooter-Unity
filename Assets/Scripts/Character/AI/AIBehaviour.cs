using Character.Aim;
using Character.Health;
using Character.Movement;
using Character.Skill;
using UnityEngine;
using UniRx;

namespace Character.AI {

	public abstract class AIBehaviour : MonoBehaviour {

		[SerializeField] protected AimBehaviour m_aim;
		[SerializeField] protected MovementBehaviour m_movement;
		[Space]
		[SerializeField] protected HealthBehaviour m_health;
		[Space]
		[SerializeField] protected TargetDetector m_targetDetector;
		[SerializeField] protected SkillBehaviour m_skillDefault;

		protected abstract void SafelyStopExtraComponents();

		private void Start() {
			m_health.GetReactiveIsDead()
				.Where(isDead => isDead)
				.Subscribe(_ => {
                    LogUtil.PrintInfo(this, GetType(), "Character is dead.");
                    SafelyStopExtraComponents();
                    SafelyStopComponents();
                    Destroy(this.gameObject);
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


            if (m_skillDefault != null) {
                m_skillDefault.StopSkill();
            }

            if (m_targetDetector != null) {
				Destroy(m_targetDetector);
			}
		}

	}

}

