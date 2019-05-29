using Injection.Model;
using Zenject;

namespace Character.Health {

    public class PlayerHealth : HealthBehaviour
    {

        [Inject] PlayerStatsModel.IStatGetter m_statsGetter;
        [Inject] PlayerStatsModel.IStatSetter m_statsSetter;

        private void Start() {
            RefreshHealth();
        }

        private void RefreshHealth() {
            m_health = m_statsGetter.GetHealth().Value;
        }

        protected override void ApplyDamageTick() {
            LogUtil.PrintInfo(this, GetType(), "ApplyDamageTick()");
            if (m_statsSetter.DeductHealthByOne()) {
                RefreshHealth();
            }
        }

        protected override void OnDeath() {
            LogUtil.PrintInfo(this, GetType(), "Player is dead.");
        }

    }

}