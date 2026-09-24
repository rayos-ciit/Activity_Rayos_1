using UnityEngine;

public class NoGoZone : MonoBehaviour
{
    public Transform player;
    public float warningThreshold = 3f;
    public float failThreshold = 1.2f;

    private Vector3 originalPos;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        originalPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        
        
        if (player != null) CheckProximity();
    }

    void CheckProximity()
    {
        //check for distance
        float distance = (player.position - originalPos).magnitude;

        if (distance <= failThreshold)
        {
            GameManager.Instance.PlayerHit();
        }
        else if (distance <= warningThreshold)
        {
            ApplyWarningState();
        }
        else
        {
            ResetState();
        }
    }

    void ApplyWarningState()
    {
        float shakeAmount = 0.05f;
        Vector3 randomOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
        transform.position = originalPos + randomOffset;
        spriteRenderer.color = Color.red;
    }

    void ResetState()
    {
        transform.position = originalPos;
        spriteRenderer.color = Color.yellow;
    }
}