using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(GridEditor))]
public class grid_editEditor : Editor
{
    private grid Grid;

    SerializedProperty width, length, gridSize, waterLevel, terrainSlopeAngle, xOffset, zOffset, blockIndent;

    public void OnEnable()
    {
        width = serializedObject.FindProperty("width");
        length = serializedObject.FindProperty("length");
        gridSize = serializedObject.FindProperty("gridSize");
        waterLevel = serializedObject.FindProperty("waterLevel");
        terrainSlopeAngle = serializedObject.FindProperty("terrainSlopeAngle");
        xOffset = serializedObject.FindProperty("xOffset");
        zOffset = serializedObject.FindProperty("zOffset");
        blockIndent = serializedObject.FindProperty("blockIndent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        if (GUILayout.Button("Update"))
        {
            Grid = GameObject.FindGameObjectWithTag("manager").GetComponent<grid>();

            width.intValue = Grid.width;
            length.intValue = Grid.length;
            gridSize.intValue = Grid.gridSize;
            waterLevel.floatValue = Grid.waterLevel;
            terrainSlopeAngle.floatValue = Grid.terrainSlopeAngle;
            xOffset.floatValue = Grid.xOffset;
            zOffset.floatValue = Grid.zOffset;
            blockIndent.intValue = Grid.blockIndent;

        }

        serializedObject.ApplyModifiedProperties();
    }
}
