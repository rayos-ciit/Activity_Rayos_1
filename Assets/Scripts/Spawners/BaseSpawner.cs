using UnityEngine;

public abstract class BaseSpawner : MonoBehaviour
{
    public GameObject creaturePrefab;
    public float spawnRate = 2.5f;
    
    protected float timer = 0f;

    protected virtual void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnCreature();
            timer = 0f;
        }
    }

    protected abstract void SpawnCreature();
    protected abstract void OnDrawGizmos(); 
}