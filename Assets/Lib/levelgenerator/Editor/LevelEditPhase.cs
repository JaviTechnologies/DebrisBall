using UnityEngine;
using System.Collections;
using javitechnologies.ballwar.model;
using System.Collections.Generic;
using UnityEditor;

namespace javitechnologies.levelgenerator.editor
{
    public class LevelEditPhase
    {
        LevelData editableLevelData;
        bool creatingNewLevel;
        bool showSpawnersBoxEditMode = false;
        bool showDebrisBoxEditMode = false;
        private Vector2 spawnerListScrollPosition = Vector2.zero;

        private bool initialized = false;

        private System.Action OnCancelAction;
        private System.Action<LevelData> OnSaveAction;

        public void Init (System.Action<LevelData> saveActionCb, System.Action cancelActionCb, LevelData levelData = null)
        {
            // Set all to default values
            Reset();

            // creating flag
            creatingNewLevel = levelData == null;

            // copy values from levelData if this is an edit
            if (!creatingNewLevel)
                editableLevelData.Clone(levelData);

            // keep callbacks for this setup
            OnSaveAction = saveActionCb;
            OnCancelAction = cancelActionCb;

            // lose focus from previus values
            GUI.FocusControl("");

            // it's ready
            initialized = true;
        }

        private void Reset()
        {
            // desinitialize
            initialized = false;

            // check editable object
            if (editableLevelData == null)
            {
                // Create a ScriptableObject to transiently edit here (not stored)
                editableLevelData = ScriptableObject.CreateInstance<LevelData>();
                editableLevelData.objects = new List<DebrisData>();
            }

            // default values
            editableLevelData.levelId = -1;
            editableLevelData.levelName = "";
            editableLevelData.objects.Clear();
        }

        public void Render ()
        {
            if (!initialized)
                return;
            
            if (creatingNewLevel)
            {
                GUILayout.Label("Creating Level.");
            }
            else
            {
                GUILayout.Label(string.Format("Editing Level: '{0}'.", editableLevelData.levelName));
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal("box");
            editableLevelData.levelId = EditorGUILayout.LongField("Id", editableLevelData.levelId, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            editableLevelData.levelName = EditorGUILayout.TextField("Name", editableLevelData.levelName as string);
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            showSpawnersBoxEditMode = EditorGUILayout.Foldout(showSpawnersBoxEditMode, string.Format("Spawners: {0}", 0));
            if (showSpawnersBoxEditMode)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Label("Whaaaaaaat?!: ", GUILayout.Width(100));
                GUILayout.EndVertical();
            }

            GUILayout.Space(10);

            showDebrisBoxEditMode = EditorGUILayout.Foldout(showDebrisBoxEditMode, string.Format("Debris: {0}", editableLevelData.objects.Count));
            if (showDebrisBoxEditMode)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                if (editableLevelData.objects.Count > 0)
                {
                    Color aux = GUI.backgroundColor;
                    GUI.backgroundColor = Color.yellow;
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label("Name", GUILayout.Width(100));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label("Position", GUILayout.Width(100));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label("Scale", GUILayout.Width(100));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label("Density", GUILayout.Width(60));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label("Prefab", GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();

                    GUI.backgroundColor = aux;
                    // show debris
                    spawnerListScrollPosition = GUILayout.BeginScrollView(spawnerListScrollPosition, GUIStyle.none);
                    for (int i = 0; i < editableLevelData.objects.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginHorizontal("box");
                        editableLevelData.objects[i].name = EditorGUILayout.TextField(editableLevelData.objects[i].name, GUILayout.Width(100));
                        //                            GUILayout.Label(editingLevelSetup.objects[i].name, GUILayout.Width(100));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal("box");
                        GUILayout.Label(editableLevelData.objects[i].position.ToString(), GUILayout.Width(100));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal("box");
                        GUILayout.Label(editableLevelData.objects[i].scale.ToString(), GUILayout.Width(100));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal("box");
                        GUILayout.Label(editableLevelData.objects[i].density.ToString(), GUILayout.Width(60));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal("box");
                        editableLevelData.objects[i].prefab = EditorGUILayout.ObjectField(editableLevelData.objects[i].prefab, typeof(Transform), false, GUILayout.Width(200)) as Transform;
                        GUILayout.EndHorizontal();
                        //                    GUILayout.BeginHorizontal("box");
                        //                    if(GUILayout.Button("Edit", GUILayout.Width(50)))
                        //                    {
                        //                        editingSpawnerSetup = editingLevelSetup.objects[i];
                        //                    }
                        //                    GUILayout.EndHorizontal();
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndScrollView();
                }

                GUILayout.EndVertical();
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("Cancel", GUILayout.Width(80)))
            {
                if (OnCancelAction != null)
                    OnCancelAction();

                Reset();
            }
            GUI.backgroundColor = Color.green;
            GUI.enabled = IsValidData();
            if (GUILayout.Button("Save", GUILayout.Width(80)))
            {
                if (GUI.changed && editableLevelData != null)
                {
                    if (OnSaveAction != null)
                        OnSaveAction(editableLevelData);
                }

                Reset();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }

        private bool IsValidData()
        {
            // validate object
            if (editableLevelData == null)
                return false;

            // check level id
            if (editableLevelData.levelId < 1)
                return false;

            // check name
            if (string.IsNullOrEmpty(editableLevelData.levelName))
                return false;

            // check objects
            if (editableLevelData.objects == null)
                return false;

            return true;
        }
    }
}