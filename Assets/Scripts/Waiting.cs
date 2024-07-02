using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiting : MonoBehaviour
{
    public GuestController guest;
    public bool isGuest = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GuestController>() != null)
        {
            if(isGuest)
            {
                guest = other.GetComponent<GuestController>();
                if(transform.CompareTag("Order"))
                {
                    guest.ChangeState(GuestState.Order);
                    return;
                }
                guest.ChangeState(GuestState.Idle);
                isGuest = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        guest = null;
    }
}
