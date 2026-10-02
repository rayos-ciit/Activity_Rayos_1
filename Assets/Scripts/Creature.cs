using UnityEngine;

public class Creature : MonoBehaviour
{
    public Transform[] pathPoints; //3 for quadratic, 4 for Cubic
    public GameObject coinPrefab;
    public float speed = 0.3f;
    
    private float t = 0f;

    void Start()
    {
        GameManager.Instance.activeCreatures.Add(transform);
    }

    void Update()
    {
    
        if (GameManager.Instance.isGameOver) return;

        t += speed * Time.deltaTime;

        if (t >= 1f)
        {
            GameManager.Instance.TakeDamage(); 
            RemoveSelf();
        }
        else
        {
            if (pathPoints.Length == 3)
                transform.position = EquationsUtility.QuadraticFast(pathPoints[0].position, pathPoints[1].position, pathPoints[2].position, t);
            else if (pathPoints.Length == 4)
                transform.position = EquationsUtility.CubicFast(pathPoints[0].position, pathPoints[1].position, pathPoints[2].position, pathPoints[3].position, t);
        }
    }

    public void Die()
    {
        if (coinPrefab != null) Instantiate(coinPrefab, transform.position, Quaternion.identity); // Spawn coin on death
        RemoveSelf();
    }

    void RemoveSelf()
    {
        GameManager.Instance.activeCreatures.Remove(transform);
        Destroy(gameObject);
    }
}