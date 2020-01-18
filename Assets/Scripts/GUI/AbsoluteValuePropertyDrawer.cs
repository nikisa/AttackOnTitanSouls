using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbsoluteValueAttribute))]
public class AbsoluteValuePropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        if (property.propertyType == SerializedPropertyType.Integer) {
            if (property.intValue < 0) {
                property.intValue = Mathf.Abs(EditorGUI.IntField(position, label, property.intValue));
            }
            else if(property.intValue == 0) {
                property.intValue = 1;
            }
        }

        else if (property.propertyType == SerializedPropertyType.Float) {
            if (property.floatValue < 0) {
                property.floatValue = Mathf.Abs(EditorGUI.FloatField(position, label, property.floatValue));
            }
            else if (property.floatValue == 0) {
                property.floatValue = float.MinValue;
            }    
        }
        else {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
