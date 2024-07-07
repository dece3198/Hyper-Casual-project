using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class StaffUpGradeManager : MonoBehaviour
{
    public static StaffUpGradeManager instance;
    [SerializeField] private GameObject upGradeUi;
    [SerializeField] private GameObject maxUiA;
    [SerializeField] private GameObject maxUiB;
    [SerializeField] private GameObject maxUiC;
    private GameObject clickUi;
    private int speedCount = 0;
    private int volumeCount = 0;
    public int PersonnelCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerExit(Collider other)
    {
        upGradeUi.SetActive(false);
    }

    public void StaffSpeedUp()
    {
        if(speedCount < 5)
        {
            speedCount++;
            if (speedCount == 5)
            {
                maxUiA.SetActive(true);
            }
            clickUi = EventSystem.current.currentSelectedGameObject;
            if (clickUi.transform.CompareTag("Advertisement"))
            {
                for (int i = 0; i < StaffManager.instance.transform.childCount; i++)
                {
                    StaffManager.instance.transform.GetChild(i).GetComponent<NavMeshAgent>().speed += 0.25f;
                }
                return;
            }

            if (MoneyManager.instance.Money >= 50000)
            {
                for (int i = 0; i < StaffManager.instance.transform.childCount; i++)
                {
                    StaffManager.instance.transform.GetChild(i).GetComponent<NavMeshAgent>().speed += 0.25f;
                }
                MoneyManager.instance.Money -= 50000;
            }
        }   
    }

    public void StaffVolumeUp()
    {
        if(volumeCount < 10)
        {
            volumeCount++;
            if (volumeCount == 10)
            {
                maxUiB.SetActive(true);
            }
            clickUi = EventSystem.current.currentSelectedGameObject;
            if (clickUi.transform.CompareTag("Advertisement"))
            {
                for (int i = 0; i < StaffManager.instance.transform.childCount; i++)
                {
                    StaffManager.instance.transform.GetChild(i).GetComponent<StaffController>().VolumeUp();
                }
                return;
            }

            if (MoneyManager.instance.Money >= 50000)
            {
                for (int i = 0; i < StaffManager.instance.transform.childCount; i++)
                {
                    StaffManager.instance.transform.GetChild(i).GetComponent<StaffController>().VolumeUp();
                }
                MoneyManager.instance.Money -= 50000;
            }
        }
    }

    public void StaffPersonnelUp()
    {
        if (PersonnelCount < 5)
        {
            clickUi = EventSystem.current.currentSelectedGameObject;
            if (clickUi.transform.CompareTag("Advertisement"))
            {

                StaffManager.instance.transform.GetChild(PersonnelCount).gameObject.SetActive(true);
                PersonnelCount++;
                if (PersonnelCount == 5)
                {
                    maxUiC.SetActive(true);
                }
                return;
            }

            if (MoneyManager.instance.Money >= 100000)
            {
                StaffManager.instance.transform.GetChild(PersonnelCount).gameObject.SetActive(true);
                MoneyManager.instance.Money -= 100000;
            }
            PersonnelCount++;
            if (PersonnelCount == 4)
            {
                maxUiC.SetActive(true);
            }

        }
    }

    public void Xbutton()
    {
        gameObject.SetActive(false);
    }
}
