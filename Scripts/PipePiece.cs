using UnityEngine;

public class PipePiece : MonoBehaviour
{
    [Header("Connection Directions")]
    public bool up;
    public bool down;
    public bool left;
    public bool right;

    private PuzzleManager puzzleManager;

    void Start()
    {
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
    }

    public void RotatePiece()
    {
        transform.Rotate(Vector3.up * 90f, Space.World);

        bool oldUp = up;
        up = left;
        left = down;
        down = right;
        right = oldUp;

        puzzleManager.CheckConnections();
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position, Vector3.one * 0.9f);
    //}
}
