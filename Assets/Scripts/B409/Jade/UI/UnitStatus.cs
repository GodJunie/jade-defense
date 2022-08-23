using UnityEngine;
using TMPro;

namespace B409.Jade.UI {
    using Battle;

    public class UnitStatus : MonoBehaviour {
        [SerializeField]
        private TMP_Text textHp;
        [SerializeField]
        private TMP_Text textAtk;
        [SerializeField]
        private TMP_Text textRange;
        [SerializeField]
        private TMP_Text textMoveSpeed;
        [SerializeField]
        private TMP_Text textAtkSpeed;
        [SerializeField]
        private TMP_Text textCooltime;

        public void SetUI(Status status) {
            this.textHp.text = status.Hp.ToString("0");
            this.textAtk.text = status.Atk.ToString("0");
            this.textRange.text = status.Range.ToString("0.#");
            this.textMoveSpeed.text = (status.MoveSpeed * 100).ToString("0");
            this.textAtkSpeed.text = (1 / status.AttackSpeed).ToString("0.000");
            this.textCooltime.text = status.Cooltime.ToString("0.#");
        }
    }
}