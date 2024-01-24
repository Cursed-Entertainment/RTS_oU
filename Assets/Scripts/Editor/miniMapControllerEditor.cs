using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MiniMap_Controller))]
public class miniMapControllerEditor : Editor
{
    Vector2 temp;

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Test"))
        {
            Camera miniMapCamera = GameObject.FindGameObjectWithTag("mmCamera").GetComponent<Camera>();
            temp = GetMainGameViewSize();

            miniMapCamera.transform.position = new Vector3(Terrain.activeTerrain.terrainData.size.x / 2, 90, Terrain.activeTerrain.terrainData.size.z / 2);

            float aspectRatio = temp.x / temp.y;

            float viewPortY = 3.0f / 4.0f;
            float viewPortHeight = 1.0f / 4.5f;

            float viewPortWidth = 1.0f / (4.5f * aspectRatio);
            float viewPortX = 1 - (0.25f / aspectRatio);

            miniMapCamera.rect = new Rect(viewPortX, viewPortY, viewPortWidth, viewPortHeight);

            Debug.Log("Width: " + Screen.width + " Height: " + Screen.height);
        }
    }

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }
}
