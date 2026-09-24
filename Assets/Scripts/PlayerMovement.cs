using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform finishZone;
    public float winThreshold = 1.0f;

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        HandleInput();
        CheckWinCondition();
    }

    void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x != 0) y = 0; 

        Vector3 moveDir = new Vector3(x, y, 0).normalized; //normalize direction
        transform.position += moveDir * speed * Time.deltaTime; //combined movement
    }

    void CheckWinCondition()
    {
        
        if ((finishZone.position - transform.position).magnitude <= winThreshold) 
        {
            GameManager.Instance.PlayerWon();
        }
    }
}