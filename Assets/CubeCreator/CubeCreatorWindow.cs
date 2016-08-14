using UnityEditor;
using UnityEngine;
using System.Collections;

public class CubeCreatorWindow : EditorWindow
{
    private CubeCreator.Settings cubeSettings;

    [MenuItem("Window/Cube Creator")]
    public static void ShowCubeCreatorWindow()
    {
        EditorWindow.GetWindow<CubeCreatorWindow>();
    }

    public void OnGUI()
    {
        this.cubeSettings.UniformScale = EditorGUILayout.FloatField(
            new GUIContent("Uniform Scale"),
            this.cubeSettings.UniformScale);
        
        this.cubeSettings.Color = (CubeCreator.Settings.CubeColor)
            EditorGUILayout.EnumPopup(new GUIContent("Color"), this.cubeSettings.Color);

        if (GUILayout.Button("Create Cube"))
        {
            GameObject cube = CubeCreator.CreateCube(this.cubeSettings);

            Selection.activeGameObject = cube;
        }
    }
}
