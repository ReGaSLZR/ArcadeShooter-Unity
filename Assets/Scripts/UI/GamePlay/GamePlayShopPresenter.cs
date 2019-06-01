using Injection.Model;
using Misc;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.GamePlay {

    public class GamePlayShopPresenter : MonoBehaviour
    {

        [Header("UI for Stats in Shop")]
        [SerializeField] private TextMeshProUGUI m_textCoinsLeft;
        [SerializeField] private TextMeshProUGUI m_textHealthProgress;
        [SerializeField] private TextMeshProUGUI m_textRocketsProgress;
        [SerializeField] private TextMeshProUGUI m_textShieldDurationProgress;
        [SerializeField] private TextMeshProUGUI m_textShieldRegenProgress;

        [Header("Shop Item buttons")]
        [SerializeField] private ShopItem m_healthPlus1;
        [Space]
        [SerializeField] private ShopItem m_skillLimitedRefill;
        [SerializeField] private ShopItem m_skillLimitedCapPlus1;
        [Space]
        [SerializeField] private ShopItem m_skillRechargeableRegenTimeDecreaseByHalfSecond;
        [SerializeField] private ShopItem m_skillRechargeableCapPlus1;

        [Inject] private readonly PlayerStatsModel.IShop m_shop;
        [Inject] private readonly PlayerStatsModel.IStatGetter m_stat;
        [Inject] private readonly RoundModel.IGetter m_round;

        private void Start() {
            SetObservables();

            m_round.GetTimer()
                .Where(timer => (timer == 0))
                .Subscribe(_ => ConfigureButtons())
                .AddTo(this);

            m_stat.GetCoins()
                .Subscribe(coins => m_textCoinsLeft.text = coins.ToString())
                .AddTo(this);
        }

        private void ConfigureButtons() {
            UpdateStatsProgress();

            //auto-refill when a round is over as a reward for the player
            m_shop.RefillSkillRechargeable(0); 

            RefreshItems();

            ForceCheckCoinCostForItems();
            ForceCheckNonCoinIssueForItems();
        }

        private void TakeItemThenForceCheckAllStock(ShopItem item) {
            item.m_isTaken = true;
            ForceCheckCoinCostForItems();
            ForceCheckNonCoinIssueForItems();
            UpdateStatsProgress();

            item.m_button.interactable = false;
        }

        private void ForceCheckCoinCostForItems() {
            int currentCoins = m_stat.GetCoins().Value;

            m_healthPlus1.DisplayAvailabilityByCoins(currentCoins);
            m_skillLimitedCapPlus1.DisplayAvailabilityByCoins(currentCoins);
            m_skillLimitedRefill.DisplayAvailabilityByCoins(currentCoins);
            m_skillRechargeableCapPlus1.DisplayAvailabilityByCoins(currentCoins);
            m_skillRechargeableRegenTimeDecreaseByHalfSecond.DisplayAvailabilityByCoins(currentCoins);
        }

        private void ForceCheckNonCoinIssueForItems() {
            if(m_stat.IsHealthMaxCap()) {
                m_healthPlus1.Disable(false);
            }

            if(m_stat.IsLimitedSkillMaxCap()) {
                m_skillLimitedCapPlus1.Disable(false);
            }

            if(m_stat.IsLimitedSkillOnFull()) {
                m_skillLimitedRefill.Disable(false);
            }

            if(m_stat.IsRechargeableSkillMaxCap()) {
                m_skillRechargeableCapPlus1.Disable(false);
            }

            if(m_stat.IsRechargeableSkillRegenMaxCap()) {
                m_skillRechargeableRegenTimeDecreaseByHalfSecond.Disable(false);
            }

        }

        private void UpdateStatsProgress() {
            m_textHealthProgress.text = m_stat.GetHealthProgress();
            m_textRocketsProgress.text = m_stat.GetLimitedSkillProgress();
            m_textShieldDurationProgress.text = m_stat.GetRechargeableSkillProgress();
            m_textShieldRegenProgress.text = m_stat.GetRechargeableSkillRegenProgress();
        }

        private void RefreshItems() {
            m_healthPlus1.Refresh();
            m_skillLimitedCapPlus1.Refresh();
            m_skillLimitedRefill.Refresh();
            m_skillRechargeableCapPlus1.Refresh();
            m_skillRechargeableRegenTimeDecreaseByHalfSecond.Refresh();
        }

        private void SetObservables() {
            m_healthPlus1.m_button.OnClickAsObservable()
               .Subscribe(_ => {
                   if(m_shop.IncreaseHealthByOne(m_healthPlus1.m_coinCost)) {
                       TakeItemThenForceCheckAllStock(m_healthPlus1);
                   }
                   else {
                       LogUtil.PrintInfo(this, GetType(), "Could not +1 health. " +
                           "Not taking the coin cost.");
                   }
               })
               .AddTo(this);

             m_skillLimitedCapPlus1.m_button.OnClickAsObservable()
               .Subscribe(_ => {
                   if (m_shop.IncreaseSkillLimitedMaxByOne(m_skillLimitedCapPlus1.m_coinCost)) {
                       TakeItemThenForceCheckAllStock(m_skillLimitedCapPlus1);
                   }
                   else {
                       LogUtil.PrintInfo(this, GetType(), "Could not +1 limited skill cap. " +
                           "Not taking the coin cost.");
                   }
               })
               .AddTo(this);

            m_skillLimitedRefill.m_button.OnClickAsObservable()
               .Subscribe(_ => {
                   if (m_shop.RefillSkillLimited(m_skillLimitedRefill.m_coinCost)) {
                       TakeItemThenForceCheckAllStock(m_skillLimitedRefill);
                   }
                   else {
                       LogUtil.PrintInfo(this, GetType(), "Could not refill skill limited. " +
                           "Not taking the coin cost.");
                   }
               })
               .AddTo(this);

           m_skillRechargeableCapPlus1.m_button.OnClickAsObservable()
               .Subscribe(_ => {
                   if (m_shop.IncreaseSkillRechargeableByOne(m_skillRechargeableCapPlus1.m_coinCost)) {
                       TakeItemThenForceCheckAllStock(m_skillRechargeableCapPlus1);
                   }
                   else {
                       LogUtil.PrintInfo(this, GetType(), "Could not +1 rechargeable skill cap. " +
                           "Not taking the coin cost.");
                   }
               })
               .AddTo(this);

             m_skillRechargeableRegenTimeDecreaseByHalfSecond.m_button.OnClickAsObservable()
               .Subscribe(_ => {
                   if (m_shop.DecreaseSkillRechargeableRegenTimeByHalfSecond(
                       m_skillRechargeableRegenTimeDecreaseByHalfSecond.m_coinCost)) {
                       TakeItemThenForceCheckAllStock(m_skillRechargeableRegenTimeDecreaseByHalfSecond);
                   }
                   else {
                       LogUtil.PrintInfo(this, GetType(), "Could not -0.5s rechargeable skill regen time. " +
                           "Not taking the coin cost.");
                   }
               })
               .AddTo(this);
        }

    }


}