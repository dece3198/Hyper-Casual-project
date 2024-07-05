using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpGradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upGradeObj;
    [SerializeField] private float upGradePrice;
    [SerializeField] private float curMoney;
    [SerializeField] private Image upImage;
    [SerializeField] private int speed;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Chair[] chairs;
    float time = 3f;


    private void Start()
    {
        if (upGradePrice < 10000)
        {
            priceText.text = upGradePrice.ToString() + "¿ø";
        }
        else if (upGradePrice >= 10000)
        {
            priceText.text = (upGradePrice / 10000).ToString() + "¸¸¿ø";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                return;
            }
            if (MoneyManager.instance.Money > 0)
            {
                curMoney += speed;
                MoneyManager.instance.Money -= speed;
            }

            upImage.fillAmount = curMoney / upGradePrice;

            if (curMoney >= upGradePrice)
            {
                switch (transform.tag)
                {
                    case "FizzCup": FizzCupMachine(); break;
                    case "TableChair": TableChair(); break;
                }

            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        time = 3f;
    }

    private void FizzCupMachine()
    {
        upGradeObj.SetActive(true);
        gameObject.SetActive(false);
    }

    private void TableChair()
    {
        for(int i = 0; i < chairs.Length; i++)
        {
            ChairManager.instance.chair.Add(chairs[i]);
        }
        upGradeObj.SetActive(true);
        gameObject.SetActive(false);
    }

}
