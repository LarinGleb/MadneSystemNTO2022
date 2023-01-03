using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SavingTest)), CanEditMultipleObjects]
public class SavingTestWindow : Editor
{  
    public override void OnInspectorGUI () {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save", EditorStyles.miniButton)) {
            var st = (SavingTest)target;
            st.BeginSave();
        }
    }
}
