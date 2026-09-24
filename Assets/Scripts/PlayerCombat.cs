using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameObject rocketPrefab;
    public int rocketCount = 4;
    public int maxRockets = 8;
    public float fireRate = 3f;
    private float fireTimer = 0f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        
        
        ProcessBarrageTimer();
    }

    void ProcessBarrageTimer()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            FireRockets();
            fireTimer = 0f;
        }
    }

    void FireRockets()
    {
        if (rocketPrefab == null) return;
        float angleStep = 360f / rocketCount;
        float currentAngle = angleStep / 2f;

        for (int i = 0; i < rocketCount; i++)
        {
            float radians = (currentAngle - 90f) * Mathf.Deg2Rad; //for the -90 offset
            float dirX = Mathf.Cos(radians);
            float dirY = Mathf.Sin(radians);
            Vector3 fireDirection = new Vector3(dirX, dirY, 0);

            GameObject newRocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            
            //passing null for target player so that they don't trigger against the player.
            newRocket.GetComponent<Rocket>().Initialize(fireDirection, null);

            currentAngle += angleStep;
        }
    }

    //powerup script communication with the player
    public void AddRocket()
    {
        rocketCount = Mathf.Min(rocketCount + 1, maxRockets);
    }
}