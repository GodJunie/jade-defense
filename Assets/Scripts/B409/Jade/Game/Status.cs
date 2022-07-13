using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Game {
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

        public float Hp => hp;
        public float Atk => atk;
        public float MoveSpeed => moveSpeed;
        public float AttackSpeed => attackSpeed;
        public float Range => range;
    }
}