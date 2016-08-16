namespace RedBlueGames.ToolsExamples
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;

    /// <summary>
    /// PrimitiveCreatorPrefKeys stores keys in EditorPrefs or PlayerPrefs for PrimitiveCreator
    /// values
    /// </summary>
    public static class PrimitiveCreatorPrefKeys
    {
        private const string PreferencePrefix = "PrimitiveCreator/";

        public static string PreviousConfigIndex
        {
            get
            {
                return string.Concat(PreferencePrefix, ProjectID, "PreviousConfigIndex");
            }
        }

        public static string UniformScale
        {
            get
            {
                return string.Concat(PreferencePrefix, ProjectID, "UniformScale");
            }
        }

        public static string Color
        {
            get
            {
                return string.Concat(PreferencePrefix, ProjectID, "Color");
            }
        }

        public static string PrimitiveType
        {
            get
            {
                return string.Concat(PreferencePrefix, ProjectID, "PrimitiveType");
            }
        }

        private static string ProjectID
        {
            get
            {
                return string.Concat(PlayerSettings.companyName, ".", PlayerSettings.productName, "/");
            }
        }
    }
}