using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Transform player;
    public float pickupThreshold = 1.0f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        
        
        if (player != null && gameObject.activeSelf) CheckPickup();
    }

    void CheckPickup()
    {
        //distance check
        float distance = (player.position - transform.position).magnitude;
        
        if (distance <= pickupThreshold)
        {
            PlayerCombat combatScript = player.GetComponent<PlayerCombat>();
            if (combatScript != null)
            {
                combatScript.AddRocket(); //call public method on the player
            }
            gameObject.SetActive(false);
    }
}

}

