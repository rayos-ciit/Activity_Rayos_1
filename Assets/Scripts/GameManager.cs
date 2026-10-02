using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Transform> activeCreatures = new List<Transform>();
    public bool isGameOver = false;

    [Header("HP System")]
    public Image realHpBar;
    public Image ghostHpBar;
    public int currentHP = 20; 
    private float maxHP = 20f;
    
    private float ghostDelayTimer = 0f;
    private float ghostLerpT = 0f;
    private float ghostStartFill = 1f;

    [Header("Coin System")]
    public TMP_Text coinText;
    public RectTransform coinIconUI;
    private float displayCoins = 0f;
    private int targetCoins = 0;

    void Awake() { Instance = this; }

    void Update()
    {
        ProcessGhostHP();
        ProcessCoinUI();
    }


    public void TakeDamage()
    {
        currentHP--;
        realHpBar.fillAmount = currentHP / maxHP; 
        
        ghostDelayTimer = 0f;
        ghostLerpT = 0f;
        ghostStartFill = ghostHpBar.fillAmount;
        
        if (currentHP <= 0 && !isGameOver) 
        {
            isGameOver = true;
            
            //changed this so that the HPBar emptying makes all active creatures stop moving
            foreach (Transform creature in activeCreatures)
            {
                if (creature != null) Destroy(creature.gameObject);
            }
            activeCreatures.Clear();
        }
    }

    void ProcessGhostHP()
    {
        if (ghostHpBar.fillAmount > realHpBar.fillAmount)
        {
            ghostDelayTimer += Time.deltaTime;
            if (ghostDelayTimer > 0.5f) 
            {
                ghostLerpT += Time.deltaTime * 1.5f; 
                float ease = EquationsUtility.EaseOutQuad(Mathf.Clamp01(ghostLerpT));
                ghostHpBar.fillAmount = Mathf.Lerp(ghostStartFill, realHpBar.fillAmount, ease);
            }
        }
    }

    public void AddCoin(int amount)
    {
        targetCoins += amount;
        coinIconUI.localScale = Vector3.one * 1.5f;
    }

    void ProcessCoinUI()
    {
        //scaling down to normal
        coinIconUI.localScale = Vector3.Lerp(coinIconUI.localScale, Vector3.one, Time.deltaTime * 5f);


        if (Mathf.Abs(displayCoins - targetCoins) > 0.1f)
        {
            displayCoins = Mathf.Lerp(displayCoins, targetCoins, Time.deltaTime * 5f);
            coinText.text = "" + Mathf.RoundToInt(displayCoins);
        }
    }
}