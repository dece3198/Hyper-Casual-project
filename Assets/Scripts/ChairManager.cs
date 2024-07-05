using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public static ChairManager instance;
    public List<Chair> chair = new List<Chair>();
    private List<Chair> randChairs = new List<Chair>();
    public int chairCount = 0;

    private void Awake()
    {
        instance = this;
    }

    public void Specify(GuestController guest)
    {

        for(int i = 0; i < chair.Count; i++)
        {
            if (chair[i].guest == null)
            {
                randChairs.Add(chair[i]);
            }
        }

        int rand = Random.Range(0, randChairs.Count);
        randChairs[rand].guest = guest;
        guest.agent.SetDestination(randChairs[rand].transform.position);
        chairCount++;
        randChairs.Clear();
    }
}
