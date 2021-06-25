using VPG.UX;
using UnityEditor;
using UnityEngine;

namespace VPG.Editor.UX
{
    /// <summary>
    /// Custom editor for <see cref="ICourseController"/>s.
    /// Takes care of adding required components.
    /// </summary>
    [CustomEditor(typeof(CourseMenuSpawner))]
    public class CourseMenuSpawnerEditor : UnityEditor.Editor
    {
        private SerializedProperty useCustomPrefabProperty;
        private SerializedProperty customPrefabProperty;
        private SerializedProperty defaultPrefabProperty;

        private GameObject defaultPrefab;
        private GameObject customPrefab;
        private bool useCustomPrefab;

        private void OnEnable()
        {
            defaultPrefabProperty = serializedObject.FindProperty("defaultPrefab");
            useCustomPrefabProperty = serializedObject.FindProperty("useCustomPrefab");
            customPrefabProperty = serializedObject.FindProperty("customPrefab");
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((CourseMenuSpawner)target), typeof(CourseMenuSpawner), false);
            EditorGUILayout.ObjectField(new GUIContent("Default prefab", "Default menu prefab. If you want to change the menu that is spawned, set a custom prefab instead."), defaultPrefabProperty.objectReferenceValue, typeof(GameObject), false);

            GUI.enabled = useCustomPrefab == false && Application.isPlaying == false;

            GUI.enabled = !Application.isPlaying;

            useCustomPrefab = EditorGUILayout.Toggle(new GUIContent("Use custom prefab", "Use a custom menu prefab instead of default"), useCustomPrefabProperty.boolValue);

            if (useCustomPrefab)
            {
                customPrefab = EditorGUILayout.ObjectField(new GUIContent("Custom prefab", "Custom menu prefab. If you leave this empty no menu will be spawned."), customPrefabProperty.objectReferenceValue, typeof(GameObject), false) as GameObject;
                customPrefabProperty.objectReferenceValue = customPrefab;
            }

            useCustomPrefabProperty.boolValue = useCustomPrefab;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
