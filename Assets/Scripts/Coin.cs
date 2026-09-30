using UnityEngine;

public class Coin : MonoBehaviour
{
    private Vector3 targetWorldPos;
    private float t = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //turn ui position to world space
        //this is for resolution changes just in case
        targetWorldPos = Camera.main.ScreenToWorldPoint(GameManager.Instance.coinIconUI.position);
        targetWorldPos.z = 0;

        t += Time.deltaTime * 2f;
        
        
        transform.position = Vector3.Lerp(startPos, targetWorldPos, t);

        if ((transform.position - targetWorldPos).magnitude < 0.5f)
        {
            GameManager.Instance.AddCoin(10);
            Destroy(gameObject);
        }
    }
}