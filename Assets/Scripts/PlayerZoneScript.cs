using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerZoneScript : MonoBehaviour
{
    [Header("Rocket Barrage")]
    public GameObject rocketPrefab;
    public int rocketCount = 4;
    public int maxRockets = 8;
    public float fireRate = 3f;
    private float fireTimer = 0f;

    [Header("Power-Ups")]
    public Transform powerUpZone;
    public float pickupThreshold = 1.0f;

    //player movement speed
    [Header("Movement")]
    public float speed = 5f;

    //positions of the zones
    [Header("References")]
    public Transform noGoZone;
    public Transform finishZone;
    public GameObject winUI;

    //tresholds for zones
    [Header("Thresholds")]
    public float warningThreshold = 3f;
    public float failThreshold = 1.2f;
    public float winThreshold = 1.2f;

    private Vector3 originalNoGoPos;
    private SpriteRenderer noGoRenderer;

    void Start()
    {
        //puts original state of no-go zone here for shake
        originalNoGoPos = noGoZone.position;
        noGoRenderer = noGoZone.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //cardinal movement
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //if moving horizontally cancel vertical input
        if (x != 0) y = 0; 

        //to make sure that going directional doesn't make player move faster
        Vector3 moveDir = new Vector3(x, y, 0).normalized;

        //transform player by adding scaled vector to the current position
        transform.position += moveDir * speed * Time.deltaTime;


        //timer for rocket barrage
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            FireRockets();
            fireTimer = 0f;
        }


        //prox of power-up
        if (powerUpZone != null && powerUpZone.gameObject.activeSelf)
        {
            float distanceToPowerUp = Vector3.Distance(transform.position, powerUpZone.position);
            if (distanceToPowerUp <= pickupThreshold)
            {
                rocketCount = Mathf.Min(rocketCount + 1, maxRockets);
                powerUpZone.gameObject.SetActive(false); 
            }
        }



        //distance calculation
        float distanceToNoGo = Vector3.Distance(transform.position, noGoZone.position);
        float distanceToFinish = Vector3.Distance(transform.position, finishZone.position);

        //logic for no-go zone
        if (distanceToNoGo <= failThreshold)
        {
            //restart scene just in case
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (distanceToNoGo <= warningThreshold)
        {
            //shake effect logic
            float shakeAmount = 0.05f;
            Vector3 randomOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);

            noGoZone.position = originalNoGoPos + randomOffset;
            noGoRenderer.color = Color.red;
        }
        else
        {   
            //reset color and transform
            noGoZone.position = originalNoGoPos;
            noGoRenderer.color = Color.yellow;
        }

        //finish zone logic
        if (distanceToFinish <= winThreshold)
        {
            winUI.SetActive(true);
        }
    }

    void FireRockets()
    {
        if (rocketPrefab == null) return;

        float angleStep = 360f / rocketCount;
        float currentAngle = angleStep / 2f; 

        for (int i = 0; i < rocketCount; i++)
        {
            // convertion from angle to radians
            float radians = currentAngle * Mathf.Deg2Rad;
            float dirX = Mathf.Cos(radians);
            float dirY = Mathf.Sin(radians);
            Vector3 fireDirection = new Vector3(dirX, dirY, 0);

            //spawn and initialize
            GameObject newRocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            newRocket.GetComponent<Rocket>().Initialize(fireDirection);

            currentAngle += angleStep;
        }
    }
}