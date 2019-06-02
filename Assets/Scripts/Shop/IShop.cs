namespace Shop {

    public interface IShop
    {
        bool Shop(ShopItem item);

        //bool AddHealth(int health, int coinCost);

        //bool AddSkillLimitedAmmo(int ammo, int coinCost);
        //bool AddSkillLimitedCap(int cap, int coinCost);

        //bool AddSkillRechargeableAmmo(int ammo, int coinCost);
        //bool AddSkillRechargeableCap(int cap, int coinCost);
        //bool AddSkillRechargeableRegen(float regen, int coinCost);

        bool IsMaxValueForStat(ShopItem.ForStat stat);
        //bool DeductCoinsBy(int coins);
    }

}