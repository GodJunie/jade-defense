using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Battle {
    [System.Serializable]
    public struct Status {
        [BoxGroup("General")]
        [SerializeField]
        private float cooltime;
        [BoxGroup("General")]
        [SerializeField]
        private float hp;
        [BoxGroup("General")]
        [SerializeField]
        private float moveSpeed;
        [BoxGroup("General")]
        [SerializeField]
        private float range;
       
       
        [BoxGroup("Target")]
        [SerializeField]
        private TargetFilterMode targetFilterMode;
        [BoxGroup("Target")]
        [SerializeField]
        private bool descending;
        [BoxGroup("Target")]
        [SerializeField]
        private int targetCount;
        [BoxGroup("Target")]
        [SerializeField]
        private bool targetEnemy;

        [BoxGroup("Attack")]
        [SerializeField]
        private float attackSpeed;
        [BoxGroup("Attack")]
        [SerializeField]
        private AttackMode attackMode;
        [BoxGroup("Attack")]
        [ShowIf("ShowAtk")]
        [SerializeField]
        private float atk;
        [BoxGroup("Attack")]
        [ShowIf("ShowHeal")]
        [SerializeField]
        private float heal;
        [BoxGroup("Attack")]
        [ShowIf("ShowInterval")]
        [SerializeField]
        private float interval;
        [BoxGroup("Attack")]
        [ShowIf("ShowDuration")]
        [SerializeField]
        private float duration;
        [BoxGroup("Attack")]
        [ShowIf("ShowDistance")]
        [SerializeField]
        private float distance;
        [BoxGroup("Attack")]
        [ShowIf("ShowSlowRate")]
        [Range(0f, 1f)]
        [SerializeField]
        private float slowRate;

        // General
        public float Cooltime => cooltime;
        public float Hp => hp;
        public float MoveSpeed => moveSpeed;
        public float Range => range;

        // Target
        public TargetFilterMode TargetFilterMode => targetFilterMode;
        public bool Descending => descending;
        public int TargetCount => targetCount;
        public bool TargetEnemy => targetEnemy;


        public AttackMode AttackMode => attackMode;
        public float AttackSpeed => attackSpeed;
        public float Atk => atk;
        public float Heal => heal;
        public float Interval => interval;
        public float Duration => duration;
        public float Distance => distance;
        public float SlowRate => slowRate;

#if UNITY_EDITOR
        private bool ShowAtk {
            get {
                return 
                    this.attackMode == AttackMode.Attack || 
                    this.attackMode == AttackMode.DamageOverTime ||
                    this.attackMode == AttackMode.Stun || 
                    this.attackMode == AttackMode.KnockBack || 
                    this.attackMode == AttackMode.Slow;
            }
        }

        private bool ShowHeal {
            get {
                return this.attackMode == AttackMode.Heal;
            }
        }

        private bool ShowInterval {
            get {
                return this.attackMode == AttackMode.DamageOverTime;
            }
        }

        private bool ShowDuration {
            get {
                return 
                    this.attackMode == AttackMode.DamageOverTime || 
                    this.attackMode == AttackMode.Stun || 
                    this.attackMode == AttackMode.KnockBack || 
                    this.attackMode == AttackMode.Slow;
            }
        }

        private bool ShowDistance {
            get {
                return this.attackMode == AttackMode.KnockBack;
            }
        }

        private bool ShowSlowRate {
            get {
                return this.attackMode == AttackMode.Slow;
            }
        }
#endif
    }

    public static class StatusExtensions {
        public static string GetAttackDescriptionString(this Status status) {
            string d = "";

            if(status.AttackMode == AttackMode.Heal) {
                d = string.Format("Heal <sprite name=Hp> <color={0}>{1} HP</color> ", GameConsts.HpColor.GetHexString(), status.Heal);
            } else {
                d = string.Format("Inflicts <sprite name=Atk> <color={0}>{1} dmg</color> ", GameConsts.DamageColor.GetHexString(), status.Atk);
            }

            if(status.TargetCount == 0) {
                d += string.Format("to the <sprite index=7> <color={1}>all {0}</color> within range ", status.TargetEnemy ? "opponents" : "allies", GameConsts.TargetColor.GetHexString());
            } else {
                string t = string.Format("<sprite index=7> <color={2}>{0} {1}</color>", status.TargetCount, status.TargetEnemy ? "opponents" : "allies", GameConsts.TargetColor.GetHexString());
                switch(status.TargetFilterMode) {
                case TargetFilterMode.Hp:
                    d += string.Format("to {0} with {1} current HP first ", t, status.Descending ? "high" : "low");
                    break;
                case TargetFilterMode.MaxHp:
                    d += string.Format("to {0} with {1} maximum HP first ", t, status.Descending ? "high" : "low");
                    break;
                case TargetFilterMode.Distance:
                    d += string.Format("to the {1} {0} ", t, status.Descending ? "farthest" : "nearest");
                    break;
                case TargetFilterMode.Index:
                    d += string.Format("to {0} ", t);
                    break;
                default:
                    break;
                }
            }

            switch(status.AttackMode) {
            case AttackMode.Attack:
                break;
            case AttackMode.DamageOverTime:
                d += string.Format("over {0:0.#} secs", status.Duration);
                break;
            case AttackMode.Heal:
                break;
            case AttackMode.Stun:
                d += string.Format("and stuns for {0:0.#} secs", status.Duration);
                break;
            case AttackMode.Slow:
                d += string.Format("and slow down <sprite name=Slow> <color={2}>{0:P1}</color> speed for <color={3}>{1:0.#} secs</color>", status.SlowRate, status.Duration, GameConsts.SlowColor.GetHexString(), GameConsts.DurationColor.GetHexString());
                break;
            default:
                break;
            }

            d += ".";
            return d;
        }
    }
}