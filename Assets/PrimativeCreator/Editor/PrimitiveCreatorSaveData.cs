namespace RedBlueGames.ToolsExamples
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class PrimitiveCreatorSaveData : ScriptableObject
    {
        public List<PrimitiveCreator.Config> ConfigPresets;

        public bool HasConfigForIndex(int index)
        {
            if (this.ConfigPresets == null || this.ConfigPresets.Count == 0)
            {
                return false;
            }
            
            return index >= 0 && index < this.ConfigPresets.Count;
        }
    }
}