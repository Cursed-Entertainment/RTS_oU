using UnityEngine;

[DisallowMultipleComponent]
public class RTS_TeamColors : MonoBehaviour
{
    public Color PlayerColor = Color.blue;
    public Color EnemyColor = Color.red;
    [HideInInspector]
    public Color TempColor = Color.white;

    Texture PlayerColorTexture;
    Texture EnemyColorTexture;

    void Start()
    {
        PlayerColorTexture = ColorToTexture(PlayerColor);
        EnemyColorTexture = ColorToTexture(EnemyColor);

        RTS_Materials Materials = GetComponent<RTS_Materials>();
        Materials.PlayerMaterial.SetColor("_Color", PlayerColor);
        Materials.AIMaterial.SetColor("_Color", EnemyColor);
    }

    public Texture2D ColorToTexture(Color color)
    {
        Texture2D texture = new Texture2D(2, 2);
        Color[] colors = new Color[4];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }

        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }
}
