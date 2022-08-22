using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using TMPro;

namespace B409.Jade.UI {
    using Data;
    using Game;

    public class ParameterPanel : MonoBehaviour {
        [SerializeField]
        private RadarChart radarChart;
        [SerializeField]
        private float duration = 1f;
        [SerializeField]
        private TMP_Text textDescription;
        [SerializeField]
        [TextArea(0, 4)]
        private string descriptionFormat;

        public void Open() {
            this.gameObject.SetActive(true);

            radarChart.SetMaxValue(GameConsts.ParameterMaxValue);

            var progress = GameManager.Instance.Progress;

            float strength = progress.Parameters[Parameter.Strength];
            float deft = progress.Parameters[Parameter.Deft];
            float endurance = progress.Parameters[Parameter.Endurance];
            float intelligence = progress.Parameters[Parameter.Intelligence];
            float luck = progress.Parameters[Parameter.Luck];

            this.textDescription.text = string.Format(descriptionFormat, strength, GameConsts.GetApDiscountRate(strength) * 100, deft, GameConsts.GetCraftingBonusRate(deft) * 100, endurance, GameConsts.GetMaxAp(endurance), intelligence, GameConsts.GetTradeDiscountRate(intelligence) * 100, luck, GameConsts.GetFarmingCount(luck));

            DrawChart(duration);
        }

        private async void DrawChart(float duration) {
            var progress = GameManager.Instance.Progress;

            var values = progress.Parameters.OrderBy(pair => (int)pair.Key).Select(pair => pair.Value).ToArray();

            for(float i = 0; i < duration; i += Time.fixedDeltaTime) {
                radarChart.SetValues(values.Select(e => e * (i / duration)).ToArray());
                await UniTask.WaitForFixedUpdate();
            }

            radarChart.SetValues(values);
        }
    }
}