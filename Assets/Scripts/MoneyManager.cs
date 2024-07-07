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
    [SerializeField] private GameObject moneyPrefab;
    public Transform[] moneyPos;
    public Stack<GameObject> moneyStack = new Stack<GameObject>();
    public Queue<GameObject> exitMoney = new Queue<GameObject>();
    public int moneyCount = 0;

    private void Awake()
    {
        instance = this;
        Money = 100000000;
    }

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject money = Instantiate(moneyPrefab, transform);
            moneyStack.Push(money);
            money.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            if(exitMoney.Count > 0)
            {
                StartCoroutine(MoneyExitCo(other.GetComponent<PlayerController>()));
            }
        }
    }

    public void Refill(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject money = Instantiate(moneyPrefab, transform);
            moneyStack.Push(money);
            money.SetActive(false);
        }
    }
    
    private void EnterPool(GameObject money)
    {
        moneyStack.Push(money);
        money.SetActive(false);
    }

    private IEnumerator MoneyExitCo(PlayerController player)
    {
        float time = 1f;
        GameObject money = exitMoney.Dequeue();
        moneyCount--;
        while (time > 0)
        {
            time -= Time.deltaTime;
            money.transform.position = Vector3.Lerp(money.transform.position, player.moneyPos.position, 0.1f);
            yield return null;
        }
        Money += money.GetComponent<Money>().money;
        EnterPool(money);
    }
}
