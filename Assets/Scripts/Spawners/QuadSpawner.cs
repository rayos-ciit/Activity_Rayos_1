using UnityEngine;

public class QuadSpawner : BaseSpawner
{
    [Header("Quadratic Path Points")]
    public Transform spawnPoint;
    public Transform controlPoint;
    public Transform targetPoint;

    protected override void SpawnCreature()
    {
        if (creaturePrefab == null || spawnPoint == null || controlPoint == null || targetPoint == null) return;

        GameObject newObj = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
        Creature creatureScript = newObj.GetComponent<Creature>();
        
        if (creatureScript != null)
        {
            //3 points!
            creatureScript.pathPoints = new Transform[] { spawnPoint, controlPoint, targetPoint };
        }
    }

    //visualization of pathing

    protected override void OnDrawGizmos()
    {
        if (spawnPoint == null || controlPoint == null || targetPoint == null) return;

        Gizmos.color = Color.yellow;
        Vector3 previousPoint = spawnPoint.position;

        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;
            Vector3 currentPoint = EquationsUtility.QuadraticFast(spawnPoint.position, controlPoint.position, targetPoint.position, t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}