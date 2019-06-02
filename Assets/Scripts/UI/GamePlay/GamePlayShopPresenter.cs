using Injection.Model;
using Shop;
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
        [SerializeField] private ShopItemView[] m_shopItemViews;

        [Space]
        [SerializeField] private ShopItem[] m_autoRewardedItems;

        [Inject] private readonly IShop m_shop;
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

            foreach(ShopItem item in m_autoRewardedItems) {
                m_shop.Shop(item);
            }

            RefreshItems();

            ForceCheckCoinCostForItems();
            ForceCheckNonCoinIssueForItems();
        }

        private void TakeItemThenForceCheckAllStock(ShopItemView item) {
            item.m_isTaken = true;
            ForceCheckCoinCostForItems();
            ForceCheckNonCoinIssueForItems();
            UpdateStatsProgress();

            item.m_button.interactable = false;
        }

        private void ForceCheckCoinCostForItems() {
            int currentCoins = m_stat.GetCoins().Value;

            foreach(ShopItemView view in m_shopItemViews) {
                view.DisplayAvailabilityByCoins(currentCoins);
            }
        }

        private void ForceCheckNonCoinIssueForItems() {
            foreach(ShopItemView view in m_shopItemViews) {
                if(m_shop.IsMaxValueForStat(view.m_shopItem.m_stat)) {
                    view.Disable(false);
                }
            }
        }

        private void UpdateStatsProgress() {
            m_textHealthProgress.text = m_stat.GetHealthProgress();
            m_textRocketsProgress.text = m_stat.GetLimitedSkillProgress();
            m_textShieldDurationProgress.text = m_stat.GetRechargeableSkillProgress();
            m_textShieldRegenProgress.text = m_stat.GetRechargeableSkillRegenProgress();
        }

        private void RefreshItems() {
            foreach(ShopItemView view in m_shopItemViews) {
                view.Refresh();
            }
        }

        private void SetObservables() {
            foreach(ShopItemView view in m_shopItemViews) {
                view.m_button.OnClickAsObservable()
                    .Subscribe(_ => {
                        if (m_shop.Shop(view.m_shopItem)) {
                            TakeItemThenForceCheckAllStock(view);
                        }
                        else {
                            LogUtil.PrintWarning(this, GetType(), "Could not shop for item.");
                        }
                    })
                    .AddTo(this);
            }
        }

    }


}