using Injection.Model;
using Zenject;

namespace Character.Health {

    public class PlayerHealth : HealthBehaviour
    {

        [Inject] PlayerStatsModel.IGetter m_playerStatsGetter;
        [Inject] PlayerStatsModel.ISetter m_playerStatsSetter;

        private void Start() {
            RefreshHealth();
        }

        private void RefreshHealth() {
            m_health = m_playerStatsGetter.GetHealth().Value;
        }

        protected override void ApplyDamageTick() {
            LogUtil.PrintInfo(this, GetType(), "ApplyDamageTick()");
            if (m_playerStatsSetter.DeductHealthByOne()) {
                RefreshHealth();
            }
        }

    }

}