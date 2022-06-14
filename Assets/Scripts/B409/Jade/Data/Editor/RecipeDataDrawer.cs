using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace B409.Jade.Data {
    public class RecipeDataDrawer<T> : OdinValueDrawer<T> where T : RecipeData {
        protected override void DrawPropertyLayout(GUIContent label) {
            var rect = EditorGUILayout.GetControlRect(label != null, 45);

            if(label != null) {
                rect.xMin = EditorGUI.PrefixLabel(rect.AlignCenterY(15), label).xMin;
            } else {
                rect = EditorGUI.IndentedRect(rect);
            }

            RecipeData recipe = this.ValueEntry.SmartValue;
            Texture texture = null;

            if(recipe) {
                texture = GUIHelper.GetAssetThumbnail(recipe.Result?.Item?.Icon, typeof(ItemData), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : recipe.Result?.Item?.Name);
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), recipe, texture, this.ValueEntry.BaseValueType);
        }
    }
}
