using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera playerCamera;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;

    [Header("Spawn Settings")]
    public float spawnDelay = 10f;
    private bool monsterActive = false;

    [Header("Jumpscare Settings")]
    public AudioClip jumpscareSound;
    public GameObject jumpscareUI;
    public float jumpscareDuration = 2f;

    private AudioSource audioSource;
    private bool isScaring = false;
    private NavMeshAgent agent;
    private Renderer[] renderers;
    private Collider[] colliders;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;

        audioSource = GetComponent<AudioSource>();

        if (jumpscareUI != null)
            jumpscareUI.SetActive(false);

        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        SetMonsterVisible(false);

        monsterActive = false;
        StartCoroutine(EnableMonsterAfterDelay());
    }

    private System.Collections.IEnumerator EnableMonsterAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SetMonsterVisible(true);
        monsterActive = true;
        Debug.Log("Monster has spawned and is now hunting...");
    }

    private void Update()
    {
        if (!monsterActive) return;

        if (player == null || playerCamera == null || isScaring)
            return;

        if (!IsMonsterVisible())
        {
            ChasePlayer();
        }
        else
        {
            agent.ResetPath();
        }
    }

    void SetMonsterVisible(bool state)
    {
        foreach (Renderer r in renderers)
            r.enabled = state;

        foreach (Collider c in colliders)
            c.enabled = state;

        if (agent != null)
            agent.enabled = state;
    }

    bool IsMonsterVisible()
    {
        Vector3 viewportPos = playerCamera.WorldToViewportPoint(transform.position);

        if (viewportPos.z < 0)
            return false;

        return viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f;
    }

    void ChasePlayer()
    {
        if (agent == null) return;
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!monsterActive) return;
        if (isScaring) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(DoJumpscare());
        }
    }

    private System.Collections.IEnumerator DoJumpscare()
    {
        isScaring = true;
        agent.ResetPath();

        if (jumpscareUI != null)
            jumpscareUI.SetActive(true);

        if (jumpscareSound != null)
            AudioSource.PlayClipAtPoint(jumpscareSound, transform.position);

        transform.position = player.position + player.forward * 0.5f;
        transform.LookAt(player);

        yield return new WaitForSeconds(jumpscareDuration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisableMonster()
    {
        Debug.Log("Monster disabled by keypad correct code.");

        monsterActive = false;
        isScaring = false;

        if (agent != null)
        {
            agent.ResetPath();
            agent.enabled = false;
        }

        SetMonsterVisible(false);
    }
}
