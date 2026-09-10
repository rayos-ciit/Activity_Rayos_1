using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameLogic : MonoBehaviour
{
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
        //get movement for vector add and scalar multiply
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //to make sure that going directional doesn't make player move faster
        Vector3 moveDir = new Vector3(x, y, 0).normalized;

        //transform player by adding scaled vector to the current position
        transform.position += moveDir * speed * Time.deltaTime;

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
}