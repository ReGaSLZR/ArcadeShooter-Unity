using System;
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
        #region Interfaces
        public interface IShop {
            bool IncreaseHealthByOne();   
            bool IncreaseSkillInvocableRechargeableByTen();
            bool IncreaseSkillLimitedSpecialMaxByOne();
            void RefillSkillLimitedSpecial();
            void RefillInvocableRechargeable();

            bool DeductCoinsBy(int coins);
        }

        public interface IStatGetter {
            ReactiveProperty<int> GetHealth();
            bool IsHealthMaxCap();
            bool IsPlayerDead();

            ReactiveProperty<int> GetInvocableRechargeableSkill();
            bool IsInvocableRechargeableMaxCap();

            ReactiveProperty<int> GetSpecialLimitedSkill();
            bool IsSpecialLimitedMaxCap();

            ReactiveProperty<int> GetScore();
            ReactiveProperty<int> GetCoins();
        }

        public interface IStatSetter {
            bool DeductHealthByOne();
            bool InvokeRechargeableSkill(bool shouldUse);
            bool UseSpecialLimitedSkill();
        }

        public interface ICoinSetter {
            void IncreaseCoinsBy(int coins);
        }

        public interface IScoreSetter {
            void IncreaseScoreBy(int score);
        }
        #endregion

        private const int MAX_HEALTH = 3;
        private const int MAX_UPGRADE_SKILL_RECHARGEABLE = 100;
        private const int SKILL_RECHARGEABLE_DRAIN_TICK = 10;
        private const int SKILL_RECHARGEABLE_REGEN_TIME = 10;
        private const int MAX_SCORE = 9999;
        private const int MAX_COINS = 9999;
        private const int MAX_UPGRADE_SKILL_LIMITED_SPECIAL = 10;

        private int m_currentMaxRechargeable = 40;
        private bool m_isInvocableRechargeableInUse;
        private DateTimeOffset m_rechargeRegenRestTimestamp;

        private int m_currentMaxSpecialLimited = 3;

        private ReactiveProperty<int> m_reactiveHealth;
        private ReactiveProperty<int> m_reactiveSpecialLimitedSkill;
        private ReactiveProperty<int> m_reactiveInvocableRechargeableSkill;

        private ReactiveProperty<int> m_reactiveScore;
        private ReactiveProperty<int> m_reactiveCoins;

        private void Awake() {
            m_reactiveHealth = new ReactiveProperty<int>(MAX_HEALTH);
            m_reactiveSpecialLimitedSkill = new ReactiveProperty<int>(m_currentMaxSpecialLimited);
            m_reactiveInvocableRechargeableSkill = new ReactiveProperty<int>(m_currentMaxRechargeable);

            m_reactiveScore = new ReactiveProperty<int>(0);
            m_reactiveCoins = new ReactiveProperty<int>(0);
        }

        private void Start() {
            SetRechargeableRegenObservable();
            SetRechargeableUseObservable();

            m_reactiveInvocableRechargeableSkill
                .Where(charge => (charge == 0))
                .Subscribe(_ => {
                    m_isInvocableRechargeableInUse = false;
                    //so as to prevent auto-full recharge when Player is not hit
                    //during the duration of the invoked rechargeable skill
                    ResetRechargeRegen();
                })
                .AddTo(this);
        }

        private void ResetRechargeRegen() {
            m_rechargeRegenRestTimestamp = DateTimeOffset.Now;
        }

        private void SetRechargeableUseObservable() {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Where(_ => m_isInvocableRechargeableInUse && (m_reactiveInvocableRechargeableSkill.Value > 0))
                .Subscribe(_ => {
                    m_reactiveInvocableRechargeableSkill.Value -= SKILL_RECHARGEABLE_DRAIN_TICK;
                })
                .AddTo(this);
        }

        private void SetRechargeableRegenObservable() {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Timestamp()
                .Where(timeStamp => !m_isInvocableRechargeableInUse &&
                    (m_reactiveInvocableRechargeableSkill.Value < m_currentMaxRechargeable) &&
                    (timeStamp.Timestamp >= m_rechargeRegenRestTimestamp.AddSeconds(SKILL_RECHARGEABLE_REGEN_TIME))
                )
                .Subscribe(_ => {
                    m_reactiveInvocableRechargeableSkill.Value = m_currentMaxRechargeable;
                    m_isInvocableRechargeableInUse = false;
                    ResetRechargeRegen();
                })
                .AddTo(this);
        }

        #region IStatSetter functions

        public bool DeductHealthByOne() {
            if (m_reactiveHealth.Value > 0) {
                m_reactiveHealth.Value--;
                ResetRechargeRegen();
                return true;
            }
            else {
                LogUtil.PrintWarning(this, GetType(), "Cannot DeductHealthByOne() if health is <=0");
                return false;
            }
        }

        /* 
         * Returns if the shield was toggled. If FALSE, it is in regen state. 
         */
        public bool InvokeRechargeableSkill(bool shouldUse) {
            if(m_reactiveInvocableRechargeableSkill.Value > 0) {
                m_isInvocableRechargeableInUse = shouldUse;
                ResetRechargeRegen();
                return true;
            }
            return false;
        }

        public bool UseSpecialLimitedSkill() {
            if(m_reactiveSpecialLimitedSkill.Value > 0) {
                m_reactiveSpecialLimitedSkill.Value--;
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

        public bool IsPlayerDead() {
            return (m_reactiveHealth.Value == 0);
        }

        public bool IsInvocableRechargeableMaxCap() {
            return (m_currentMaxRechargeable == MAX_UPGRADE_SKILL_RECHARGEABLE);
        }

        public ReactiveProperty<int> GetInvocableRechargeableSkill() {
            return m_reactiveInvocableRechargeableSkill;
        }

        public ReactiveProperty<int> GetSpecialLimitedSkill() {
            return m_reactiveSpecialLimitedSkill;
        }

        public bool IsSpecialLimitedMaxCap() {
            return (m_currentMaxSpecialLimited == MAX_UPGRADE_SKILL_LIMITED_SPECIAL);
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

        public bool IncreaseSkillInvocableRechargeableByTen() {
            if(m_currentMaxRechargeable < MAX_UPGRADE_SKILL_RECHARGEABLE) {
                m_currentMaxRechargeable += 10;
                m_reactiveInvocableRechargeableSkill.Value = m_currentMaxRechargeable;

                return true;
            }

            return false;
        }

        public bool IncreaseSkillLimitedSpecialMaxByOne() {
            if(!IsSpecialLimitedMaxCap()) {
                m_currentMaxSpecialLimited++;
                return true;
            }

            return false;
        }

        public void RefillSkillLimitedSpecial() {
            m_reactiveSpecialLimitedSkill.Value = m_currentMaxSpecialLimited;
        }

        public void RefillInvocableRechargeable() {
            m_reactiveInvocableRechargeableSkill.Value = m_currentMaxRechargeable;
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