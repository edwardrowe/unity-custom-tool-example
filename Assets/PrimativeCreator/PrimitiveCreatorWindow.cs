namespace RedBlueGames.ToolsExamples
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections;

    public class PrimitiveCreatorWindow : EditorWindow
    {
        private PrimitiveCreator.Settings currentSettings;

        [MenuItem("Window/Primitive Creator")]
        public static void ShowCubeCreatorWindow()
        {
            EditorWindow.GetWindow<PrimitiveCreatorWindow>("Primitive Creator");
        }

        public void OnEnable()
        {
            this.currentSettings = new PrimitiveCreator.Settings();
        }

        public void OnGUI()
        {
            this.currentSettings.PrimitiveType = 
                (UnityEngine.PrimitiveType)EditorGUILayout.EnumPopup(
                new GUIContent("Primitive"),
                this.currentSettings.PrimitiveType);

            this.currentSettings.UniformScale = EditorGUILayout.FloatField(
                new GUIContent("Uniform Scale"),
                this.currentSettings.UniformScale);
        
            this.currentSettings.Color = (PrimitiveCreator.Settings.PrimitiveColor)
                EditorGUILayout.EnumPopup(new GUIContent("Color"), this.currentSettings.Color);

            if (GUILayout.Button("Create Primitive"))
            {
                GameObject cube = PrimitiveCreator.CreatePrimitive(this.currentSettings);

                Selection.activeGameObject = cube;
            }
        }
    }
}