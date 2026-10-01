using UnityEngine;

public class CubicSpawner : BaseSpawner
{
    [Header("Cubic Path Points")]
    public Transform spawnPoint;
    public Transform controlPoint1;
    public Transform controlPoint2;
    public Transform targetPoint;

    protected override void SpawnCreature()
    {
        if (creaturePrefab == null || spawnPoint == null || controlPoint1 == null || controlPoint2 == null || targetPoint == null) return;

        GameObject newObj = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
        Creature creatureScript = newObj.GetComponent<Creature>();
        
        if (creatureScript != null)
        {
            //4 points!!
            creatureScript.pathPoints = new Transform[] { spawnPoint, controlPoint1, controlPoint2, targetPoint };
        }
    }

    protected override void OnDrawGizmos()
    {
        if (spawnPoint == null || controlPoint1 == null || controlPoint2 == null || targetPoint == null) return;

        Gizmos.color = Color.cyan;
        Vector3 previousPoint = spawnPoint.position;

        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;
            Vector3 currentPoint = EquationsUtility.CubicFast(spawnPoint.position, controlPoint1.position, controlPoint2.position, targetPoint.position, t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}