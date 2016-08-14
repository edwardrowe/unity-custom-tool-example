namespace RedBlueGames.ToolsExamples
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public class PrimitiveCreator
    {
        private static readonly Dictionary<Settings.PrimitiveColor, string> materialPaths =
            new Dictionary<Settings.PrimitiveColor, string>()
            {
                { Settings.PrimitiveColor.Red, "Red" },
                { Settings.PrimitiveColor.Green, "Green" },
                { Settings.PrimitiveColor.Blue, "Blue" }
            };

        public static GameObject CreatePrimitive(Settings primitiveSettings)
        {
            var cube = GameObject.CreatePrimitive(primitiveSettings.PrimitiveType);
            cube.transform.localScale = new Vector3(
                primitiveSettings.UniformScale,
                primitiveSettings.UniformScale,
                primitiveSettings.UniformScale);

            var materialResourcePath = materialPaths[primitiveSettings.Color];
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

        public class Settings
        {
            public UnityEngine.PrimitiveType PrimitiveType;
            public PrimitiveColor Color;
            public float UniformScale = 1.0f;

            public enum PrimitiveColor
            {
                Red,
                Blue,
                Green
            }
        }
    }
}