using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Spine.Unity;

namespace B409.Jade.Battle {
    public class UnitGenerator : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        [Button]
        private void GenerateMonster(SkeletonDataAsset asset) {
            var g = new GameObject();
            var a = new GameObject();
            var anim = a.AddComponent<SkeletonAnimation>();
            anim.skeletonDataAsset = asset;
            anim.Initialize(true);
        }
    }
}