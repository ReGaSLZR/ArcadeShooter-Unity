using UnityEngine;

namespace Character.Skill {

	public abstract class SkillBehaviour : MonoBehaviour {

        private bool m_isSkillUsable = true;

        protected abstract void ExecuteUseSkill();

        public void UseSkill() {
            if(m_isSkillUsable) {
                ExecuteUseSkill();
            }
        }

        public void StopSkill() {
            m_isSkillUsable = false;
            Destroy(this);
        }

	}

}