using UnityEngine;

public class TechCentre : MonoBehaviour
{
    void Start()
    {
        Material[] m = GetComponent<Building_Controller>().materials;
        foreach (Material mat in m)
        {
            if (mat.name == "glass")
            {
                mat.shader = Shader.Find("Specular");
                mat.SetFloat("Shininess", 0.1f);
            }
        }
    }
}
