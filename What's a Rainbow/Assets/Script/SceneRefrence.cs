using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SceneReference
{
    [SerializeField, Tooltip("The scene asset to reference.")]
    private SceneAsset sceneAsset;

    [SerializeField, Tooltip("The path to the scene asset.")]
    private string scenePath;

    public string ScenePath => scenePath;
    public string SceneName => System.IO.Path.GetFileNameWithoutExtension(scenePath);

    // Ensure path is updated when the asset is assigned
#if UNITY_EDITOR
    public void UpdateScenePath()
    {
        if (sceneAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(sceneAsset);
            if (!string.IsNullOrEmpty(path) && path.EndsWith(".unity"))
            {
                scenePath = path;
            }
            else
            {
                Debug.LogWarning("The selected asset is not a valid scene file.");
            }
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

        // Draw scene asset field with a tooltip
        EditorGUI.BeginChangeCheck();
        sceneAssetProp.objectReferenceValue = EditorGUI.ObjectField(
            position,
            new GUIContent(label.text, "Select a scene asset to reference"),
            sceneAssetProp.objectReferenceValue,
            typeof(SceneAsset),
            false
        );

        if (EditorGUI.EndChangeCheck())
        {
            SceneReference sceneRef = fieldInfo.GetValue(property.serializedObject.targetObject) as SceneReference;
            sceneRef?.UpdateScenePath();
            scenePathProp.stringValue = sceneRef?.ScenePath;
        }

        // Optionally, display the path under the asset field for better context
        if (!string.IsNullOrEmpty(scenePathProp.stringValue))
        {
            EditorGUI.LabelField(position, "Scene Path:", scenePathProp.stringValue);
        }

        EditorGUI.EndProperty();
    }
}
#endif
