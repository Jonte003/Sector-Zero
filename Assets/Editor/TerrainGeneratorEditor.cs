using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainGenerator generator = (TerrainGenerator)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Map Generation Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate New Map", GUILayout.Height(30)))
        {
            generator.GenerateMap();
        }

        EditorGUILayout.Space(2);

        GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
        if (GUILayout.Button("Clear Map", GUILayout.Height(30)))
        {
            generator.ClearMap();
        }
        GUI.backgroundColor = Color.white;
    }
}