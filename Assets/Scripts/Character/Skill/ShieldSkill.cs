using UnityEngine;

namespace Character.Skill {

    public class ShieldSkill : SkillBehaviour
    {

        [SerializeField] private GameTags m_tagOnActive;
        [SerializeField] private GameTags m_tagOnExhaust;

        [SerializeField] private Transform m_shieldedMember;
        [SerializeField] private Transform m_shieldFXMember;

        private void Awake() {
            if((m_shieldFXMember == null) || (m_shieldedMember == null)) {
                LogUtil.PrintError(this, GetType(), "Cannot have NULL for both members.");
                Destroy(this);
            }

        }

        private void Start() {
            m_isActive = false;
            m_shieldFXMember.gameObject.SetActive(false);
        }

        private bool ShouldUse() {
            return m_shieldedMember.gameObject.tag.Equals(m_tagOnExhaust.ToString());
        }

        private void ToggleSkill() {
            bool shouldUse = ShouldUse();

            m_shieldedMember.gameObject.tag = shouldUse ? m_tagOnActive.ToString() :
                m_tagOnExhaust.ToString();
            m_shieldFXMember.gameObject.SetActive(shouldUse);
            m_isActive = shouldUse;
        }

        protected override void ExecuteUseSkill() {
            ToggleSkill();
        }

        public override void StopSkill() {
            LogUtil.PrintInfo(this, GetType(), "Stopping shield skill...");

            //force-turn it to active state first to ensure that
            //the ToggleSkill will trigger a stop call
            m_shieldedMember.tag = m_tagOnActive.ToString(); 
            ToggleSkill();
        }

    }

}