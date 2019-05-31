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
            bool IncreaseHealthByOne(int coinCost);   

            bool IncreaseSkillLimitedMaxByOne(int coinCost);
            bool RefillSkillLimited(int coinCost);

            bool IncreaseSkillRechargeableByOne(int coinCost);
            bool DecreaseSkillRechargeableRegenTimeByHalfSecond(int coinCost);
            bool RefillSkillRechargeable(int coinCost);

            bool DeductCoinsBy(int coins);
        }

        public interface IStatGetter {
            ReactiveProperty<int> GetHealth();
            string GetHealthProgress();
            bool IsHealthMaxCap();
            bool IsPlayerDead();

            ReactiveProperty<int> GetRechargeableSkill();
            string GetRechargeableSkillProgress();
            string GetRechargeableSkillRegenProgress();
            int GetRechargeableSkillCurrentCap();
            bool IsRechargeableMaxCap();
            bool IsRechargeableRegenMaxCap();

            ReactiveProperty<int> GetLimitedSkill();
            string GetLimitedSkillProgress();
            int GetLimitedSkillCurrentCap();
            bool IsLimitedMaxCap();
            bool IsLimitedOnFull();

            ReactiveProperty<int> GetScore();
            ReactiveProperty<int> GetCoins();
        }

        public interface IStatSetter {
            bool DeductHealthByOne();
            bool InvokeRechargeableSkill(bool shouldUse);
            bool UseLimitedSkill();
        }

        public interface ICoinSetter {
            void IncreaseCoinsBy(int coins);
        }

        public interface IScoreSetter {
            void IncreaseScoreBy(int score);
        }
        #endregion

        [Header("Max Values")]
        [SerializeField] private int m_maxHealth = 3;
        [SerializeField] private int m_maxScore = 9999;
        [SerializeField] private int m_maxCoins = 9999;
        [SerializeField] private int m_maxCapSkillLimited = 10;
        [SerializeField] private int m_maxCapSkillRechargeable = 10;

        [Header("Skill Capacities")]
        [SerializeField] private int m_capSkillLimited = 3;
        [SerializeField] private int m_capSkillRechargeable = 4;

        [Header("Rechargeable Skill: Drain, Regen")]
        [SerializeField] private int m_skillRechargeableDrainTick = 1;
        [SerializeField] private float m_skillRechargeableRegenTick = 0.5f;
        [SerializeField] private float m_skillRechargeableRegenTime = 10f;
        [SerializeField] private float m_skillRechargeableRegenTimeDecrement = 0f;

        private bool m_isRechargeableSkillInUse;
        private DateTimeOffset m_rechargeRegenStartTimestamp;

        private ReactiveProperty<int> m_reactiveHealth;
        private ReactiveProperty<int> m_reactiveSkillLimited;
        private ReactiveProperty<int> m_reactiveSkillRechargeable;

        private ReactiveProperty<int> m_reactiveScore;
        private ReactiveProperty<int> m_reactiveCoins;

        private void Awake() {
            m_reactiveHealth = new ReactiveProperty<int>(m_maxHealth);
            m_reactiveSkillLimited = new ReactiveProperty<int>(m_capSkillLimited);
            m_reactiveSkillRechargeable = new ReactiveProperty<int>(m_capSkillRechargeable);

            m_reactiveScore = new ReactiveProperty<int>(0);
            m_reactiveCoins = new ReactiveProperty<int>(0);
        }

        private void Start() {
            SetRechargeableRegenObservable();
            SetRechargeableUseObservable();

            m_reactiveSkillRechargeable
                .Where(charge => (charge == 0))
                .Subscribe(_ => {
                    m_isRechargeableSkillInUse = false;
                    //so as to prevent auto-full recharge when Player is not hit
                    //during the duration of the invoked rechargeable skill
                    ResetRechargeRegen();
                })
                .AddTo(this);
        }

        private void ResetRechargeRegen() {
            m_rechargeRegenStartTimestamp = DateTimeOffset.Now;
        }

        private void SetRechargeableUseObservable() {
            Observable.Interval(TimeSpan.FromSeconds(m_skillRechargeableDrainTick))
                .Where(_ => m_isRechargeableSkillInUse && (m_reactiveSkillRechargeable.Value > 0))
                .Subscribe(_ => {
                    m_reactiveSkillRechargeable.Value -= m_skillRechargeableDrainTick;
                })
                .AddTo(this);
        }

        private void SetRechargeableRegenObservable() {
            Observable.Interval(TimeSpan.FromSeconds(m_skillRechargeableRegenTick))
                .Timestamp()
                .Where(timeStamp => !m_isRechargeableSkillInUse &&
                    (m_reactiveSkillRechargeable.Value < m_capSkillRechargeable) &&
                    (timeStamp.Timestamp >= m_rechargeRegenStartTimestamp.AddSeconds(
                        m_skillRechargeableRegenTime - m_skillRechargeableRegenTimeDecrement))
                )
                .Subscribe(_ => {
                    m_reactiveSkillRechargeable.Value = m_capSkillRechargeable;
                    m_isRechargeableSkillInUse = false;
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
            if(m_reactiveSkillRechargeable.Value > 0) {
                m_isRechargeableSkillInUse = shouldUse;
                ResetRechargeRegen();
                return true;
            }
            return false;
        }

        public bool UseLimitedSkill() {
            if(m_reactiveSkillLimited.Value > 0) {
                m_reactiveSkillLimited.Value--;
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
            return (m_reactiveHealth.Value == m_maxHealth);
        }

        public bool IsPlayerDead() {
            return (m_reactiveHealth.Value == 0);
        }

        public bool IsRechargeableMaxCap() {
            return (m_capSkillRechargeable == m_maxCapSkillRechargeable);
        }

        public bool IsRechargeableRegenMaxCap() {
            return (m_skillRechargeableRegenTimeDecrement == m_skillRechargeableRegenTime);
        }

        public ReactiveProperty<int> GetRechargeableSkill() {
            return m_reactiveSkillRechargeable;
        }

        public ReactiveProperty<int> GetLimitedSkill() {
            return m_reactiveSkillLimited;
        }

        public bool IsLimitedMaxCap() {
            return (m_capSkillLimited == m_maxCapSkillLimited);
        }

        public bool IsLimitedOnFull() {
            return (m_reactiveSkillLimited.Value == m_capSkillLimited);
        }

        public ReactiveProperty<int> GetScore() {
            return m_reactiveScore;
        }

        public ReactiveProperty<int> GetCoins() {
            return m_reactiveCoins;
        }

        public string GetHealthProgress() {
            return m_reactiveHealth.Value.ToString() + "/" + m_maxHealth.ToString();
        }

        public string GetRechargeableSkillProgress() {
            return m_capSkillRechargeable.ToString();
        }

        public string GetRechargeableSkillRegenProgress() {
            return (m_skillRechargeableRegenTime - m_skillRechargeableRegenTimeDecrement).ToString();
        }

        public int GetRechargeableSkillCurrentCap() {
            return m_capSkillRechargeable;
        }

        public string GetLimitedSkillProgress() {
            return m_reactiveSkillLimited.Value.ToString() + "/" + m_capSkillLimited;
        }

        public int GetLimitedSkillCurrentCap() {
            return m_capSkillLimited;
        }

        #endregion

        #region ICoinsSetter

        public void IncreaseCoinsBy(int coins) {
            m_reactiveCoins.Value = Mathf.Clamp((m_reactiveCoins.Value + coins), 0, m_maxCoins);
        }

        #endregion

        #region IScoreSetter

        public void IncreaseScoreBy(int score) {
            m_reactiveScore.Value = Mathf.Clamp((m_reactiveScore.Value + score), 0, m_maxScore);
        }

        #endregion

        #region IShop functions

        public bool IncreaseHealthByOne(int coinCost) {
            if(m_reactiveHealth.Value < m_maxHealth) {
                m_reactiveHealth.Value++;
                DeductCoinsBy(coinCost);
                return true;
            }

            return false;
        }

        public bool IncreaseSkillRechargeableByOne(int coinCost) {
            if((m_capSkillRechargeable < m_maxCapSkillRechargeable) 
                && DeductCoinsBy(coinCost)) {
                    m_capSkillRechargeable++;
                    return true;
            }

            return false;
        }

        public bool DecreaseSkillRechargeableRegenTimeByHalfSecond(int coinCost) {
            if((m_skillRechargeableRegenTimeDecrement < m_skillRechargeableRegenTime) 
                && DeductCoinsBy(coinCost)) {
                 m_skillRechargeableRegenTimeDecrement += 0.5f;
                  return true;
            }
            return false;
        }

        public bool IncreaseSkillLimitedMaxByOne(int coinCost) {
            if(!IsLimitedMaxCap() && DeductCoinsBy(coinCost)) {
                m_capSkillLimited++;
                return true;
            }

            return false;
        }

        public bool RefillSkillLimited(int coinCost) {
            if(DeductCoinsBy(coinCost)) {
                m_reactiveSkillLimited.Value = m_capSkillLimited;
                return true;
            }

            return false;
        }

        public bool RefillSkillRechargeable(int coinCost) {
            if(DeductCoinsBy(coinCost)) {
                m_reactiveSkillRechargeable.Value = m_capSkillRechargeable;
                return true;
            }

            return false;
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