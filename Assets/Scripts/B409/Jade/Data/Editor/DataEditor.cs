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
        private static string ItemDataFolderPath = "Assets/Prefabs/Items";
        private static string CookingDataFolderPath = "Assets/Prefabs/Cookings";
        private static string CraftingDataFolderPath = "Assets/Prefabs/Crafting";
        private static string JewerlyDataFolderPath = "Assets/Prefabs/Jewerly";

        private CreateNewItemData createNewItemData;
        private CreateNewCookingData createNewCookingData;
        private CreateNewCraftingData createNewCraftingData;
        private CreateNewJewerlyData createNewJewerlyData;

        [MenuItem("B409/Game Data")]
        private static void OpenWindow() {
            var window = GetWindow<DataEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 700);
            window.Show();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if(createNewItemData != null) {
                DestroyImmediate(createNewItemData.itemData);
            }
        }

        protected override void OnBeginDrawEditors() {
            OdinMenuTreeSelection selected = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar(); 

            {
                GUILayout.FlexibleSpace();

                if(SirenixEditorGUI.ToolbarButton("Delete Current")) {
                    var asset = selected.SelectedValue;
                    if(asset is ItemData) {
                        string path = AssetDatabase.GetAssetPath(asset as ItemData);
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                    }
                }
            }

            SirenixEditorGUI.EndHorizontalToolbar();
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree(true);

            createNewItemData = new CreateNewItemData();
            tree.Add("Item/Create New", createNewItemData);
            tree.AddAllAssetsAtPath("Item/Items", ItemDataFolderPath, typeof(ItemData));
            tree.EnumerateTree().AddIcons<ItemData>(e => e.Icon);

            createNewCookingData = new CreateNewCookingData();
            tree.Add("Cooking/Create New", createNewCookingData);
            tree.AddAllAssetsAtPath("Cooking/Cookings", CookingDataFolderPath, typeof(CookingData));
            tree.EnumerateTree().AddIcons<CookingData>(e => e.Result.Item?.Icon);

            createNewCraftingData = new CreateNewCraftingData();
            tree.Add("Crafting/Create New", createNewCraftingData);
            tree.AddAllAssetsAtPath("Crafting/Craftings", CraftingDataFolderPath, typeof(CraftingData));
            tree.EnumerateTree().AddIcons<CraftingData>(e => e.Result.Item?.Icon);

            createNewJewerlyData = new CreateNewJewerlyData();
            tree.Add("Jewerly/Create New", createNewJewerlyData);
            tree.AddAllAssetsAtPath("Jewerly/Jewerlys", JewerlyDataFolderPath, typeof(JewerlyData));
            tree.EnumerateTree().AddIcons<JewerlyData>(e => e.Result.Item?.Icon);

            tree.EnumerateTree().Where(e => e.Value as ItemData).ForEach(AddDragHandles);

            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem) {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        #region Create Menu
        [System.Serializable]
        public class CreateNewItemData {
            [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public ItemData itemData;

            public CreateNewItemData() {
                itemData = CreateInstance<ItemData>();
            }

            [Button("Add New Item data", Style = ButtonStyle.Box, ButtonHeight = 50)]
            private void CreateNewData() {
                AssetDatabase.CreateAsset(itemData, Path.ChangeExtension(Path.Combine(ItemDataFolderPath, itemData.Name), "asset"));
                AssetDatabase.SaveAssets();

                itemData = CreateInstance<ItemData>();
            }
        }

        [System.Serializable]
        public class CreateNewCookingData {
            [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public CookingData cookingData;

            public CreateNewCookingData() {
                cookingData = CreateInstance<CookingData>();
            }

            [Button("Add New Item data", Style = ButtonStyle.Box, ButtonHeight = 50)]
            private void CreateNewData() {
                AssetDatabase.CreateAsset(cookingData, Path.ChangeExtension(Path.Combine(CookingDataFolderPath, cookingData.Result.Item?.Name), "asset"));
                AssetDatabase.SaveAssets();

                cookingData = CreateInstance<CookingData>();
            }
        }

        [System.Serializable]
        public class CreateNewCraftingData {
            [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public CraftingData craftingData;

            public CreateNewCraftingData() {
                craftingData = CreateInstance<CraftingData>();
            }

            [Button("Add New Item data", Style = ButtonStyle.Box, ButtonHeight = 50)]
            private void CreateNewData() {
                AssetDatabase.CreateAsset(craftingData, Path.ChangeExtension(Path.Combine(CraftingDataFolderPath, craftingData.Result.Item?.Name), "asset"));
                AssetDatabase.SaveAssets();

                craftingData = CreateInstance<CraftingData>();
            }
        }

        [System.Serializable]
        public class CreateNewJewerlyData {
            [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public JewerlyData jewerlyData;

            public CreateNewJewerlyData() {
                jewerlyData = CreateInstance<JewerlyData>();
            }

            [Button("Add New Item data", Style = ButtonStyle.Box, ButtonHeight = 50)]
            private void CreateNewData() {
                AssetDatabase.CreateAsset(jewerlyData, Path.ChangeExtension(Path.Combine(JewerlyDataFolderPath, jewerlyData.Result.Item?.Name), "asset"));
                AssetDatabase.SaveAssets();

                jewerlyData = CreateInstance<JewerlyData>();
            }
        }
        #endregion
    }
}