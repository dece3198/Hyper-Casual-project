using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUpGrade : MonoBehaviour
{
    [SerializeField] private GameObject upGradeUi;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            upGradeUi.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        upGradeUi.SetActive(false);
    }

    public void Xbutton()
    {
        gameObject.SetActive(false);
    }
}
