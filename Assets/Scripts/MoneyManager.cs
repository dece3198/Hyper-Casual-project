using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    [SerializeField] private int money;
    public int Money 
    {
        get { return money; } 
        set 
        { 
            money = value; 
            if(money < 10000)
            {
                moneyText.text = money.ToString() + "원";
            }
            else if(money >= 10000 && money < 100000000)
            {
                moneyText.text = (money/ 10000).ToString() + "만원";
            }
            else if(((money % 100000000) / 10000) < 1000)
            {
                moneyText.text = (money / 100000000).ToString() + "억";
            }
            else if(money >= 100000000)
            {
                moneyText.text = (money / 100000000).ToString() + "억" + ((money % 100000000) / 10000).ToString() + "만원";
            }
        }
    }
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        instance = this;
        Money = 200000;
    }
}
