using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Segment Settings")]
    [Tooltip("Array of segment prefabs that will be randomly generated.")]
    public GameObject[] segmentPrefabs;

    [Tooltip("The length of one segment along the Z-axis. For your segments (from 5 to -5) this is 10.")]
    public float segmentLength = 10f;

    [Tooltip("The number of segments that should always be present in the scene simultaneously.")]
    public int segmentsToKeep = 3;

    [Header("Spawn Point Settings")]
    [Tooltip("Reference to the Transform object that indicates where the next segment should spawn.")]
    public Transform spawnPoint;

    [Header("Player Settings")]
    [Tooltip("Reference to the player's Transform. Used to determine when to generate new segments if the player is moving.")]
    public Transform playerTransform;

    [Tooltip("The prefab of your game character that will be spawned.")]
    public GameObject playerPrefab;

    [Header("Generation Control")]
    [Tooltip("How far (in Unity units) ahead of the player's current segment should the next segment be generated? Higher values mean earlier generation.")]
    [Range(0f, 50f)] 
    public float preGenerationDistance = 20f; 

    private List<GameObject> activeSegments = new List<GameObject>();
    private GameObject spawnedPlayer;

    void Awake()
    {
        if (segmentPrefabs == null || segmentPrefabs.Length == 0)
        {
            Debug.LogError("'Segment Prefabs' array is empty! Please assign segment prefabs in the Inspector.", this);
            enabled = false;
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("'Spawn Point' object is not assigned! Please assign a Transform to Spawn Point in the Inspector.", this);
            enabled = false;
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned! Please assign the character prefab in the Inspector.", this);
            enabled = false;
            return;
        }
    }

    void Start()
    {
        SpawnPlayerAtStart();

        if (playerTransform == null)
        {
            Debug.LogError("Failed to spawn player. Level generation will not proceed.", this);
            enabled = false;
            return;
        }

        spawnPoint.position = new Vector3(0, spawnPoint.position.y, playerTransform.position.z + (segmentLength / 2f));

        for (int i = 0; i < segmentsToKeep; i++)
        {
            SpawnNextSegment();
        }

        Debug.Log("Initial segments and player spawned. Ready for continuous generation.");
    }

    void Update()
    {
        if (playerTransform != null && activeSegments.Count > 0)
        {
            float generationTriggerZ = spawnPoint.position.z - preGenerationDistance;

            Debug.Log($"Player Z: {playerTransform.position.z:F2}, Trigger Z: {generationTriggerZ:F2}, SpawnPoint Z: {spawnPoint.position.z:F2}, Active Segments: {activeSegments.Count}");

            if (playerTransform.position.z >= generationTriggerZ)
            {
                SpawnNextSegment();
            }
        }

        if (activeSegments.Count > segmentsToKeep)
        {
            DestroyOldestSegment();
        }
    }

    public void SpawnNextSegment()
    {
        int randomIndex = Random.Range(0, segmentPrefabs.Length);
        GameObject segmentToSpawn = segmentPrefabs[randomIndex];

        GameObject newSegment = Instantiate(segmentToSpawn, spawnPoint.position, spawnPoint.rotation);
        activeSegments.Add(newSegment);

        spawnPoint.position += Vector3.forward * segmentLength;

        Debug.Log($"Spawned new segment at position: {newSegment.transform.position:F2}. SpawnPoint moved to: {spawnPoint.position:F2}. Active segments: {activeSegments.Count}", newSegment);
    }

    void DestroyOldestSegment()
    {
        if (activeSegments.Count > 0)
        {
            GameObject oldestSegment = activeSegments[0];
            activeSegments.RemoveAt(0);
            Destroy(oldestSegment);

            Debug.Log($"Destroyed oldest segment. Remaining active segments: {activeSegments.Count}");
        }
    }

    void SpawnPlayerAtStart()
    {
        spawnPoint.position = new Vector3(0, spawnPoint.position.y, 0);

        Vector3 playerSpawnPosition = spawnPoint.position;
        playerSpawnPosition.z -= ((segmentLength / 2f) + 1f);
        playerSpawnPosition.y += 0.5f;

        playerSpawnPosition.z += 0.25f;

        spawnedPlayer = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);

        playerTransform = spawnedPlayer.transform;
        Debug.Log($"Player '{spawnedPlayer.name}' spawned at position: {spawnedPlayer.transform.position:F2}.", spawnedPlayer);
    }
}