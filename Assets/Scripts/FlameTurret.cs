using UnityEngine;

public class FlameTurret : TurretParent
{
    public float coneAngle = 45f;

    protected override void Start()
    {
        base.Start(); //linerenderer call
        lr.loop = true;
    }

    protected override void DrawRangeVisuals()
    {
        lr.positionCount = 3;
        float baseAngle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
        float halfCone = (coneAngle / 2f) * Mathf.Deg2Rad;

        Vector3 left = new Vector3(Mathf.Cos(baseAngle + halfCone), Mathf.Sin(baseAngle + halfCone), 0) * range;
        Vector3 right = new Vector3(Mathf.Cos(baseAngle - halfCone), Mathf.Sin(baseAngle - halfCone), 0) * range;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + left);
        lr.SetPosition(2, transform.position + right);
    }

    protected override void ProcessTargeting(Transform target)
    {
        Vector2 dirToTarget = target.position - transform.position;
        float pAngle = (Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg) - 90f;
        float delta = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, pAngle));

        if (delta <= coneAngle / 2f)
        {
            //face target always
            transform.rotation = Quaternion.Euler(0f, 0f, pAngle + 90f);
            
            fireTimer += Time.deltaTime;
            if (fireTimer >= 0.1f) 
            { 
                Fire(transform.up); 
                fireTimer = 0f; 
            }
        }
    }
}