using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Data {
    public class DialogueData : ScriptableObject {
        public class DialogueBlock {
            [SerializeField]
            private string script;
        }
    }
}