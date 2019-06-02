using UnityEngine;

namespace Shop {

    [CreateAssetMenu(fileName = "New ShopItem", menuName = "Shop/Item")]
    public class ShopItem : ScriptableObject
    {

        public string m_title;

        [TextArea]
        public string m_details;

        [Tooltip("Error message to display when an issue (non-coin related) is met.")]
        public string m_spielCustomMessage;

        [Range(1, 100)]
        public int m_coinCost;

        public const string SPIEL_NOT_ENOUGH_COINS = "Not enough coins.";

    }

}


