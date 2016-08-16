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

        private GUIContent configHeaderLabel;
        private GUIContent primitiveTypeLabel;
        private GUIContent uniformScaleLabel;
        private GUIContent colorLabel;

        private GUIContent saveButtonLabel;
        private GUIContent renameButtonLabel;
        private GUIContent deleteButtonLabel;

        private GUIContent createPrimitiveButtonLabel;

        [MenuItem("Window/Primitive Creator")]
        private static void ShowCubeCreatorWindow()
        {
            EditorWindow.GetWindow<PrimitiveCreatorWindow>("Primitive Creator");
        }

        private void OnEnable()
        {
            this.LoadSaveData();
            this.LoadConfigFromPreferences();

            this.savedConfigLabel = new GUIContent("Selected Preset");

            this.configHeaderLabel = new GUIContent("Primitive Configuration");
            this.primitiveTypeLabel = new GUIContent("Primitive");
            this.uniformScaleLabel = new GUIContent("Uniform Scale");
            this.colorLabel = new GUIContent("Color");

            this.saveButtonLabel = new GUIContent("Save");
            this.renameButtonLabel = new GUIContent("Rename");
            this.deleteButtonLabel = new GUIContent("Delete");

            this.createPrimitiveButtonLabel = new GUIContent("Create Primitive");
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
            if (GUILayout.Button(this.saveButtonLabel))
            {
                var namePresetWindow = PrimitiveCreatorNamePresetWindow.Show("Name Preset");

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
            if (GUILayout.Button(this.renameButtonLabel))
            {
                var namePresetWindow = PrimitiveCreatorNamePresetWindow.Show("Rename Preset",
                                           this.currentConfig.Name);

                namePresetWindow.NameSaved.AddListener((string name) =>
                    {
                        this.currentConfig.Name = name;
                        this.PopulateConfigPresetsDropdown();
                    });
            }

            if (GUILayout.Button(this.deleteButtonLabel))
            {
                this.saveData.ConfigPresets.Remove(this.currentConfig);
                this.selectedConfigPreset = this.saveData.ConfigPresets.Count;
                this.PopulateConfigPresetsDropdown();
                this.LoadConfigPresetByIndex(this.selectedConfigPreset);
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(this.configHeaderLabel, EditorStyles.boldLabel);
            this.currentConfig.PrimitiveType = (PrimitiveType)EditorGUILayout.EnumPopup(
                this.primitiveTypeLabel,
                this.currentConfig.PrimitiveType);

            this.currentConfig.UniformScale = EditorGUILayout.FloatField(
                this.uniformScaleLabel,
                this.currentConfig.UniformScale);
        
            this.currentConfig.Color = (PrimitiveCreator.Config.PrimitiveColor)
                EditorGUILayout.EnumPopup(this.colorLabel, this.currentConfig.Color);
            
            EditorGUILayout.Space();

            if (GUILayout.Button(this.createPrimitiveButtonLabel))
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