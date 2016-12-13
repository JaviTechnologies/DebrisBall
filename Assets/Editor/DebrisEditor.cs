using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(SphereDebris))]
public class DebrisEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		SphereDebris debris = (SphereDebris)target;
		EditorGUILayout.LabelField ("Volume", debris.Volume.ToString ());
		EditorGUILayout.LabelField ("Mass", debris.Mass.ToString ());
	}
}
