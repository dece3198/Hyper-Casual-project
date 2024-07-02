using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingManager : MonoBehaviour
{
    [SerializeField] private Waiting[] waitingPos;

    private void Update()
    {
        if (waitingPos[0].guest == null)
        {
            for (int i = 0; i < waitingPos.Length; i++)
            {
                if (waitingPos[i].guest != null)
                {
                    if (waitingPos[i].guest.guestState == GuestState.Idle)
                    {
                        waitingPos[i].isGuest = true;
                        waitingPos[i].guest.agent.SetDestination(waitingPos[i - 1].transform.position);
                    }
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GuestController>() != null)
        {
            for (int i = 0; i < waitingPos.Length; i++)
            {
                if (waitingPos[i].guest == null)
                {
                    waitingPos[i].isGuest = true;
                    other.GetComponent<GuestController>().agent.SetDestination(waitingPos[i].transform.position);
                    return;
                }
            }
        }
    }

}
