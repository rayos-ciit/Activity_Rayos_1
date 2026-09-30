using UnityEngine;

public class ShotgunTurret : TurretParent
{
    protected override void Start()
    {
        base.Start();
        lr.loop = true;
    }

    protected override void DrawRangeVisuals()
    {
        int segments = 36;
        lr.positionCount = segments;
        for (int i = 0; i < segments; i++)
        {
            float rad = (i * 10f) * Mathf.Deg2Rad;
            lr.SetPosition(i, transform.position + new Vector3(Mathf.Cos(rad) * range, Mathf.Sin(rad) * range, 0));
        }
    }

    protected override void ProcessTargeting(Transform target)
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Vector2 dir = target.position - transform.position;
            float baseAngleRad = Mathf.Atan2(dir.y, dir.x);
            float spreadRad = 15f * Mathf.Deg2Rad;

            FireSpread(baseAngleRad);
            FireSpread(baseAngleRad + spreadRad);
            FireSpread(baseAngleRad - spreadRad);
            fireTimer = 0f;
        }
    }

    void FireSpread(float rad)
    {
        Fire(new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0));
    }
}