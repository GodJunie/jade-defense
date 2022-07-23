// System
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// UnityEngine
using UnityEngine;
// UnityEditor
using UnityEditor;
// OdinIndpector
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;

namespace B409.Jade.Data.Editor {
    public class DataEditor : OdinMenuEditorWindow {
        #region Create Menu Settings
        // Item
        private static string ItemDataFolderPath = "Assets/Datas/Items";
        // Recipe
        private static string CookingDataFolderPath = "Assets/Datas/Cookings";
        private static string CraftingDataFolderPath = "Assets/Datas/Craftings";
        private static string JewelleryDataFolderPath = "Assets/Datas/Jewelleries";
        // Unit
        private static string MonsterDataFolderPath = "Assets/Datas/Monsters";
        private static string EnemyDataFolderPath = "Assets/Datas/Enemies";
        // Table
        private static string TableDataFolderPath = "Assets/Datas/Tables";
        // Battle
        private static string BattleDataFolderPath = "Assets/Datas/Battles";
        // Stage
        private static string StageDataFolderPath = "Assets/Datas/Stages";

        private CreateNewData<ItemData> createNewItemData;
        private CreateNewData<CookingData> createNewCookingData;
        private CreateNewData<CraftingData> createNewCraftingData;
        private CreateNewData<JewelleryData> createNewJewelleryData;
        private CreateNewData<MonsterData> createNewMonsterData;
        private CreateNewData<EnemyData> createNewEnemyData;
        private CreateNewData<BattleData> createNewBattleData;
        private CreateNewData<StageData> createNewStageData;

        private void AddCreateMenu(OdinMenuTree tree) {
            createNewItemData = new CreateNewData<ItemData>(ItemDataFolderPath);
            tree.Add("1. Item", createNewItemData);
            tree.AddAllAssetsAtPath("1. Item", ItemDataFolderPath, typeof(ItemData));
            tree.EnumerateTree().AddIcons<ItemData>(e => e.Icon);

            createNewCookingData = new CreateNewData<CookingData>(CookingDataFolderPath);
            tree.Add("2. Cooking", createNewCookingData);
            tree.AddAllAssetsAtPath("2. Cooking", CookingDataFolderPath, typeof(CookingData));
            tree.EnumerateTree().AddIcons<CookingData>(e => e.Result.Item?.Icon);

            createNewCraftingData = new CreateNewData<CraftingData>(CraftingDataFolderPath);
            tree.Add("3. Crafting", createNewCraftingData);
            tree.AddAllAssetsAtPath("3. Crafting", CraftingDataFolderPath, typeof(CraftingData));
            tree.EnumerateTree().AddIcons<CraftingData>(e => e.Result.Item?.Icon);

            createNewJewelleryData = new CreateNewData<JewelleryData>(JewelleryDataFolderPath);
            tree.Add("4. Jewelry", createNewJewelleryData);
            tree.AddAllAssetsAtPath("4. Jewelry", JewelleryDataFolderPath, typeof(JewelleryData));
            tree.EnumerateTree().AddIcons<JewelleryData>(e => e.Result.Item?.Icon);

            createNewMonsterData = new CreateNewData<MonsterData>(MonsterDataFolderPath);
            tree.Add("5. Monster", createNewMonsterData);
            tree.AddAllAssetsAtPath("5. Monster", MonsterDataFolderPath, typeof(MonsterData));
            tree.EnumerateTree().AddIcons<MonsterData>(e => e.Icon);

            createNewEnemyData = new CreateNewData<EnemyData>(EnemyDataFolderPath);
            tree.Add("6. Enemy", createNewEnemyData);
            tree.AddAllAssetsAtPath("6. Enemy", EnemyDataFolderPath, typeof(EnemyData));
            tree.EnumerateTree().AddIcons<EnemyData>(e => e.Icon);

            createNewBattleData = new CreateNewData<BattleData>(BattleDataFolderPath);
            tree.Add("7. Battle", createNewBattleData);
            tree.AddAllAssetsAtPath("7. Battle", BattleDataFolderPath, typeof(BattleData));

            createNewStageData = new CreateNewData<StageData>(StageDataFolderPath);
            tree.Add("8. Stage", createNewStageData);
            tree.AddAllAssetsAtPath("8. Stage", StageDataFolderPath, typeof(StageData));

            tree.AddAllAssetsAtPath("9. Table", TableDataFolderPath, typeof(ScriptableObject));


            tree.EnumerateTree().Where(e => e.Value is ItemData).ForEach(AddDragHandles);
            tree.EnumerateTree().Where(e => e.Value is RecipeData).ForEach(AddDragHandles);
            tree.EnumerateTree().Where(e => e.Value is UnitData).ForEach(AddDragHandles);
            tree.EnumerateTree().Where(e => e.Value is BattleData).ForEach(AddDragHandles);


            tree.SortMenuItemsByName();
        }
        #endregion

        [MenuItem("B409/Game Data")]
        private static void OpenWindow() {
            var window = GetWindow<DataEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 700);
            window.Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if(createNewItemData != null) {
                DestroyImmediate(createNewItemData.data);
            }
            if(createNewCookingData != null) {
                DestroyImmediate(createNewCookingData.data);
            }
            if(createNewCraftingData != null) {
                DestroyImmediate(createNewCraftingData.data);
            }
            if(createNewJewelleryData != null) {
                DestroyImmediate(createNewJewelleryData.data);
            }
            if(createNewMonsterData != null) {
                DestroyImmediate(createNewMonsterData.data);
            }
            if(createNewEnemyData != null) {
                DestroyImmediate(createNewEnemyData.data);
            }
            if(createNewBattleData != null) { 
                DestroyImmediate(createNewBattleData.data);
            }
            if(createNewStageData != null) {
                DestroyImmediate(createNewStageData.data);
            }
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selected = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar(); 

            {
                GUILayout.FlexibleSpace();

                if(SirenixEditorGUI.ToolbarButton("Save")) {
                    var asset = selected.SelectedValue;
                    if(asset is IDataID) {
                        string path = AssetDatabase.GetAssetPath(asset as ScriptableObject);
                        AssetDatabase.RenameAsset(path, (asset as IDataID).Id.ToString());
                        AssetDatabase.SaveAssets();
                    }
                }

                if(SirenixEditorGUI.ToolbarButton("Delete Current")) {
                    var asset = selected.SelectedValue;
                    if(asset is ScriptableObject) {
                        string path = AssetDatabase.GetAssetPath(asset as ScriptableObject);
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                    }
                }
            }

            SirenixEditorGUI.EndHorizontalToolbar();
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree(true);

            AddCreateMenu(tree);

            tree.Config.DrawSearchToolbar = true;

            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem) {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        #region Create Menu
        [System.Serializable]
        public class CreateNewData<T> where T : ScriptableObject, IDataID {
            [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public T data;
            private string path;

            public CreateNewData(string path) {
                this.path = path;
                data = CreateInstance<T>();
            }

            [Button("Add New Data", Style = ButtonStyle.Box, ButtonHeight = 50)]
            private void CreateNew() {
                AssetDatabase.CreateAsset(data, Path.ChangeExtension(Path.Combine(path, data.Id.ToString()), "asset"));
                AssetDatabase.SaveAssets();

                data = CreateInstance<T>();
            }
        }
        #endregion
    }
}