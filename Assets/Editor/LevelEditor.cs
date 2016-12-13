using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using javitechnologies.levelgenerator.data;
using javitechnologies.ballwar.model;
using javitechnologies.ballwar.levelgenerator.spawner;
using javitechnologies.ballwar.levelgenerator.view;

namespace javitechnologies.levelgenerator.editor
{
    public class LevelEditor : EditorWindow
    {
        enum LevelEditorMode
        {
            LIST_LEVEL_MODE,
            EDIT_LEVEL_MODE
        }

        [Serializable]
        public struct PrefabData
        {
            public string name;
            public Transform prefab;

            public PrefabData(string name, Transform prefab)
            {
                this.name = name;
                this.prefab = prefab;
            }
        }

        private const string dataDirectory = "Assets/Data/";

        public static string DataDirectory
        {
            get
            {
                return dataDirectory;
            }
        }

        /**
         * The current level generator data scriptable object
         */
        private LevelGeneratorData currentLevelGeneratorSetup;
        private LevelData editingLevelSetup = null;

        private LevelEditorMode mode = LevelEditorMode.LIST_LEVEL_MODE;

//        private int viewIndex = 1;
//        public List<PrefabData> prefabs = new List<PrefabData>();

        [MenuItem("Tools/Custom/Level Editor")]
        static void Init()
        {
            EditorWindow.GetWindow<LevelEditor>("Level Editor");
        }

        void  OnEnable()
        {
            
            if (EditorPrefs.HasKey("ObjectPath"))
            {
                string objectPath = EditorPrefs.GetString("ObjectPath");
                currentLevelGeneratorSetup = AssetDatabase.LoadAssetAtPath(objectPath, typeof(LevelGeneratorData)) as LevelGeneratorData;
            }
        }

        void ShowLevelList()
        {
            if (currentLevelGeneratorSetup != null && currentLevelGeneratorSetup.levels.Count > 0)
            {
                int numberWidth = 50;
                int nameWidth = 150;

                List<LevelData> levels = currentLevelGeneratorSetup.levels;

//                int currentHeight = levels.Count * 20;

                GUILayout.Label(string.Format("{0} awesome levels in you inventory.", levels.Count));

                GUILayout.BeginHorizontal("box");
                GUILayout.Label("Id", GUILayout.Width(numberWidth));
                GUILayout.Label("Name", GUILayout.Width(nameWidth));
                GUILayout.Label("Spawners", GUILayout.Width(numberWidth));
                GUILayout.Label("Options", GUILayout.Width(nameWidth));
                GUILayout.EndHorizontal();

                for (int i = 0; i < levels.Count; i++)
                {
                    GUILayout.BeginHorizontal("box");

                    GUILayout.Label(levels[i].levelId.ToString(), GUILayout.Width(numberWidth));
                    GUILayout.Label(levels[i].levelName, GUILayout.Width(nameWidth));
                    GUILayout.Label(levels[i].spawners.Count.ToString(), GUILayout.Width(numberWidth));
                    GUILayout.BeginHorizontal(GUILayout.Width(numberWidth));
//                    GUILayout.Space(100);
                    if (GUILayout.Button("Edit", GUILayout.ExpandWidth(false)))
                    {
                        editingLevelSetup = levels[i];
                        mode = LevelEditorMode.EDIT_LEVEL_MODE;
                    }
                    if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    {
                        DeleteItem(i);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                GUILayout.Label("No levels to show");
            }
        }

        private Vector2 spawnerListScrollPosition = Vector2.zero;
        private SpawnerType editingSpawnerType = SpawnerType.NONE;


        AbstractSpawnerSetup editingSpawnerSetup = null;

        Transform editingSpawnerPrefab = null;

        private void AutoSave()
        {
            currentLevelGeneratorSetup = null;
        }

        private void NoValidSelectionDisplay()
        {
            GUILayout.Label("Select a LevelGeneratorGameObject", EditorStyles.boldLabel);
            AutoSave();
            mode = LevelEditorMode.LIST_LEVEL_MODE;
        }

        void  OnGUI()
        {
            if (Selection.activeGameObject == null)
            {
                NoValidSelectionDisplay();
                return;
            }

            LevelGenerator levelGeneratorScript = Selection.activeGameObject.GetComponent<LevelGenerator>();
            if (levelGeneratorScript == null)
            {
                NoValidSelectionDisplay();
                return;
            }

            currentLevelGeneratorSetup = levelGeneratorScript.levelGeneratorSetup;
            if (currentLevelGeneratorSetup == null)
            {
                if (GUILayout.Button("Load data from scene", GUILayout.Width(200)))
                {
                    LoadLevelGeneratorSetupFromScene();
                    levelGeneratorScript.levelGeneratorSetup = currentLevelGeneratorSetup;
                }
                mode = LevelEditorMode.LIST_LEVEL_MODE;
                return;
            }

            if (currentLevelGeneratorSetup == null)
            {
                Debug.LogError("WTF!");
                return;
            }

//            Debug.Log("LevelEditor.OnGUI");

            EditorGUIUtility.labelWidth = 60f;

//            GUILayout.Label("Level Inventory", EditorStyles.boldLabel);
            GUILayout.Space(10);

            switch (mode)
            {
                case LevelEditorMode.LIST_LEVEL_MODE:
                    ShowLevelList();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add Level", GUILayout.Width(80)))
                    {
                        editingLevelSetup = null;
                        mode = LevelEditorMode.EDIT_LEVEL_MODE;
                        return;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);
                    break;
                case LevelEditorMode.EDIT_LEVEL_MODE:
                    if (editingLevelSetup == null)
                    {
                        GUILayout.Label("Creating new Level...");
                        editingLevelSetup = LevelSetupFactory.Create();
                    }
                    else
                    {
                        GUILayout.Label(string.Format("Editing {0}", editingLevelSetup.levelName));
                    }
                    
                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginHorizontal("box");
                    editingLevelSetup.levelId = EditorGUILayout.LongField("Id", editingLevelSetup.levelId, GUILayout.Width(250));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal("box");
                    editingLevelSetup.levelName = EditorGUILayout.TextField("Name", editingLevelSetup.levelName as string);
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();

//                    // test
////                    GUILayout.Label("TEST", GUILayout.Width(100));
////                    EditorGUI.PropertyField(new Rect(500, 500, 300, 300), editingLevelSetup.spawners, false);
////                    GUILayout.Space(30);
//                    // end


                    GUILayout.Space(10);
                    GUILayout.Label("Spawners: ", GUILayout.Width(100));
                    
                    // show spawners
                    spawnerListScrollPosition = GUILayout.BeginScrollView(spawnerListScrollPosition, GUIStyle.none);
                    for (int i = 0; i < editingLevelSetup.spawners.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginHorizontal("box");
                        GUILayout.Label(editingLevelSetup.spawners[i].type.ToString(), GUILayout.Width(70));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal("box");
                        editingLevelSetup.spawners[i].prefab = EditorGUILayout.ObjectField(editingLevelSetup.spawners[i].prefab, typeof(Transform), false, GUILayout.Width(200)) as Transform;
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal("box");
                        if(GUILayout.Button("Edit", GUILayout.Width(50)))
                        {
                            editingSpawnerSetup = editingLevelSetup.spawners[i];
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndScrollView();

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginHorizontal("box", GUILayout.Width(300));
                    GUILayout.Label("Type: ", GUILayout.Width(100));
                    editingSpawnerType = (SpawnerType)EditorGUILayout.EnumPopup(editingSpawnerType, GUILayout.Width(100));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal("box", GUILayout.Width(300));
                    editingSpawnerPrefab = EditorGUILayout.ObjectField("Prefab", editingSpawnerPrefab, typeof(Transform), false) as Transform;
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();

                    switch (editingSpawnerType)
                    {
                        case SpawnerType.FIXED:
                            FixedSpawnerSetup fixedSpawner = null;
                            if (editingSpawnerSetup != null && editingSpawnerSetup.type == SpawnerType.FIXED)
                            {
                                fixedSpawner = (FixedSpawnerSetup)editingSpawnerSetup;
                            }
                            else
                            {
                                fixedSpawner = SpawnerFactory.CreateFixedSpawner();
                            }

                            GUILayout.BeginHorizontal();
                            GUILayout.BeginHorizontal("box", GUILayout.Width(300));
                            fixedSpawner.placementPosition = EditorGUILayout.Vector3Field("Position", fixedSpawner.placementPosition);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal("box", GUILayout.Width(300));
                            fixedSpawner.localScale = EditorGUILayout.Vector3Field("Scale", fixedSpawner.localScale);
                            GUILayout.EndHorizontal();
                            GUILayout.EndHorizontal();
                            break;
                        case SpawnerType.RANDOM:
                            RandomSpawnerSetup randomSpawner = null;
                            if (editingSpawnerSetup != null && editingSpawnerSetup.type == SpawnerType.RANDOM)
                            {
                                randomSpawner = (RandomSpawnerSetup)editingSpawnerSetup;
                            }
                            else
                            {
                                randomSpawner = SpawnerFactory.CreateRandomSpawner();
                            }

                            GUILayout.BeginHorizontal();
                            GUILayout.BeginHorizontal("box", GUILayout.Width(300));
                            randomSpawner.quantity = EditorGUILayout.IntField("Quantity", randomSpawner.quantity);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal("box", GUILayout.Width(300));
                            randomSpawner.range = (Rect)EditorGUILayout.RectField("Range", randomSpawner.range);
                    
                            GUILayout.EndHorizontal();
                            GUILayout.EndHorizontal();

                            break;
                    }

                    if (GUILayout.Button("Add spawner"))
                    {
                        
                    }



                    GUILayout.Space(20);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUI.backgroundColor = Color.gray;
                    if (GUILayout.Button("Cancel", GUILayout.Width(80)))
                    {
                        mode = LevelEditorMode.LIST_LEVEL_MODE;
                    }
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Save", GUILayout.Width(80)))
                    {
//                        levelGeneratorSetup.levels.Add(selectedLevelSetup);
                        mode = LevelEditorMode.LIST_LEVEL_MODE;
                    }
                    GUILayout.EndHorizontal();

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
//            if (GUI.changed && levelGeneratorSetup != null)
//            {
//                EditorUtility.SetDirty(levelGeneratorSetup);
//            }
        }

        void LoadLevelGeneratorSetupFromScene()
        {
            Debug.Log("LoadLevelGeneratorSetupFromScene");
            currentLevelGeneratorSetup = LevelGeneratorSetupFactory.Create();

            if (currentLevelGeneratorSetup != null)
            {
                LevelData levelSetup = GenerateSetupFromScene();

                currentLevelGeneratorSetup.levels.Add(levelSetup);

                string relPath = AssetDatabase.GetAssetPath(currentLevelGeneratorSetup);
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }

        LevelData GenerateSetupFromScene()
        {
            LevelData levelSetup = LevelSetupFactory.Create();

            Transform[] children = Selection.activeGameObject.GetComponentsInChildren<Transform>();
//            Debug.Log("child ==> " + children.Length);
            for (int i = 0; i < children.Length; i++)
            {
                Debris debris = children[i].gameObject.GetComponent<Debris>();
                if (debris == null)
                    continue;
                
                levelSetup.spawners.Add(CreateFixedSpawner(children[i]));
            }

            AssetDatabase.SaveAssets();

            return levelSetup;
        }

        FixedSpawnerSetup CreateFixedSpawner(Transform obj)
        {
            FixedSpawnerSetup fixedSpawner = new FixedSpawnerSetup();

            fixedSpawner.type = SpawnerType.FIXED;
            fixedSpawner.localScale = obj.localScale;
            fixedSpawner.placementPosition = obj.position;
            fixedSpawner.prefab = ((GameObject)PrefabUtility.GetPrefabParent(obj.gameObject)).transform;

            return fixedSpawner;
        }

        void OpenItemList()
        {
            string absPath = EditorUtility.OpenFilePanel("Select Inventory Item List", "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                currentLevelGeneratorSetup = AssetDatabase.LoadAssetAtPath(relPath, typeof(LevelGeneratorData)) as LevelGeneratorData;
                if (currentLevelGeneratorSetup.levels == null)
                    currentLevelGeneratorSetup.levels = new List<LevelData>();
                if (currentLevelGeneratorSetup)
                {
                    EditorPrefs.SetString("ObjectPath", relPath);
                }
            }
        }

        void AddItem()
        {
            LevelData newItem = LevelSetupFactory.Create();
            newItem.levelName = "New Item";
            currentLevelGeneratorSetup.levels.Add(newItem);
//            viewIndex = currentLevelGeneratorSetup.levels.Count;
        }

        void DeleteItem(int index)
        {
            currentLevelGeneratorSetup.levels.RemoveAt(index);
        }
    }
}