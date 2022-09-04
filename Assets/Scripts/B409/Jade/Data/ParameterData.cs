using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    using Game;
    using Battle;

    [CreateAssetMenu(fileName = "ParameterData", menuName = "B409/Parameter Data")]
    public class ParameterData : ScriptableObject {
        [BoxGroup("Parameter Max Value")]
        [HideLabel]
        [SerializeField]
        private float parameterMaxValue;

        [BoxGroup("Max Ap")]
        [HideLabel]
        [SerializeField]
        private ParameterBasedValue maxAp;

        [BoxGroup("Ap Discount Rate")]
        [HideLabel]
        [SerializeField]
        private ParameterBasedValue apDiscountRate;

        [BoxGroup("Trade Discount Rate")]
        [HideLabel]
        [SerializeField]
        private ParameterBasedValue tradeDiscountRate;
     
        [BoxGroup("Farming Count")]
        [HideLabel]
        [SerializeField]
        private ParameterBasedValue farmingCount;
     
        [BoxGroup("Crafting Bonus Rate")]
        [HideLabel]
        [SerializeField]
        private ParameterBasedValue craftingBonusRate;

        public float ParameterMaxValue => parameterMaxValue;

        public float GetMaxAp(GameProgress progress) {
            return maxAp.GetValue(progress, parameterMaxValue);
        }

        public float GetApDiscountRate(GameProgress progress) {
            return apDiscountRate.GetValue(progress, parameterMaxValue);
        }

        public float GetTradeDiscountRate(GameProgress progress) {
            return tradeDiscountRate.GetValue(progress, parameterMaxValue);
        }

        public int GetFarmingCount(GameProgress progress) {
            return Mathf.FloorToInt(farmingCount.GetValue(progress, parameterMaxValue));
        }

        public float GetCraftingBonusRate(GameProgress progress) {
            return craftingBonusRate.GetValue(progress, parameterMaxValue);
        }

        [System.Serializable]
        public class ParameterBasedValue {
            [LabelText("Based Parameter")]
            [SerializeField]
            private Parameter parameter;
            [HorizontalGroup("group")]
            [BoxGroup("group/Min")]
            [HideLabel]
            [SerializeField]
            private float min;
            [HorizontalGroup("group")]
            [BoxGroup("group/Max")]
            [HideLabel]
            [SerializeField]
            private float max;

            public float GetValue(GameProgress progress, float parameterMaxValue) {
                float value = progress.Parameters[parameter];
                return Mathf.Lerp(min, max, value / parameterMaxValue);
            }
        }
    }
}