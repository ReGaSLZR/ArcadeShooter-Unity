using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Misc {

    [RequireComponent(typeof(Button))]
    public class ShopItem : MonoBehaviour
    {

        [Range(1, 50)]
        public int m_coinCost;

        [Space]

        [SerializeField] private TextMeshProUGUI m_textCoinCost;
        [SerializeField] private TextMeshProUGUI m_textFeedback;

        [TextArea]
        [SerializeField] private string m_feedbackNonCoinIssue;

        public bool m_isTaken;
        public Button m_button { get; private set; }

        private const string m_spielNotEnoughCoins = "Not enough coins.";

        private void Awake() {
            m_button = GetComponent<Button>();

            m_textCoinCost.text = m_coinCost.ToString();
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
            else if (coins >= m_coinCost) {
                Enable();
            }
            else {
                Disable(true);
            }
        }

        public void Disable(bool isCoinIssue) {
            m_button.interactable = false;
            m_textFeedback.text = isCoinIssue ? 
                m_spielNotEnoughCoins : m_feedbackNonCoinIssue;
            m_textFeedback.gameObject.SetActive(true);

        }

        public void Enable() {
            m_button.interactable = true;
            m_textFeedback.gameObject.SetActive(false);
        }

    }

}


