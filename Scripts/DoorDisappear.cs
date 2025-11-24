using UnityEngine;

public class DoorDisappear : MonoBehaviour
{
    [Header("Door Parts to Hide")]
    public GameObject[] doorParts;

    [Header("Hide Options")]
    public bool disableRenderers = true;
    public bool disableColliders = true;
    public bool deactivateGameObjects = false;

    public void HideDoor()
    {
        foreach (GameObject part in doorParts)
        {
            if (part == null) continue;

            if (disableRenderers)
            {
                foreach (var rend in part.GetComponentsInChildren<MeshRenderer>())
                    rend.enabled = false;
            }

            if (disableColliders)
            {
                foreach (var col in part.GetComponentsInChildren<Collider>())
                    col.enabled = false;
            }
        }
    }
}
