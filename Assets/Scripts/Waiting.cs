using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Waiting : MonoBehaviour
{
    public GuestController guest;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GuestController>() != null)
        {
            if (guest != null)
            {
                if(guest == other.GetComponent<GuestController>())
                {
                    if (guest.guestState != GuestState.Specify)
                    {
                        if (transform.CompareTag("Order"))
                        {
                            guest.ChangeState(GuestState.Order);
                            return;
                        }
                        guest.ChangeState(GuestState.Idle);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if(transform.CompareTag("Order"))
        {
            if(guest != null)
            {
                if (guest.guestState == GuestState.Specify)
                {
                    guest = null;
                }
            }
        }
    }
}
