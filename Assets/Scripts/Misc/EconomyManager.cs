using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;

    const string COINT_AMOUNT_TEXT = "Gold Amount Text";
    private int currentGold = 0;

    public void UpdateCurrentGold()
    {
        currentGold += 1;
        if(goldText == null)
        {
            goldText = GameObject.Find(COINT_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
    }
}
