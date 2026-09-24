using UnityEngine;

public enum TurretType { Flame, Sniper, Shotgun }

[RequireComponent(typeof(LineRenderer))]
public class Turret : MonoBehaviour
{
    public TurretType type;
    public Transform player;
    public GameObject rocketPrefab;
    
    public float range = 5f;
    public float fireRate = 1f;
    public float coneAngle = 45f; 

    private LineRenderer lr;
    private float fireTimer = 0f;
    private bool hasFiredSniper = false;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.loop = type == TurretType.Shotgun || type == TurretType.Flame; 
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        DrawRangeVisuals();
        if (player != null) ProcessTargeting();
    }

    //visualization 

    void DrawRangeVisuals()
    {
        if (type == TurretType.Sniper) DrawLine();
        else if (type == TurretType.Flame) DrawCone();
        else if (type == TurretType.Shotgun) DrawRadius();
    }

    void DrawLine()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.up * range); //visual is set to point upward
    }

    void DrawCone()
    {
        lr.positionCount = 3;
        //shift base angle by +90 for alignment 
        float baseAngle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
        float halfCone = (coneAngle / 2f) * Mathf.Deg2Rad;

        //x = r*cos and y = r*sin
        Vector3 leftPoint = new Vector3(Mathf.Cos(baseAngle + halfCone), Mathf.Sin(baseAngle + halfCone), 0) * range;
        Vector3 rightPoint = new Vector3(Mathf.Cos(baseAngle - halfCone), Mathf.Sin(baseAngle - halfCone), 0) * range;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + leftPoint);
        lr.SetPosition(2, transform.position + rightPoint);
    }

    void DrawRadius()
    {
        int segments = 36;
        lr.positionCount = segments;
        for (int i = 0; i < segments; i++)
        {
            //deg*(PI/180)
            float rad = (i * 10f) * Mathf.Deg2Rad; 
            float x = Mathf.Cos(rad) * range; 
            float y = Mathf.Sin(rad) * range;
            lr.SetPosition(i, transform.position + new Vector3(x, y, 0));
        }
    }

    //targeting 

    void ProcessTargeting()
    {
        Vector2 dirToPlayer = player.position - transform.position; //difference betweeen player and target
        if (dirToPlayer.magnitude > range) return;

        if (type == TurretType.Flame)
        {
            // Offset the player angle by -90 degrees
            //atan2 starts at +X,
            float pAngle = (Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg) - 90f; 
            float tAngle = transform.eulerAngles.z;
            float delta = Mathf.Abs(Mathf.DeltaAngle(tAngle, pAngle));

            if (delta <= coneAngle / 2f)
            {
                fireTimer += Time.deltaTime;
                if (fireTimer >= 0.1f) { Fire(transform.up); fireTimer = 0f; } // Fire UP
            }
        }
        else if (type == TurretType.Sniper && !hasFiredSniper)
        {
            Vector2 barrel = transform.up;
            Vector2 toPlayer = dirToPlayer.normalized; 
            float dot = Vector2.Dot(barrel, toPlayer); 

            //two direction for alignment
            if (dot >= 0.98f) 
            {
                Fire(transform.up); 
                hasFiredSniper = true;
            }
        }
        else if (type == TurretType.Shotgun)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                //shotgun uses atan2 for pellets 
                float baseAngleRad = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x); 
                float spreadRad = 15f * Mathf.Deg2Rad; 

                FireSpread(baseAngleRad);
                FireSpread(baseAngleRad + spreadRad);
                FireSpread(baseAngleRad - spreadRad);
                fireTimer = 0f;
            }
        }
    }

    void FireSpread(float angleRad)
    {
        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);
        Fire(new Vector3(x, y, 0));
    }

    void Fire(Vector3 dir)
    {
        if (rocketPrefab != null)
        {
            GameObject proj = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Rocket>().Initialize(dir, player);
        }
    }
}