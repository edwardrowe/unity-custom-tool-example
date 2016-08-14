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
        private static void ShowCubeCreatorWindow()
        {
            EditorWindow.GetWindow<PrimitiveCreatorWindow>("Primitive Creator");
        }

        private void OnEnable()
        {
            this.primitiveTypeLabel = new GUIContent("Primitive");
            this.uniformScaleLabel = new GUIContent("Uniform Scale");
            this.colorLabel = new GUIContent("Color");
        }

        private void OnGUI()
        {
            if (this.settings == null)
            {
                this.LoadLastSettings();
            }

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

            this.SaveSettings();
        }

        private void LoadLastSettings()
        {
            this.settings = new PrimitiveCreator.Settings();

            this.settings.PrimitiveType = (PrimitiveType)
                EditorPrefs.GetInt(PrimitiveCreatorPrefKeys.Color);

            this.settings.UniformScale = EditorPrefs.GetFloat(PrimitiveCreatorPrefKeys.UniformScale);

            this.settings.Color = (PrimitiveCreator.Settings.PrimitiveColor)
                EditorPrefs.GetInt(PrimitiveCreatorPrefKeys.PrimitiveType);
        }

        private void SaveSettings()
        {
            EditorPrefs.SetInt(PrimitiveCreatorPrefKeys.Color, (int)this.settings.PrimitiveType);
            EditorPrefs.SetFloat(PrimitiveCreatorPrefKeys.UniformScale, this.settings.UniformScale);
            EditorPrefs.SetInt(PrimitiveCreatorPrefKeys.PrimitiveType, (int)this.settings.Color);
        }
    }
}