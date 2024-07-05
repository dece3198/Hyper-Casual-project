using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingManager : MonoBehaviour
{
    public static WaitingManager instance;
    public Waiting[] waitingPos;
    public int waitingCount = 0;
    [SerializeField] Transform exitPos;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (waitingPos[0].guest == null)
        {
            for (int i = 0; i < waitingPos.Length; i++)
            {
                if (waitingPos[i].guest != null)
                {
                    waitingPos[i - 1].guest = waitingPos[i].guest;
                    waitingPos[i].guest.agent.SetDestination(waitingPos[i - 1].transform.position);
                    waitingPos[i].guest = null;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GuestController>() != null)
        {
            if(other.GetComponent<GuestController>().guestState != GuestState.Out)
            {
                for (int i = 0; i < waitingPos.Length; i++)
                {
                    if (waitingPos[i].guest == null)
                    {
                        waitingCount++;
                        waitingPos[i].guest = other.GetComponent<GuestController>();
                        other.GetComponent<GuestController>().agent.SetDestination(waitingPos[i].transform.position);
                        return;
                    }
                }
            }
            else
            {
                other.GetComponent<GuestController>().agent.SetDestination(exitPos.position);
            }
        }
    }
}
