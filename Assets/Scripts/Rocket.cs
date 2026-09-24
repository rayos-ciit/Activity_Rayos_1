using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 8f;
    public float hitThreshold = 0.5f;
    private Vector3 moveDirection;
    private Transform player;

    public void Initialize(Vector3 direction, Transform targetPlayer)
    {
        moveDirection = direction.normalized;
        player = targetPlayer;
        Destroy(gameObject, 4f); 
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        transform.position += moveDirection * speed * Time.deltaTime; //scale magnitude
        CheckCollision();
    }

    void CheckCollision()
    {
        //checking distance using magnitude
        if (player != null && (player.position - transform.position).magnitude < hitThreshold)
        {
            GameManager.Instance.PlayerHit();
            Destroy(gameObject);
        }
    }
}