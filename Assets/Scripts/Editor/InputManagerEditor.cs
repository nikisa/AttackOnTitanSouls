using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor {

    public override void OnInspectorGUI() {

        InputManager im = target as InputManager;

        // Inizio controllo modifiche
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck()) {
            //Se c'è stato un cambiamento richiamo Refresh()
            im.RefreshTracker();
        }

    }
}
