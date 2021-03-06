﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using javitechnologies.levelgenerator.data;
using javitechnologies.ballwar.model;
using javitechnologies.ballwar.levelgenerator.spawner;
using javitechnologies.levelgenerator.view;

namespace javitechnologies.levelgenerator.editor
{
    public class LevelEditor : EditorWindow
    {
        enum LevelEditorMode
        {
            LIST_LEVEL_MODE,
            EDIT_LEVEL_MODE
        }

        private const string dataDirectory = "Assets/Data/";

        public static string DataDirectory { get { return dataDirectory; } }

        private static LevelGenerator levelGeneratorScriptFromScene;

        /*
         * The current level generator data scriptable object
         */
        private static LevelGroupData currentLevelDataGroup;

        private static LevelData actualLevelData = null;


        private static LevelEditPhase levelEditPhase;

        // To build the window
        private LevelEditorMode mode = LevelEditorMode.LIST_LEVEL_MODE;
        private Vector2 spawnerListScrollPosition = Vector2.zero;
        Color redishColor = new Color(1f, 0f, 0f, 0.3f);
        Color tableHeaderColor = new Color(0.681f, 0.719f, 0.772f, 1.000f);
//        Color testColor = new Color(0.1f, 0.1f, 0.2f, 1f);
        Color tableItemColor1 = new Color(219.0f/255.0f, 228.0f/255.0f, 1f, 1f);//DBE4FFFF
        Color tableItemColor2 = new Color(0.707f, 0.734f, 0.801f, 1.000f);//DBE4FFFF
        private static GUIStyle labelStyle;
        private bool previousSelectedAllToggleValue = false;
//        private bool creatingNewLevel;


        private string maMessage = "Please select a LevelGenerator object in the scene.";

        [MenuItem("Tools/Custom/Level Editor")]
        static void Init()
        {
            EditorWindow.GetWindow<LevelEditor>("Level Editor");

            labelStyle = new GUIStyle(EditorStyles.boldLabel);
            labelStyle.normal.textColor = Color.black;
            labelStyle.fontStyle = FontStyle.Bold;

            levelEditPhase = new LevelEditPhase();
        }

        /*
         * Returns the name of the selected game object in the scene.
         * 
         * */
        string SelectedGameObjectName { get { return (Selection.activeGameObject != null) ? Selection.activeGameObject.name : ""; } }

        /*
         * OnGUI()
         * Rendering window here
         * 
         * */
        void  OnGUI()
        {
            ExtractCurrentValues();

            switch (mode)
            {
                case LevelEditorMode.LIST_LEVEL_MODE:
                    // Header
                    RendererHeader();

                    // Render Selected Object Info
                    RenderSelectedObject();

                    // nothing else to do if there is not game object selected
                    if (Selection.activeGameObject == null || currentLevelDataGroup == null)
                        return;

                    EditorGUIUtility.labelWidth = 60f;

                    GUILayout.Space(10);

                    ShowLevelList();

                    GUILayout.Space(10);
                    break;

                case LevelEditorMode.EDIT_LEVEL_MODE:
                    levelEditPhase.Render();
                    break;
            }





//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Level Inventory", EditorStyles.boldLabel);
//            if (levelGeneratorSetup != null)
//            {
//                if (GUILayout.Button("Show Item List"))
//                {
//                    EditorUtility.FocusProjectWindow();
//                    Selection.activeObject = levelGeneratorSetup;
//                }
//            }
//            if (GUILayout.Button("Open Item List"))
//            {
//                OpenItemList();
//            }
//            if (GUILayout.Button("New Item List"))
//            {
//                EditorUtility.FocusProjectWindow();
//                Selection.activeObject = levelGeneratorSetup;
//            }
//            GUILayout.EndHorizontal();
//        
//            if (levelGeneratorSetup == null)
//            {
//                GUILayout.BeginHorizontal();
//                GUILayout.Space(10);
//                if (GUILayout.Button("Create New Item List", GUILayout.ExpandWidth(false)))
//                {
//                    CreateNewItemList();
//                }
//                if (GUILayout.Button("Open Existing Item List", GUILayout.ExpandWidth(false)))
//                {
//                    OpenItemList();
//                }
//                GUILayout.EndHorizontal();
//            }
//            
//            GUILayout.Space(20);
//            
//            if (levelGeneratorSetup != null)
//            {
//                GUILayout.BeginHorizontal();
//            
//                GUILayout.Space(10);
//            
//                if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
//                {
//                    if (viewIndex > 1)
//                        viewIndex--;
//                }
//                GUILayout.Space(5);
//                if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
//                {
//                    if (viewIndex < levelGeneratorSetup.levels.Count)
//                    {
//                        viewIndex++;
//                    }
//                }
//            
//                GUILayout.Space(60);
//            
//                if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false)))
//                {
//                    AddItem();
//                }
//                if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false)))
//                {
//                    DeleteItem(viewIndex - 1);
//                }
//            
//                GUILayout.EndHorizontal();
//                if (levelGeneratorSetup.levels == null)
//                    Debug.Log("wtf");
//                if (levelGeneratorSetup.levels.Count > 0)
//                {
//                    GUILayout.BeginHorizontal();
//                    viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, levelGeneratorSetup.levels.Count);
//                    //Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
//                    EditorGUILayout.LabelField("of   " + levelGeneratorSetup.levels.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
//                    GUILayout.EndHorizontal();
//                
//                    levelGeneratorSetup.levels[viewIndex - 1].levelId = EditorGUILayout.LongField("Item Id", levelGeneratorSetup.levels[viewIndex - 1].levelId);
//
//                    GUILayout.Space(10);
//                
//                    GUILayout.BeginHorizontal();
//                    levelGeneratorSetup.levels[viewIndex - 1].levelName = EditorGUILayout.TextField("Item Name", levelGeneratorSetup.levels[viewIndex - 1].levelName as string);
//                    levelGeneratorSetup.levels[viewIndex - 1].spawner = EditorGUILayout.ObjectField("Item Icon", levelGeneratorSetup.levels[viewIndex - 1].spawner, typeof(CreateFixedSpawnerSetup), false) as CreateFixedSpawnerSetup;
//                    GUILayout.EndHorizontal();
//                
//                    GUILayout.Space(10);
//            
//                }
//                else
//                {
//                    GUILayout.Label("This Inventory List is Empty.");
//                }
//            }
        }

        void ExtractCurrentValues()
        {
            levelGeneratorScriptFromScene = null;
            currentLevelDataGroup = null;
            if (Selection.activeGameObject != null)
            {
                levelGeneratorScriptFromScene = Selection.activeGameObject.GetComponent<LevelGenerator>();
                currentLevelDataGroup = levelGeneratorScriptFromScene != null ? levelGeneratorScriptFromScene.CurrentLevelGroupData : null;
            }
        }

        private void AddLevelGeneratorScriptToSelectedObject()
        {
            if (Selection.activeGameObject != null)
            {
                levelGeneratorScriptFromScene = Selection.activeGameObject.AddComponent<LevelGenerator>();
            }
        }

        void RendererHeader()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.enabled = levelGeneratorScriptFromScene != null;
            if (GUILayout.Button("Load Config File", GUILayout.Width(120)))
            {
                string path = EditorUtility.OpenFilePanel("Open LevelDataGroup File", "Assets/Data", "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    LevelGroupData levelDataGroup = AssetDatabase.LoadAssetAtPath<LevelGroupData>(FileUtil.GetProjectRelativePath(path));

                    if (levelDataGroup != null)
                    {
                        if (levelDataGroup != currentLevelDataGroup)
                        {
                            currentLevelDataGroup = levelDataGroup;
                            levelGeneratorScriptFromScene.SetLevelGroupData(currentLevelDataGroup, true);
                            EditorUtility.SetDirty(currentLevelDataGroup);
                        }
                        else
                        {
                            Debug.LogWarning(string.Format("Not asigning because they are equals {0} == {1}", levelDataGroup, currentLevelDataGroup));
                        }
                    }
                    else
                    {
                        Debug.LogWarning("levelDataGroup is NULL.");
                    }
                }
            }
            if (GUILayout.Button("Create new", GUILayout.Width(80)))
            {
                CreateNewLevelDataGroup();
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }

        void RenderSelectedObject()
        {
            // save original bg color
            Color bgOriginalColor = GUI.backgroundColor;

            // change color depending on selection
            GUI.backgroundColor = levelGeneratorScriptFromScene != null ? tableHeaderColor : redishColor;

            // begin Info Panel
            GUILayout.BeginVertical("box");
            // begin first line
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Object:", SelectedGameObjectName);
            GUILayout.EndHorizontal();
            // if there is an object selected...
            if (Selection.activeGameObject != null)
            {
                // if the selected object has a LevelGenerator script
                if (levelGeneratorScriptFromScene != null)
                {
                    // Show setup file name
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("File:", currentLevelDataGroup!=null? currentLevelDataGroup.name:"-");
                    GUILayout.EndHorizontal();
                }
                else
                {
                    Color aux = GUI.backgroundColor;
                    GUI.backgroundColor = bgOriginalColor;
                    if (GUILayout.Button("Set object as Level Generator", GUILayout.Width(180)))
                    {
                        AddLevelGeneratorScriptToSelectedObject();
                    }
                    GUI.backgroundColor = aux;
                }
            }
            GUILayout.EndHorizontal();
            if (levelGeneratorScriptFromScene == null)
            {
                ShowNotification(new GUIContent(maMessage));
            }
            GUILayout.EndVertical();
            GUI.backgroundColor = bgOriginalColor;
        }

        bool selectedAllToggle;
        bool[] selectedItems;
        void ShowLevelList()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Header List
            GUILayout.BeginHorizontal();
            int inventoryCount = 0;
            if (currentLevelDataGroup != null && currentLevelDataGroup.levels != null)
                inventoryCount = currentLevelDataGroup.levels.Count;
            GUILayout.Label(string.Format("{0} cool levels in you inventory.", inventoryCount));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Load Level", GUILayout.Width(80)))
            {
                string path = EditorUtility.OpenFilePanel("Open LevelSetup File", "Assets/", "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    LevelData ld = AssetDatabase.LoadAssetAtPath<LevelData>(FileUtil.GetProjectRelativePath(path));

                    if (ld != null)
                    {
                        if (currentLevelDataGroup.levels.Contains(ld))
                        {
                            Debug.Log(string.Format("Not adding duplicated value '{0}'.", ld.levelName));
                        }
                        else
                        {
                            currentLevelDataGroup.levels.Add(ld);
                        }
                    }
                }
            }
            if (GUILayout.Button("Create Level", GUILayout.Width(80)))
            {
                actualLevelData = null;
                levelEditPhase.Init(SaveLevelDataCallback, SaveLevelDataAndExitCallback, CancelCreateOrEditLevelCallback);
                mode = LevelEditorMode.EDIT_LEVEL_MODE;
            }
            if (GUILayout.Button("Generate LevelSetup from Scene", GUILayout.Width(200)))
            {
                GenerateLevelSetupFromScene();
            }
            GUILayout.EndHorizontal();

            if (inventoryCount > 0)
            {
                int selectBoxWidth = 20;
                int numberWidth = 50;
                int nameWidth = 150;

                List<LevelData> levels = currentLevelDataGroup.levels;
                if (selectedItems == null)
                {
                    selectedItems = new bool[levels.Count];
                }
                else if (selectedItems.Length != levels.Count)
                {
                    bool[] values = new bool[levels.Count];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = (i < selectedItems.Length) ? selectedItems[i] : false;
                    }
                    selectedItems = values;
                }

//                testColor = EditorGUILayout.ColorField(testColor, GUILayout.Width(100));
//                Debug.Log("testColor:"+testColor);

                Color previousColor = GUI.backgroundColor;
                GUI.backgroundColor = tableHeaderColor;
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();

                selectedAllToggle = EditorGUILayout.Toggle(selectedAllToggle, GUILayout.Width(selectBoxWidth));
                GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(numberWidth));
                GUILayout.Label("NAME", EditorStyles.boldLabel, GUILayout.Width(nameWidth));
                GUILayout.Label("DEBRIS", EditorStyles.boldLabel, GUILayout.Width(numberWidth));

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUI.backgroundColor = previousColor;

                // Change toggle values
                if (selectedAllToggle != previousSelectedAllToggleValue)
                {
                    for (int i = 0; i < selectedItems.Length; i++)
                    {
                        selectedItems[i] = selectedAllToggle;
                    }

                    previousSelectedAllToggleValue = selectedAllToggle;
                }

                GUILayout.BeginVertical();
                for (int i = 0; i < levels.Count; i++)
                {
                    if (levels[i] == null)
                    {
                        Debug.LogWarning(string.Format("Level at position {0} is NULL.", i));
                        continue;
                    }
                    GUI.backgroundColor = i % 2 == 0? tableItemColor1: tableItemColor2;
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    selectedItems[i] = EditorGUILayout.Toggle(selectedItems[i], GUILayout.Width(selectBoxWidth));
                    GUILayout.Label(levels[i].levelId.ToString(), GUILayout.Width(numberWidth));
                    GUILayout.Label(levels[i].levelName, GUILayout.Width(nameWidth));
                    GUILayout.Label(levels[i].objects.Count.ToString(), GUILayout.Width(numberWidth));
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUI.backgroundColor = previousColor;

                // count selected
                int selectedIndex = -1;
                int selectedCount = 0;
                for (int i = 0; i < selectedItems.Length; i++)
                {
                    if (selectedItems[i])
                    {
                        selectedIndex = i;
                        selectedCount++;
                    }
                }

                if (selectedCount != selectedItems.Length)
                {
                    selectedAllToggle = false;
                    previousSelectedAllToggleValue = false;
                }

                // Level list bottom menu
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Format("{0} levels selected to :", selectedCount));
                GUI.enabled = selectedCount == 1;
                if (GUILayout.Button("Edit", GUILayout.ExpandWidth(false)))
                {
                    // set level data to a transient form
                    levelEditPhase.Init(SaveLevelDataCallback, SaveLevelDataAndExitCallback, CancelCreateOrEditLevelCallback, levels[selectedIndex]);
                    actualLevelData = levels[selectedIndex];
                    mode = LevelEditorMode.EDIT_LEVEL_MODE;
                }
                if (GUILayout.Button("Load", GUILayout.ExpandWidth(false)))
                {
                    // TODO: load
                }
                GUI.enabled = selectedCount > 0;
                if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false)))
                {
                    // TODO: confirmation window

                    // remove items
                    DeleteSelectedItems();
                }


                GUI.enabled = true;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

            }

            GUILayout.EndVertical();
        }

        void SaveLevelDataCallback(LevelData levelData, System.Action<bool> callback = null)
        {
            if (levelData != null)
            {
                if (actualLevelData != null)
                {
                    // copy content from levelData to editabletLevelData
                    actualLevelData.Clone(levelData);

                    // set dirty to be saved
                    EditorUtility.SetDirty(actualLevelData);
                }
                else
                {
                    // get a new file path
                    string path = EditorUtility.SaveFilePanel(
                        "Create LevelSetup",
                        "Assets/",
                        "NewLevelSetup.asset",
                        "asset");
                    
                    if (!string.IsNullOrEmpty(path))
                    {
                        // chage it to relative path
                        path = FileUtil.GetProjectRelativePath(path);

                        // look if this asset alrady exist
                        actualLevelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                        if (actualLevelData != null)
                        {
                            actualLevelData.Clone(levelData);

                            if (!currentLevelDataGroup.levels.Contains(actualLevelData))
                            {
                                currentLevelDataGroup.levels.Add(actualLevelData);
                            }

                            EditorUtility.SetDirty(actualLevelData);
                            EditorUtility.SetDirty(currentLevelDataGroup);
                        }
                        else
                        {
                            actualLevelData = levelData;
                            AssetDatabase.CreateAsset(actualLevelData, path);

                            currentLevelDataGroup.levels.Add(actualLevelData);

                            EditorUtility.SetDirty(actualLevelData);
                            EditorUtility.SetDirty(currentLevelDataGroup);
                        }
                    }
                    else
                    {
                        if (callback != null)
                            callback(false);

                        return;
                    }
                }

                // save changes
                AssetDatabase.SaveAssets();

                // apply changes to the scene


                if (callback != null)
                    callback(true);
            }
        }

        /*
         * Handles the creation or edition of a level
         * */
        void SaveLevelDataAndExitCallback(LevelData levelData, System.Action<bool> callback = null)
        {
            SaveLevelDataCallback(levelData, (bool shouldChangeMode) => {
                // call back to reset or not the ui
                if (callback != null)
                    callback(shouldChangeMode);

                // change mode if exit is successful
                if (shouldChangeMode)
                    mode = LevelEditorMode.LIST_LEVEL_MODE;
            });
        }

        void CancelCreateOrEditLevelCallback()
        {
            mode = LevelEditorMode.LIST_LEVEL_MODE;
        }

        void LoadLevelDataToScene (LevelData ld)
        {
            if (levelGeneratorScriptFromScene == null)
                return;


        }

        void CleanSelectedItemsFlag()
        {
            // clean selected items
            for (int i = 0; i < selectedItems.Length; i++)
            {
                selectedItems[i] = false;
            }
        }

        void CreateNewLevelDataGroup ()
        {
            string path = null;
            try
            {
                path = EditorUtility.SaveFilePanel(
                    "Crate LevelGroupData",
                    "Assets/Data/",
                    "NewLevelGroupData.asset",
                    "asset");

                if (string.IsNullOrEmpty(path))
                    return;
                
                currentLevelDataGroup = LevelGeneratorSetupFactory.Create(path);
                levelGeneratorScriptFromScene.SetLevelGroupData(currentLevelDataGroup, true);

                EditorUtility.SetDirty(currentLevelDataGroup);
            }
            catch (Exception e)
            {
                Debug.Log("File not created. " + e.Message);
            }
        }

        LevelData CreateAndSaveNewLevelSetupFile ()
        {
            // ask the user for the path
            string path = EditorUtility.SaveFilePanel(
                "Create LevelSetup",
                "Assets/",
                "NewLevelSetup.asset",
                "asset");

            // check selected path
            if (string.IsNullOrEmpty(path))
                return null;

            // return new asset file at that path
            return LevelSetupFactory.Create(path);
        }

        void GenerateLevelSetupFromScene()
        {
            if (currentLevelDataGroup == null)
            {
                CreateNewLevelDataGroup();
            }

            if (levelGeneratorScriptFromScene != null)
            {
                LevelData ld = CreateAndSaveNewLevelSetupFile();
                if (ld != null)
                {
                    Transform[] children = levelGeneratorScriptFromScene.gameObject.GetComponentsInChildren<Transform>();
//                    Debug.Log("children ==> " + children.Length);

                    if (children != null)
                    {
                        for (int i = 0; i < children.Length; i++)
                        {
                            Debris debris = children[i].gameObject.GetComponent<Debris>();
                            if (debris == null)
                                continue;

                            ld.objects.Add(CreateDebrisData(debris));
                        }
                    }

                    // add levelData if not in yet
                    if (!currentLevelDataGroup.levels.Contains(ld))
                        currentLevelDataGroup.levels.Add(ld);

                    // everybody is dirty
                    EditorUtility.SetDirty(ld);
                    EditorUtility.SetDirty(currentLevelDataGroup);
                }
            }
        }

        DebrisData CreateDebrisData(Debris debris)
        {
            DebrisData debrisData = new DebrisData();

            debrisData.name = debris.transform.name;
            debrisData.position = debris.transform.position;
            UnityEngine.Object parent = PrefabUtility.GetPrefabParent(debris.gameObject);
            debrisData.prefab = parent != null? ((GameObject)parent).transform : null;
            debrisData.scale = debris.transform.localScale;
            debrisData.density = debris.Density;

            return debrisData;
        }

        void DeleteSelectedItems()
        {
            // create a list of all alements to delete
            List<LevelData> levelsToDelete = new List<LevelData>();
            for (int i = 0; i < selectedItems.Length; i++)
            {
                if (selectedItems[i])
                    levelsToDelete.Add(currentLevelDataGroup.levels[i]);
            }

            // delete all selected items
            for (int i = 0; i < levelsToDelete.Count; i++)
            {
                currentLevelDataGroup.levels.Remove(levelsToDelete[i]);
            }

            CleanSelectedItemsFlag();
            EditorUtility.SetDirty(currentLevelDataGroup);

            AssetDatabase.SaveAssets();
        }
    }
}