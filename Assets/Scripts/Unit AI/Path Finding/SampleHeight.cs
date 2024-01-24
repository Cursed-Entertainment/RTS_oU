using UnityEngine;

public class SampleHeight : MonoBehaviour
{
    void Update()
    {
        Ray ray;
        RaycastHit hit;
        ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 temp = hit.point;
            temp.y = 0;
            Debug.Log(grid.mGrid.returnClosestTile(hit.point).status);
        }
    }
}
