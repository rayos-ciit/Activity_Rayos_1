using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public abstract class TurretParent : MonoBehaviour
{
    public float range = 5f;
    public float fireRate = 1f;
    public GameObject rocketPrefab;

    protected LineRenderer lr;
    protected float fireTimer = 0f;

    protected virtual void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
    }

    protected virtual void Update()
    {
        Transform target = GetClosestEnemy();
        DrawRangeVisuals();
        
        if (target != null) 
        {
            ProcessTargeting(target);
        }
    }

    //targeting
    protected Transform GetClosestEnemy()
    {
        Transform closest = null;
        float minDistance = range;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float dist = (enemy.transform.position - transform.position).magnitude;
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = enemy.transform;
            }
        }
        return closest;
    }

    protected void Fire(Vector3 dir)
    {
        if (rocketPrefab != null)
        {
            GameObject proj = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            //rockets will now hit any enemy
            proj.GetComponent<Rocket>().Initialize(dir, null); 
        }
    }

    //child will do their logic
    protected abstract void DrawRangeVisuals();
    protected abstract void ProcessTargeting(Transform target);

}
