// System
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// UnityEngine
using UnityEngine;
// OdinInspector
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    [CreateAssetMenu(fileName = "CraftingLevelData", menuName = "B409/Crafting Level Data")]
    public class CraftingLevelTable : RecipeLevelTable<CraftingLevelTable, CraftingData> {

    }
}