using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Battle {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Detector : MonoBehaviour {
        private new CircleCollider2D collider;
        public Action OnEnter;
        public Action OnExit;

        [ShowInInspector]
        public List<UnitController> Targets { get; private set; }
        
        private void Awake() {
            this.collider = GetComponent<CircleCollider2D>();
            this.Targets = new List<UnitController>();
        }

        public void SetRange(float range) {
            this.collider.radius = range;
        }

        private void OnTriggerEnter2D(Collider2D coll) {
            var unit = coll.transform.parent.GetComponent<UnitController>();
            if(unit == null)
                return;

            if(this.Targets.Contains(unit)) 
                return;

            this.Targets.Add(unit);
            this.OnEnter?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D coll) {
            var unit = coll.transform.parent.GetComponent<UnitController>();
            if(unit == null)
                return;

            if(this.Targets.Contains(unit)) {
                this.Targets.Remove(unit);
                this.OnExit?.Invoke();
            }
        }
    }
}