using UnityEngine;

[DisallowMultipleComponent]
public class Radar_Controller : MonoBehaviour
{
    public GameObject Dish;
    private Material[] DishMaterial;

    void Start()
    {
        DishMaterial = Dish.GetComponent<Renderer>().materials;

        foreach (Material m in DishMaterial)
        {
            m.shader = Shader.Find("Diffuse");
        }
    }

    void OnDestroy()
    {
        foreach (Material m in DishMaterial)
        {
            Destroy(m);
        }
    }
}
