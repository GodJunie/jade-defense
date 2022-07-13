using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Data {
    public class DataManager : SingletonBehaviour<DataManager> {
        [SerializeField]
        private List<ItemData> items;
        [SerializeField]
        private List<CookingData> cookings;
        [SerializeField]
        private List<JewelleryData> jewelleries;
        [SerializeField]
        private List<CraftingData> craftings;


        public List<ItemData> Items => items;
        public List<CookingData> Cookings => cookings;
        public List<JewelleryData> Jewelleries => jewelleries;
        public List<CraftingData> Craftings => craftings;
    }
}
