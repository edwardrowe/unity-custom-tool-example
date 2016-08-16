namespace RedBlueGames.ToolsExamples
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public class PrimitiveCreator
    {
        private static readonly Dictionary<Config.PrimitiveColor, string> materialPaths =
            new Dictionary<Config.PrimitiveColor, string>()
            {
                { Config.PrimitiveColor.Red, "Red" },
                { Config.PrimitiveColor.Green, "Green" },
                { Config.PrimitiveColor.Blue, "Blue" }
            };

        public static GameObject CreatePrimitive(Config primitiveConfig)
        {
            var shape = GameObject.CreatePrimitive(primitiveConfig.PrimitiveType);
            shape.transform.localScale = Vector3.one * primitiveConfig.UniformScale;

            // Assign the material for the new object
            var materialResourcePath = materialPaths[primitiveConfig.Color];
            var material = (Material)Resources.Load(materialResourcePath);
            if (material == null)
            {
                Debug.LogError("No material resource found at resource path" +
                    materialResourcePath);
            
                return null;
            }

            shape.GetComponent<Renderer>().material = material;

            return shape;
        }

        [System.Serializable]
        public class Config
        {
            public string Name;
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