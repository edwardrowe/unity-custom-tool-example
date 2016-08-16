namespace RedBlueGames.ToolsExamples
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections;

    public class PrimitiveCreatorWindow : EditorWindow
    {
        private const string SaveDataPath = "Assets/Resources/PrimitiveCreatorSaveData.asset";

        private PrimitiveCreatorSaveData saveData;
        private PrimitiveCreator.Config currentConfig;
        private int selectedConfigPreset;

        private GUIContent[] configPresetsNames;
        private GUIContent savedConfigLabel;
        private GUIContent presetNameLabel;
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
            this.LoadSaveData();
            this.LoadConfigFromPreferences();

            this.savedConfigLabel = new GUIContent("Config Preset");
            this.presetNameLabel = new GUIContent("Preset Name");
            this.primitiveTypeLabel = new GUIContent("Primitive");
            this.uniformScaleLabel = new GUIContent("Uniform Scale");
            this.colorLabel = new GUIContent("Color");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            this.selectedConfigPreset = EditorGUILayout.Popup(
                this.savedConfigLabel,
                selectedConfigPreset,
                this.configPresetsNames);

            if (EditorGUI.EndChangeCheck())
            {
                this.LoadConfigPresetByIndex(this.selectedConfigPreset);
            }

            EditorGUI.BeginDisabledGroup(this.saveData.HasConfigForIndex(this.selectedConfigPreset));
            if (GUILayout.Button("Save"))
            {
                var namePresetWindow = EditorWindow.GetWindow<PrimitiveCreatorNamePresetWindow>(
                                           true, 
                                           "Name Preset", 
                                           true);

                namePresetWindow.NameSaved.AddListener((string name) =>
                    {
                        this.currentConfig.Name = name;
                        this.saveData.ConfigPresets.Add(this.currentConfig);
                        this.PopulateConfigPresetsDropdown();

                        this.selectedConfigPreset = this.saveData.ConfigPresets.Count - 1;
                        this.LoadConfigPresetByIndex(this.selectedConfigPreset);
                    });
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!this.saveData.HasConfigForIndex(this.selectedConfigPreset));
            if (GUILayout.Button("Rename"))
            {
                var namePresetWindow = EditorWindow.GetWindow<PrimitiveCreatorNamePresetWindow>(
                                           true, 
                                           "Rename Preset", 
                                           true);

                namePresetWindow.NameSaved.AddListener((string name) =>
                    {
                        this.currentConfig.Name = name;
                        this.PopulateConfigPresetsDropdown();
                    });
            }

            if (GUILayout.Button("Delete"))
            {
                this.saveData.ConfigPresets.Remove(this.currentConfig);
                this.selectedConfigPreset = this.saveData.ConfigPresets.Count;
                this.PopulateConfigPresetsDropdown();
                this.LoadConfigPresetByIndex(this.selectedConfigPreset);
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            this.currentConfig.PrimitiveType = (PrimitiveType)EditorGUILayout.EnumPopup(
                this.primitiveTypeLabel,
                this.currentConfig.PrimitiveType);

            this.currentConfig.UniformScale = EditorGUILayout.FloatField(
                this.uniformScaleLabel,
                this.currentConfig.UniformScale);
        
            this.currentConfig.Color = (PrimitiveCreator.Config.PrimitiveColor)
                EditorGUILayout.EnumPopup(this.colorLabel, this.currentConfig.Color);
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Create Primitive"))
            {
                GameObject shape = PrimitiveCreator.CreatePrimitive(this.currentConfig);

                Selection.activeGameObject = shape;
            }

            this.SaveSettings();
        }

        private void LoadSaveData()
        {
            // This call runs a lot of code, but the basic gist is it's just loading the 
            // save data, or creating the file and any directories if it doesn't exist.
            this.saveData =
                ScriptableObjectUtility.LoadOrCreateSaveData<PrimitiveCreatorSaveData>(SaveDataPath);

            this.PopulateConfigPresetsDropdown();
        }

        private void PopulateConfigPresetsDropdown()
        {
            int numSavedConfigs = this.saveData.ConfigPresets.Count;
            this.configPresetsNames = new GUIContent[numSavedConfigs + 1];
            for (int i = 0; i < numSavedConfigs; ++i)
            {
                this.configPresetsNames[i] = new GUIContent(this.saveData.ConfigPresets[i].Name);
            }

            this.configPresetsNames[numSavedConfigs] = new GUIContent("New Preset");
        }

        private void LoadConfigFromPreferences()
        {
            var lastConfigPresetIndex = EditorPrefs.GetInt(PrimitiveCreatorPrefKeys.PreviousConfigIndex);
            this.selectedConfigPreset = lastConfigPresetIndex;
            this.LoadConfigPresetByIndex(lastConfigPresetIndex);
        }

        private void LoadConfigPresetByIndex(int index)
        {
            if (this.saveData.HasConfigForIndex(this.selectedConfigPreset))
            {
                this.currentConfig = this.saveData.ConfigPresets[this.selectedConfigPreset];
            }
            else
            {
                this.currentConfig = new PrimitiveCreator.Config();

                this.currentConfig.PrimitiveType = (PrimitiveType)
                    EditorPrefs.GetInt(PrimitiveCreatorPrefKeys.Color);

                this.currentConfig.UniformScale = EditorPrefs.GetFloat(PrimitiveCreatorPrefKeys.UniformScale);

                this.currentConfig.Color = (PrimitiveCreator.Config.PrimitiveColor)
                    EditorPrefs.GetInt(PrimitiveCreatorPrefKeys.PrimitiveType);
            }
        }

        private void SaveSettings()
        {
            EditorPrefs.SetInt(PrimitiveCreatorPrefKeys.PreviousConfigIndex, this.selectedConfigPreset);

            if (!this.saveData.HasConfigForIndex(this.selectedConfigPreset))
            {
                EditorPrefs.SetInt(PrimitiveCreatorPrefKeys.Color, (int)this.currentConfig.PrimitiveType);
                EditorPrefs.SetFloat(PrimitiveCreatorPrefKeys.UniformScale, this.currentConfig.UniformScale);
                EditorPrefs.SetInt(PrimitiveCreatorPrefKeys.PrimitiveType, (int)this.currentConfig.Color);
            }
        }
    }
}