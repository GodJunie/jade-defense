using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;


namespace B409.Jade.Data {
    public class UnitDataDrawer<TUnitData> : OdinValueDrawer<TUnitData> where TUnitData : UnitData {
        protected override void DrawPropertyLayout(GUIContent label) {
            var rect = EditorGUILayout.GetControlRect(label != null, 45);

            if(label != null) {
                rect.xMin = EditorGUI.PrefixLabel(rect.AlignCenterY(15), label).xMin;
            } else {
                rect = EditorGUI.IndentedRect(rect);
            }

            UnitData unit = this.ValueEntry.SmartValue;
            Texture texture = null;

            if(unit) {
                texture = GUIHelper.GetAssetThumbnail(unit.Icon, typeof(UnitData), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : string.Format("cooltime: {0}, name: {1}", unit.Status.Cooltime, unit.Name));
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), unit, texture, this.ValueEntry.BaseValueType);
        }
    }
}