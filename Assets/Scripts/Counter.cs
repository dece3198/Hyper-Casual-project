using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static Counter instance;
    public GameObject counter;
    public GameObject counterPos;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GroundChecker>() != null)
        {
            counter = other.GetComponent<GroundChecker>().gameObject;
        }

        if(other.GetComponent<StaffController>() != null)
        {
            other.GetComponent<StaffController>().agent.ResetPath();
            other.GetComponent<StaffController>().isWalk = false;
            other.GetComponent<StaffController>().animator.SetBool("Run", false);
            other.GetComponent<StaffController>().transform.position = counterPos.transform.position;
            other.GetComponent<StaffController>().transform.rotation = counterPos.transform.rotation;
            if(StaffUpGradeManager.instance.PersonnelCount < 2)
            {
                StartCoroutine(CounterCo(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        counter = null;
    }

    private IEnumerator CounterCo(Collider other)
    {
        yield return new WaitForSeconds(10f);
        other.GetComponent<StaffController>().ChangeState(StaffState.Idle);
    }
}
