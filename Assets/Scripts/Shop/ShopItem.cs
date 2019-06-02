using UnityEngine;

namespace Shop {

    [CreateAssetMenu(fileName = "New ShopItem", menuName = "Shop/Item")]
    public class ShopItem : ScriptableObject
    {

        public enum ForStat {
            Health,

            SkillLimitedAmmo,
            SkillLimitedCap,

            SkillRechargeableAmmo,
            SkillRechargeableCap,
            SkillRechargeableRegen
        }

        public string m_title;

        [TextArea]
        public string m_details;

        [Tooltip("Error message to display when an issue (non-coin related) is met.")]
        public string m_spielCustomMessage;

        [Space]
        public ForStat m_stat;
        [Tooltip("A value of STAT_VALUE_REFILL_TO_CAP == refill to current capacity.")]
        public float m_statValue;
        [Range(1, 100)]
        public int m_coinCost;

        public const string SPIEL_NOT_ENOUGH_COINS = "Not enough coins.";
        public const float STAT_VALUE_REFILL_TO_CAP = -999f;

    }

}


