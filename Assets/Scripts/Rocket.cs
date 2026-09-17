using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 3f; 
    private Vector3 moveDirection;

    
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
        Destroy(gameObject, lifetime); 
    }

    void Update()
    {
        
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}