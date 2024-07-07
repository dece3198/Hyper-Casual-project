using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    public static StaffManager instance;
    public GameObject staffPrefab;
    public Transform generatorPos;
    [SerializeField] private GameObject upGradeUi;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject staff = Instantiate(staffPrefab, transform);
            staff.SetActive(false);
            staff.transform.position = generatorPos.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            upGradeUi.SetActive(true);
        }
    }
}
