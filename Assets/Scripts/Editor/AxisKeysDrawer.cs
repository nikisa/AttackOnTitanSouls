using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AxisKeys))]
public class AxisKeysDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        EditorGUI.BeginProperty(position, label, property);

        //Don't indent
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Set position rects
        Rect posLabel = new Rect(position.x, position.y, 15, position.height);
        Rect posField = new Rect(position.x + 20 , position.y, 50, position.height);
        Rect negLabel = new Rect(position.x + 75 , position.y, 15, position.height);
        Rect negField = new Rect(position.x + 95 , position.y, 50, position.height);

        // Set Labels
        GUIContent posGUI = new GUIContent("+");
        GUIContent negGUI = new GUIContent("-");

        // Draw fields
        EditorGUI.LabelField(posLabel, posGUI);
        EditorGUI.PropertyField(posField, property.FindPropertyRelative("positive") , GUIContent.none);
        EditorGUI.LabelField(negLabel, negGUI);
        EditorGUI.PropertyField(negField, property.FindPropertyRelative("negative"), GUIContent.none);

        // reset indent
        EditorGUI.indentLevel = indent;


        // End Property
        EditorGUI.EndProperty();

    }

}
