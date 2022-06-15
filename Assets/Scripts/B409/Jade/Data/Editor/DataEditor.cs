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
        private static string CraftingDataFolderPath = "Assets/Prefabs/Craftings";
        private static string JewelleryDataFolderPath = "Assets/Prefabs/Jewelleries";
        private static string TableDataFolderPath = "Assets/Prefabs/Tables";

        private CreateNewItemData createNewItemData;
        private CreateNewRecipeData<CookingData> createNewCookingData;
        private CreateNewRecipeData<CraftingData> createNewCraftingData;
        private CreateNewRecipeData<JewelleryData> createNewJewelleryData;

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

                if(SirenixEditorGUI.ToolbarButton("Save")) {
                    var asset = selected.SelectedValue;
                    if(asset is ItemData) {
                        string path = AssetDatabase.GetAssetPath(asset as ScriptableObject);
                        AssetDatabase.RenameAsset(path, (asset as ItemData).Id.ToString());
                        AssetDatabase.SaveAssets();
                    }
                    if(asset is RecipeData) {
                        string path = AssetDatabase.GetAssetPath(asset as ScriptableObject);
                        AssetDatabase.RenameAsset(path, (asset as RecipeData).Id.ToString());
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

            createNewItemData = new CreateNewItemData();
            tree.Add("Item", createNewItemData);
            tree.AddAllAssetsAtPath("Item", ItemDataFolderPath, typeof(ItemData));
            tree.EnumerateTree().AddIcons<ItemData>(e => e.Icon);

            createNewCookingData = new CreateNewRecipeData<CookingData>(CookingDataFolderPath);
            tree.Add("Cooking", createNewCookingData);
            tree.AddAllAssetsAtPath("Cooking", CookingDataFolderPath, typeof(CookingData));
            tree.EnumerateTree().AddIcons<CookingData>(e => e.Result.Item?.Icon);

            createNewCraftingData = new CreateNewRecipeData<CraftingData>(CraftingDataFolderPath);
            tree.Add("Crafting", createNewCraftingData);
            tree.AddAllAssetsAtPath("Crafting", CraftingDataFolderPath, typeof(CraftingData));
            tree.EnumerateTree().AddIcons<CraftingData>(e => e.Result.Item?.Icon);

            createNewJewelleryData = new CreateNewRecipeData<JewelleryData>(JewelleryDataFolderPath);
            tree.Add("Jewellery", createNewJewelleryData);
            tree.AddAllAssetsAtPath("Jewellery", JewelleryDataFolderPath, typeof(JewelleryData));
            tree.EnumerateTree().AddIcons<JewelleryData>(e => e.Result.Item?.Icon);

            tree.EnumerateTree().Where(e => e.Value is ItemData).ForEach(AddDragHandles);
            tree.EnumerateTree().Where(e => e.Value is RecipeData).ForEach(AddDragHandles);

            tree.AddAllAssetsAtPath("Tables", TableDataFolderPath, typeof(ScriptableObject));

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
        public class CreateNewRecipeData<T> where T : RecipeData {
            [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public T data;
            private string path;

            public CreateNewRecipeData(string path) {
                this.path = path;
                data = CreateInstance<T>();
            }

            [Button("Add New Item data", Style = ButtonStyle.Box, ButtonHeight = 50)]
            private void CreateNewData() {
                AssetDatabase.CreateAsset(data, Path.ChangeExtension(Path.Combine(path, data.Id.ToString()), "asset"));
                AssetDatabase.SaveAssets();

                data = CreateInstance<T>();
            }
        }
        #endregion
    }
}