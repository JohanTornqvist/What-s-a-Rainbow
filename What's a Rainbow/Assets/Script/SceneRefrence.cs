using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SceneReference
{
    [SerializeField] private SceneAsset sceneAsset;
    [SerializeField] private string scenePath;

    public string ScenePath => scenePath;
    public string SceneName => System.IO.Path.GetFileNameWithoutExtension(scenePath);

    // Ensure path is updated when the asset is assigned
#if UNITY_EDITOR
    public void UpdateScenePath()
    {
        if (sceneAsset != null)
        {
            scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        }
    }
#endif
}

// Custom Inspector to auto-update scene path
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty sceneAssetProp = property.FindPropertyRelative("sceneAsset");
        SerializedProperty scenePathProp = property.FindPropertyRelative("scenePath");

        EditorGUI.BeginChangeCheck();
        sceneAssetProp.objectReferenceValue = EditorGUI.ObjectField(position, label, sceneAssetProp.objectReferenceValue, typeof(SceneAsset), false);
        if (EditorGUI.EndChangeCheck())
        {
            SceneReference sceneRef = fieldInfo.GetValue(property.serializedObject.targetObject) as SceneReference;
            sceneRef?.UpdateScenePath();
            scenePathProp.stringValue = sceneRef?.ScenePath;
        }

        EditorGUI.EndProperty();
    }
}
#endif
