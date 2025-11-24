using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Pieces")]
    public PipePiece startPiece;
    public PipePiece endPiece;
    public PipePiece[] allPieces;

    [Header("Reward Settings")]
    public GameObject rewardObject;
    public AudioSource rewardSound;

    public bool puzzleSolved = false;

    public void CheckConnections()
    {
        Debug.Log("Checking Puzzle...");

        if (puzzleSolved) return;

        HashSet<PipePiece> visited = new HashSet<PipePiece>();

        if (Traverse(startPiece, visited))
        {
            if (visited.Contains(endPiece))
            {
                Debug.Log("PIPE PUZZLE SOLVED!");
                puzzleSolved = true;

                if (rewardObject != null)
                    rewardObject.SetActive(true);

                if (rewardSound != null)
                    rewardSound.Play();
            }
        }
    }

    bool Traverse(PipePiece piece, HashSet<PipePiece> visited)
    {
        visited.Add(piece);

        foreach (PipePiece neighbor in allPieces)
        {
            if (visited.Contains(neighbor))
                continue;

            if (AreConnected(piece, neighbor))
                Traverse(neighbor, visited);
        }

        return visited.Contains(endPiece);
    }

    bool AreConnected(PipePiece a, PipePiece b)
    {
        Vector3 dir = b.transform.position - a.transform.position;

        // LEFT / RIGHT
        if (Mathf.Abs(dir.x) > 0.4f)
        {
            if (dir.x > 0 && a.right && b.left) return true;
            if (dir.x < 0 && a.left && b.right) return true;
        }

        // UP / DOWN
        if (Mathf.Abs(dir.z) > 0.4f)
        {
            if (dir.z > 0 && a.up && b.down) return true;
            if (dir.z < 0 && a.down && b.up) return true;
        }

        return false;
    }
}
