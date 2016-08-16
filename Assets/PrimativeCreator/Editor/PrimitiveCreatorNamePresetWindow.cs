namespace RedBlueGames.ToolsExamples
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;

    public class PrimitiveCreatorNamePresetWindow : EditorWindow
    {
        public NameSavedEvent NameSaved;

        private bool hasUpdated;
        private string text;
        private GUIContent textLabel;

        /// <summary>
        /// Show an instance of PrimativeCreatorNamePresetWindow with the specified title.
        /// </summary>
        /// <param name="windowTitle">Window title.</param>
        public static PrimitiveCreatorNamePresetWindow Show(string windowTitle)
        {
            return Show(windowTitle, string.Empty);
        }

        /// <summary>
        /// Show an instance of PrimativeCreatorNamePresetWindow with the specified title and
        /// optional default text.
        /// </summary>
        /// <param name="windowTitle">Window title.</param>
        /// <param name="defaultText">Default text.</param>
        public static PrimitiveCreatorNamePresetWindow
            Show(string windowTitle, string defaultText)
        {
            var window = EditorWindow.GetWindow<PrimitiveCreatorNamePresetWindow>(
                             true,
                             windowTitle,
                             true);
            window.text = defaultText;

            return window;
        }

        private void OnEnable()
        {
            this.NameSaved = new NameSavedEvent();
            this.textLabel = new GUIContent("Preset Name");

            this.hasUpdated = false;
        }

        private void OnGUI()
        {
            var nameFieldName = "NameField";
            GUI.SetNextControlName(nameFieldName);
            this.text = EditorGUILayout.TextField(this.textLabel, this.text);

            // Focus the name field on the first update
            if (!this.hasUpdated)
            {
                GUI.FocusControl(nameFieldName);
            }

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

            this.hasUpdated = true;
        }

        [System.Serializable]
        public class NameSavedEvent : UnityEvent<string>
        {
        }
    }
}