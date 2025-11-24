using UnityEngine;

public class InteractableDrawing : MonoBehaviour
{
    public Transform holdOffset;
    public float moveSpeed = 8f;

    private bool isHeld = false;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Transform originalParent;

    public void Pickup(PlayerInteract player)
    {
        if (isHeld) return;

        isHeld = true;

        originalParent = transform.parent;
        originalPos = transform.position;
        originalRot = transform.rotation;

        player.HoldDrawing(this);
    }

    public void Drop(PlayerInteract player)
    {
        if (!isHeld) return;

        isHeld = false;

        transform.SetParent(originalParent);
        transform.position = originalPos;
        transform.rotation = originalRot;

        player.ReleaseDrawing();
    }

    private void Update()
    {
        if (isHeld && Camera.main != null)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                holdOffset.position,
                Time.deltaTime * moveSpeed
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                holdOffset.rotation,
                Time.deltaTime * moveSpeed
            );
        }
    }
}
