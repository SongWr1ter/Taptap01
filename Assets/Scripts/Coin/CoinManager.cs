using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : SingleTon<CoinManager>
{
    public int coinCount;

    [SerializeField] private TMP_Text coinText;
    // Start is called before the first frame update
    void Start()
    {
        updateCoin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
        updateCoin();
    }
    public bool SpendCoin(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            updateCoin();
            return true;
        }
        
        Debug.Log("Óà¶î²»×ã£¡");
        return false;
    }
    public void updateCoin()
    {
        if (coinText != null)
        {
            coinText.text = coinCount.ToString();
        }
    }
}
