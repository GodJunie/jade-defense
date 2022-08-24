using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Spine.Unity;

namespace B409.Jade.Battle {
    public class UnitGenerator : MonoBehaviour {
        [SerializeField]
        private HpBar hpBarPrefab;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

#if UNITY_EDITOR
        [Button]
        private void GenerateMonster(SkeletonDataAsset asset) {
            var str = asset.name.Split(new char[] { '_' });
            var name = str[0];

            // main game Object
            var g = new GameObject();
            var unit = g.AddComponent<UnitController>();
            g.name = name;

            // animation game object (hit box attach)
            var a = new GameObject();
            a.layer = 30;
            var anim = a.AddComponent<SkeletonAnimation>();
            var box = a.AddComponent<BoxCollider2D>();
            box.isTrigger = true;
            anim.skeletonDataAsset = asset;
            anim.Initialize(true);
            a.transform.SetParent(g.transform);
            a.name = name;
            var mesh = anim.GetComponent<MeshRenderer>();
            mesh.sortingLayerName = "Character";
            a.transform.localScale = Vector3.one * .6f;

            // detector
            var d = new GameObject();
            d.layer = 31;
            var circle = d.AddComponent<CircleCollider2D>();
            var rigid = d.AddComponent<Rigidbody2D>();
            var detector = d.AddComponent<Detector>();
            d.name = "Detector";
            circle.isTrigger = true;
            rigid.gravityScale = 0;
            d.transform.SetParent(g.transform);

            // hp bar
            var hpBar = Instantiate(hpBarPrefab, g.transform);
            hpBar.name = "HpBar";

            unit.InitObjects();
        }
#endif
    }
}