using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class CubeCreator
{
    private static readonly Dictionary<Settings.CubeColor, string> materialPaths =
        new Dictionary<Settings.CubeColor, string>()
        {
            { Settings.CubeColor.Red, "Red" },
            { Settings.CubeColor.Green, "Green" },
            { Settings.CubeColor.Blue, "Blue" }
        };

    public static GameObject CreateCube(Settings cubeSettings)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(
            cubeSettings.UniformScale,
            cubeSettings.UniformScale,
            cubeSettings.UniformScale);

        var materialResourcePath = materialPaths[cubeSettings.Color];
        var material = (Material)Resources.Load(materialResourcePath);
        if (material == null)
        {
            Debug.LogError("No material resource found at resource path" +
                materialResourcePath);
            
            return null;
        }

        cube.GetComponent<Renderer>().material = material;

        return cube;
    }

    public struct Settings
    {
        public CubeColor Color;
        public float UniformScale;

        public enum CubeColor
        {
            Red,
            Blue,
            Green
        }
    }
}
