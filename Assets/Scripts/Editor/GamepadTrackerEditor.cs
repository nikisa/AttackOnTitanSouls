using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GamepadTracker))]
public class GamepadTrackerEditor : Editor
{
    public override void OnInspectorGUI() {
        GamepadTracker gt = target as GamepadTracker;

        EditorGUILayout.LabelField("Axes", EditorStyles.boldLabel);

        if (gt.axisKeys.Length == 0) {
            EditorGUILayout.HelpBox("No axes defined in InputManager.", MessageType.Info);
        }
        else {
            SerializedProperty prop = serializedObject.FindProperty("axisKeys");
            for (int i = 0; i < gt.axisKeys.Length; i++) {
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), new GUIContent("Axis" + i));
            }
        }

        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

        if (gt.buttonKeys.Length == 0) {
            EditorGUILayout.HelpBox("No buttons defined in InputManager.", MessageType.Info);
        }
        else {
            for (int i = 0; i < gt.buttonKeys.Length; i++) {
                gt.buttonKeys[i] = (KeyCode) EditorGUILayout.EnumPopup("Button " + i, gt.buttonKeys[i]);
            }
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
