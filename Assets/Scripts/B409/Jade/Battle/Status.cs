using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Battle {
    [System.Serializable]
    public struct Status {
        [SerializeField]
        private float hp;
        [SerializeField]
        private float atk;
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float attackSpeed;

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

        public float Hp => hp;
        public float Atk => atk;
        public float MoveSpeed => moveSpeed;
        public float AttackSpeed => attackSpeed;
        public float Range => range;

        public TargetFilterMode TargetFilterMode => targetFilterMode;
        public bool Descending => descending;
        public int TargetCount => targetCount;
        public bool TargetEnemy => targetEnemy;
    }
}