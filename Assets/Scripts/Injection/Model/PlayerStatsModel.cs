using UniRx;
using UnityEngine;

namespace Injection.Model {

    public class PlayerStatsModel : MonoBehaviour, 
        PlayerStatsModel.IStatSetter,
        PlayerStatsModel.IStatGetter,
        PlayerStatsModel.ICoinSetter,
        PlayerStatsModel.IScoreSetter,
        PlayerStatsModel.IShop
    {

        public interface IShop {
            bool IncreaseHealthByOne();   
            bool IncreaseShieldByTen();
            bool IncreaseRocketsMaxByOne();
            void RefillRockets();

            bool DeductCoinsBy(int coins);
        }

        public interface IStatGetter {
            ReactiveProperty<int> GetHealth();
            bool IsHealthMaxCap();

            ReactiveProperty<int> GetShield();
            bool IsShieldMaxCap();
            ReactiveProperty<int> GetShieldRegen();

            ReactiveProperty<int> GetRockets();
            bool IsRocketMaxCap();

            ReactiveProperty<int> GetScore();
            ReactiveProperty<int> GetCoins();
        }

        public interface IStatSetter {
            bool DeductHealthByOne();
            bool UseShield();
            bool UseRocket();
        }

        public interface ICoinSetter {
            void IncreaseCoinsBy(int coins);
        }

        public interface IScoreSetter {
            void IncreaseScoreBy(int score);
        }


        private const int MAX_HEALTH = 3;
        private const int MAX_SHIELD = 100;
        private int m_currentMaxShield = 40;
        private const int MAX_SHIELD_REGEN_TICK = 10;

        private const int MAX_SCORE = 9999;
        private const int MAX_COINS = 9999;

        private const int MAX_ROCKETS = 10;
        private int m_currentMaxSpecialRockets = 3;

        private ReactiveProperty<int> m_reactiveHealth;
        private ReactiveProperty<int> m_reactiveRockets;
        private ReactiveProperty<int> m_reactiveShield;
        private ReactiveProperty<int> m_reactiveShieldRegen;

        private ReactiveProperty<int> m_reactiveScore;
        private ReactiveProperty<int> m_reactiveCoins;

        private void Awake() {
            m_reactiveHealth = new ReactiveProperty<int>(MAX_HEALTH);
            m_reactiveRockets = new ReactiveProperty<int>(m_currentMaxSpecialRockets);
            m_reactiveShield = new ReactiveProperty<int>(m_currentMaxShield);
            m_reactiveShieldRegen = new ReactiveProperty<int>(0);

            m_reactiveScore = new ReactiveProperty<int>(0);
            m_reactiveCoins = new ReactiveProperty<int>(0);
        }

        #region IStatSetter functions

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

        public bool UseShield() {
            if(m_reactiveShieldRegen.Value == MAX_SHIELD_REGEN_TICK) {
                LogUtil.PrintWarning(this, GetType(), "UseShield() TODO");
                //TODO
                return true;
            }
            return false;
        }

        public bool UseRocket() {
            if(m_reactiveRockets.Value > 0) {
                m_reactiveRockets.Value--;
                return true;
            }

            return false;
        }

        #endregion

        #region IStatGetter functions

        public ReactiveProperty<int> GetHealth() {
            return m_reactiveHealth;
        }

        public bool IsHealthMaxCap() {
            return (m_reactiveHealth.Value == MAX_HEALTH);
        }

        public ReactiveProperty<int> GetShield() {
            return m_reactiveShield;
        }

        public bool IsShieldMaxCap() {
            return (m_currentMaxShield == MAX_SHIELD);
        }

        public ReactiveProperty<int> GetShieldRegen() {
            return m_reactiveShieldRegen;
        }

        public ReactiveProperty<int> GetRockets() {
            return m_reactiveRockets;
        }

        public bool IsRocketMaxCap() {
            return (m_currentMaxSpecialRockets == MAX_ROCKETS);
        }

        public ReactiveProperty<int> GetScore() {
            return m_reactiveScore;
        }

        public ReactiveProperty<int> GetCoins() {
            return m_reactiveCoins;
        }

        #endregion

        #region ICoinsSetter

        public void IncreaseCoinsBy(int coins) {
            m_reactiveCoins.Value = Mathf.Clamp((m_reactiveCoins.Value + coins), 0, MAX_COINS);
        }

        #endregion

        #region IScoreSetter

        public void IncreaseScoreBy(int score) {
            m_reactiveScore.Value = Mathf.Clamp((m_reactiveScore.Value + score), 0, MAX_SCORE);
        }

        #endregion

        #region IShop functions

        public bool IncreaseHealthByOne() {
            if(m_reactiveHealth.Value < MAX_HEALTH) {
                m_reactiveHealth.Value++;
                return true;
            }

            return false;
        }

        public bool IncreaseShieldByTen() {
            if(m_currentMaxShield < MAX_SHIELD) {
                m_currentMaxShield += 10;
                m_reactiveShield.Value = m_currentMaxShield;

                return true;
            }

            return false;
        }

        public bool IncreaseRocketsMaxByOne() {
            if(!IsRocketMaxCap()) {
                m_currentMaxSpecialRockets++;
                return true;
            }

            return false;
        }

        public void RefillRockets() {
            m_reactiveRockets.Value = m_currentMaxSpecialRockets;
        }

        public bool DeductCoinsBy(int coins) {
            if(coins <= m_reactiveCoins.Value) {
                m_reactiveCoins.Value -= coins;
                return true;
            }

            return false;
        }

        #endregion

    }

}