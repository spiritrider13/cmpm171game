using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyMoneyMoney : MonoBehaviour
{
    public int moneyScore;

    public Text score;


    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        moneyScore += scoreToAdd;
        score.text = "$" + moneyScore;
    }

}
