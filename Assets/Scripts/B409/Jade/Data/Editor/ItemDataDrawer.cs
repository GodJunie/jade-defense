using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace B409.Jade.Data {
    public class ItemDataDrawer : OdinValueDrawer<ItemData> {
        protected override void DrawPropertyLayout(GUIContent label) {
            var rect = EditorGUILayout.GetControlRect(label != null, 45);

            if(label != null) {
                rect.xMin = EditorGUI.PrefixLabel(rect.AlignCenterY(15), label).xMin;
            } else {
                rect = EditorGUI.IndentedRect(rect);
            }

            ItemData item = this.ValueEntry.SmartValue;
            Texture texture = null;

            if(item) {
                texture = GUIHelper.GetAssetThumbnail(item.Icon, typeof(ItemData), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : item.Name);
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), item, texture, this.ValueEntry.BaseValueType);
        }
    }
}