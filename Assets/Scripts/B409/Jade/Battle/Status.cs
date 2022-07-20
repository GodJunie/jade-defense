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
                    this.attackMode == AttackMode.DotDamage ||
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
                return this.attackMode == AttackMode.DotDamage;
            }
        }

        private bool ShowDuration {
            get {
                return 
                    this.attackMode == AttackMode.DotDamage || 
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
}