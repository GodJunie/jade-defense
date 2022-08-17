using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConsts {

    public static Color HpColor = new Color32(5, 215, 0, 255);
    public static Color DamageColor = new Color32(255, 40, 40, 255);
    public static Color SlowColor = new Color32(63, 227, 255, 255);
    public static Color DurationColor = new Color32(255, 217, 73, 255);
    public static Color TargetColor = new Color32(255, 163, 32, 255);


    public static Color ButtonGreen = new Color32(82, 255, 0, 255);
    public static Color ButtonRed = new Color32(255, 59, 59, 255);


    public const int GOLD_ID = 400000;


    #region Parameter
    public const float ParameterMaxValue = 120f;

    public const float MaxApMin = 100f;
    public const float MaxApMax = 200f;

    public const float ApDiscountRateMin = 0f;
    public const float ApDiscountRateMax = 0.3f;

    public const float TradeDiscountRateMin = 0f;
    public const float TradeDiscountRateMax = 0.3f;

    public const int FarmingCountMin = 3;
    public const int FarmingCountMax = 8;

    public const float CraftingBonusRateMin = 0f;
    public const float CraftingBonusRateMax = 0.3f;

    public static float GetMaxAp(float value) {
        return Mathf.Lerp(MaxApMin, MaxApMax, value / ParameterMaxValue);
    }
    
    public static float GetApDiscountRate(float value) {
        return Mathf.Lerp(ApDiscountRateMin, ApDiscountRateMax, value / ParameterMaxValue);
    }

    public static float GetTradeDiscountRate(float value) {
        return Mathf.Lerp(TradeDiscountRateMin, TradeDiscountRateMax, value / ParameterMaxValue);
    }

    public static int GetFarmingCount(float value) {
        return Mathf.FloorToInt(Mathf.Lerp(FarmingCountMin, FarmingCountMax, value / ParameterMaxValue));
    }

    public static float GetCraftingBonusRate(float value) {
        return Mathf.Lerp(CraftingBonusRateMin, CraftingBonusRateMax, value / ParameterMaxValue);
    }
    #endregion
}
