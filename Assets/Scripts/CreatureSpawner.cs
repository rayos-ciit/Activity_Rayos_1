using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject creaturePrefab;
    public Transform[] assignedPath; 
    public float spawnRate = 2.5f;
    
    private float timer = 0f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnCreature();
            timer = 0f;
        }
    }

    void SpawnCreature()
    {
        if (creaturePrefab == null || assignedPath.Length == 0) return;

        //spawns the creature in through selected position
        GameObject newObj = Instantiate(creaturePrefab, transform.position, Quaternion.identity);
        
        Creature creatureScript = newObj.GetComponent<Creature>();
        if (creatureScript != null)
        {
            creatureScript.pathPoints = assignedPath;
        }
    }
}