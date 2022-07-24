using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    [InlineEditor]
    public class StageSequenceData : ScriptableObject, IDataID {
        [SerializeField]
        private int id;

        public int Id => id;
    }
}
