using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop {

    [RequireComponent(typeof(Button))]
    public class ShopItemView : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI m_textTitle;
        [SerializeField] private TextMeshProUGUI m_textDetails;
        [SerializeField] private TextMeshProUGUI m_textCoinCost;
        [SerializeField] private TextMeshProUGUI m_textFeedback;

        public ShopItem m_shopItem;

        public bool m_isTaken;
        public Button m_button { get; private set; }

        private void Awake() {
            if(m_shopItem == null) {
                LogUtil.PrintError(this, GetType(), "Cannot have NULL shop item value.");
                Destroy(this);
            }

            m_button = GetComponent<Button>();

            //not on Start() where it may be called many times
            //this needs to be configured only once
            ConfigureView(); 
        }

        private void ConfigureView() {
            m_textTitle.text = m_shopItem.m_title;
            m_textDetails.text = m_shopItem.m_details;
            m_textCoinCost.text = m_shopItem.m_coinCost.ToString();

            Refresh();
        }

        public void Refresh() {
            m_textFeedback.gameObject.SetActive(false);
            m_button.interactable = true;
            m_isTaken = false;
        }

        public void DisplayAvailabilityByCoins(int coins) {
            if(m_isTaken) {
                return;
            }
            else if (coins >= m_shopItem.m_coinCost) {
                Enable();
            }
            else {
                Disable(true);
            }
        }

        public void Disable(bool isCoinIssue) {
            m_button.interactable = false;
            m_textFeedback.text = isCoinIssue ? 
                ShopItem.SPIEL_NOT_ENOUGH_COINS : m_shopItem.m_spielCustomMessage;
            m_textFeedback.gameObject.SetActive(true);

        }

        public void Enable() {
            m_button.interactable = true;
            m_textFeedback.gameObject.SetActive(false);
        }

    }

}


