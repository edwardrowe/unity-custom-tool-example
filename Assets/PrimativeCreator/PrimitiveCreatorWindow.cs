namespace RedBlueGames.ToolsExamples
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections;

    public class PrimitiveCreatorWindow : EditorWindow
    {
        private PrimitiveCreator.Settings settings;
        private GUIContent primitiveTypeLabel;
        private GUIContent uniformScaleLabel;
        private GUIContent colorLabel;

        [MenuItem("Window/Primitive Creator")]
        public static void ShowCubeCreatorWindow()
        {
            EditorWindow.GetWindow<PrimitiveCreatorWindow>("Primitive Creator");
        }

        public void OnEnable()
        {
            this.settings = new PrimitiveCreator.Settings();

            this.primitiveTypeLabel = new GUIContent("Primitive");
            this.uniformScaleLabel = new GUIContent("Uniform Scale");
            this.colorLabel = new GUIContent("Color");
        }

        public void OnGUI()
        {
            this.settings.PrimitiveType = (PrimitiveType)EditorGUILayout.EnumPopup(
                this.primitiveTypeLabel,
                this.settings.PrimitiveType);

            this.settings.UniformScale = EditorGUILayout.FloatField(
                this.uniformScaleLabel,
                this.settings.UniformScale);
        
            this.settings.Color = (PrimitiveCreator.Settings.PrimitiveColor)
                EditorGUILayout.EnumPopup(this.colorLabel, this.settings.Color);

            if (GUILayout.Button("Create Primitive"))
            {
                GameObject shape = PrimitiveCreator.CreatePrimitive(this.settings);

                Selection.activeGameObject = shape;
            }
        }
    }
}