using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodMoneySpawn : MonoBehaviour
{
    public MoneyFabList moneysizes;
  
    // Start is called before the first frame update
    void Start()
    {
        // instan
        moneysizes = GameObject.Find("MoneyTypes").GetComponent<MoneyFabList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MoodBasedTip(int tipsize)
    {
        switch (tipsize)
        {
          case 0:
                Instantiate(moneysizes.GetMoneyPrefab(0), transform);
                break;

          case 1:
                Instantiate(moneysizes.GetMoneyPrefab(1), transform);
                break;
          case 2:
                Instantiate(moneysizes.GetMoneyPrefab(2), transform);
                break;
            default:
                break;
        }
    }
}
