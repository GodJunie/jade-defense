using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    public class DataManager : SingletonBehaviour<DataManager> {
        [AssetList(Path = "/Datas/Items", AutoPopulate = true)]
        [SerializeField]
        private List<ItemData> items;
        [AssetList(Path = "/Datas/Cookings", AutoPopulate = true)]
        [SerializeField]
        private List<CookingData> cookings;
        [AssetList(Path = "/Datas/Jewelleries", AutoPopulate = true)]
        [SerializeField]
        private List<JewelleryData> jewelleries;
        [AssetList(Path = "/Datas/Craftings", AutoPopulate = true)]
        [SerializeField]
        private List<CraftingData> craftings;
        [AssetList(Path = "/Datas/Stages", AutoPopulate = true)]
        [SerializeField]
        private List<StageData> stages;
             

        public List<ItemData> Items => items;
        public List<CookingData> Cookings => cookings;
        public List<JewelleryData> Jewelleries => jewelleries;
        public List<CraftingData> Craftings => craftings;
        public List<StageData> Stages => stages;
    }
}
