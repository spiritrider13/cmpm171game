using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoneyMoneyMoney : MonoBehaviour
{
    public int moneyScore;

    public Text score;
    public void Start()
    {
        DontDestroyOnLoad(transform.gameObject);



    }
    public void forthefunny() { 
        if(SceneManager.GetActiveScene().name == "End Scene")
        {
        
                score = GameObject.Find("Text (Legacy)").GetComponent<Text>();
            
            addScore(0);
        }
    }


    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        moneyScore += scoreToAdd;
        Debug.Log("amongus" + scoreToAdd);
        score.text = "$" + moneyScore;
    }

}
