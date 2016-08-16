namespace RedBlueGames.ToolsExamples
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;

    public class PrimitiveCreatorNamePresetWindow : EditorWindow
    {
        private string text;
        private GUIContent textLabel;

        public NameSavedEvent NameSaved;

        private void OnEnable()
        {
            this.NameSaved = new NameSavedEvent();
            this.textLabel = new GUIContent("Preset Name");
        }

        private void OnGUI()
        {
            this.text = EditorGUILayout.TextField(this.textLabel, this.text);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                this.Close();
            }
            if (GUILayout.Button("Save"))
            {
                this.Close();
                this.NameSaved.Invoke(this.text);
            }
            EditorGUILayout.EndHorizontal();
        }

        [System.Serializable]
        public class NameSavedEvent : UnityEvent<string>
        {
        }
    }
}