using Injection.Model;
using UnityEngine;
using Zenject;

namespace Character.Skill {

	public abstract class SkillBehaviour : MonoBehaviour {

        [Inject] protected readonly FXModel.IGetter m_fxModel;
        [Inject] protected readonly SpawnParentModel.IParent m_spawnParent;
        [Inject] protected readonly Injection.Instantiator m_instantiator;

        protected abstract void ExecuteUseSkill();

        public void UseSkill() {
             ExecuteUseSkill();
        }

        public void StopSkill() {
            StopAllCoroutines();
            Destroy(this);
        }

	}

}