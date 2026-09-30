using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 8f;
    private Vector3 moveDirection;

    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
        Destroy(gameObject, 4f); 
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        transform.position += moveDirection * speed * Time.deltaTime;
        
        //collision check for all enemies
        for (int i = GameManager.Instance.activeCreatures.Count - 1; i >= 0; i--)
        {
            Transform enemy = GameManager.Instance.activeCreatures[i];
            if (enemy != null && (enemy.position - transform.position).magnitude < 0.5f)
            {
                enemy.GetComponent<Creature>().Die(); //one hit kill
                Destroy(gameObject);
                return;
            }
        }
    }
}