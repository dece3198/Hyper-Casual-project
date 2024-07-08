using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUpGradeUi : MonoBehaviour
{
    [SerializeField] private GameObject playerUi;
    [SerializeField] private GameObject[] maxUi;
    private GameObject clickUi;
    private int speedCount = 0;
    private int volumeCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            playerUi.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerUi.SetActive(false);
    }

    public void PlayerSpeedUp()
    {
        if(speedCount < 5)
        {
            speedCount++;
            if(speedCount == 5)
            {
                maxUi[0].SetActive(true);
            }

            clickUi = EventSystem.current.currentSelectedGameObject;
            if (clickUi.transform.CompareTag("Advertisement"))
            {
                JoyStick.instance.moveSpeed += 0.25f;
                return;
            }

            if(MoneyManager.instance.Money >= 50000)
            {
                JoyStick.instance.moveSpeed += 0.25f;
                MoneyManager.instance.Money -= 50000;
            }
        }
    }


    public void PlayerVolumeUp()
    {
        if (volumeCount < 10)
        {
            volumeCount++;
            if(volumeCount == 10)
            {
                maxUi[1].SetActive(true);
            }

            clickUi = EventSystem.current.currentSelectedGameObject;
            if (clickUi.transform.CompareTag("Advertisement"))
            {
                PlayerController.instance.VolumeUp();
                return;
            }

            if (MoneyManager.instance.Money >= 50000)
            {
                PlayerController.instance.VolumeUp();
                MoneyManager.instance.Money -= 50000;
            }
        }
    }


    public void XButton()
    {
        playerUi.SetActive(false);
    }
}
