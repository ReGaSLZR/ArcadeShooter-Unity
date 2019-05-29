using UniRx;
using UnityEngine;

namespace Injection.Model {

    public class PlayerStatsModel : MonoBehaviour, PlayerStatsModel.IGetter, PlayerStatsModel.ISetter
    {

        public interface IGetter {
            ReactiveProperty<int> GetHealth();
            bool IsHealthOnMax();
        }

        public interface ISetter {
            bool DeductHealthByOne();
            bool IncreaseHealthByOne();
        }

        private const int MAX_HEALTH = 3;

        [Tooltip("Changes to Starting Health value are only applied on game restart.")]
        [Range(1, MAX_HEALTH)]
        [SerializeField] private int m_startingHealth;

        private ReactiveProperty<int> m_reactiveHealth;

        private void Awake() {
            m_reactiveHealth = new ReactiveProperty<int>(m_startingHealth);
        }

        #region IGetter functions

        public ReactiveProperty<int> GetHealth() {
            return m_reactiveHealth;
        }

        public bool IsHealthOnMax() {
            return (m_reactiveHealth.Value == MAX_HEALTH);
        }

        #endregion

        #region ISetter functions

        public bool DeductHealthByOne() {
            if (m_reactiveHealth.Value > 0) {
                m_reactiveHealth.Value--;
                return true;
            }
            else {
                LogUtil.PrintWarning(this, GetType(), "Cannot DeductHealthByOne() if health is <=0");
                return false;
            }
        }

        public bool IncreaseHealthByOne() { 
            if(m_reactiveHealth.Value < MAX_HEALTH) {
                m_reactiveHealth.Value++;
                return true;
            }
            else {
                LogUtil.PrintWarning(this, GetType(), "Cannot IncreaseHealthByOne() if health is >= MAX");
                return false;
            }
        }

        #endregion

    }

}