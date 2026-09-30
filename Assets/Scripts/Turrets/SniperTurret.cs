using UnityEngine;

public class SniperTurret : TurretParent
{
    private bool hasFired = false;

    protected override void DrawRangeVisuals()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.up * range);
    }

    protected override void ProcessTargeting(Transform target)
    {
        Vector2 dirToTarget = (target.position - transform.position).normalized;
        float dot = Vector2.Dot(transform.up, dirToTarget);

        if (dot >= 0.98f && !hasFired)
        {
            Fire(transform.up);
            hasFired = true; 
        }
        else if (dot < 0.98f)
        {
            hasFired = false; //this is for resseting when the target is out of range
        }
    }
}