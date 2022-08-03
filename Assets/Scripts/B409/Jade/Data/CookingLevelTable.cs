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
    [CreateAssetMenu(fileName = "CookingLevelData", menuName = "B409/Cooking Level Data")]
    public class CookingLevelTable : RecipeLevelTable<CookingLevelTable, CookingLevelData, CookingData> {
      
    }

    public class CookingLevelData : RecipeLevelData<CookingLevelData, CookingData> {

    }
}