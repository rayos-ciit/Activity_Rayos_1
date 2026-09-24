using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject winUI;
    public bool isGameOver = false;

    void Awake()
    {
        //singleton for easier access
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayerHit()
    {
        if (isGameOver) return;
        isGameOver = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayerWon()
    {
        if (isGameOver) return;
        isGameOver = true;
        winUI.SetActive(true);
    }
}