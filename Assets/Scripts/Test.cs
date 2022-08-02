using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;


namespace B409.Jade.Game {
    public class Test : MonoBehaviour {
        GameProgress p;


        // Start is called before the first frame update
        void Start() {
           
        }

        // Update is called once per frame
        void Update() {

        }

        [Button]
        private void AAA() {
            p = new GameProgress();
            p.AddItems(new Dictionary<int, int> {
                { 10000, 1 },
                { 10001, 2 },
                { 10002, 3 },
            });

            var json = JsonConvert.SerializeObject(p);

            Debug.Log(json);

            p = JsonConvert.DeserializeObject<GameProgress>(json);

            Debug.Log(p.Items[10000]);
        }
    }
}