using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public static ChairManager instance;
    [SerializeField] private Chair[] chair;
    private List<Chair> chairs = new List<Chair>();

    private void Awake()
    {
        instance = this;
    }

    public void Specify(GuestController guest)
    {

        for(int i = 0; i < chair.Length; i++)
        {
            if (chair[i].guest == null)
            {
                chairs.Add(chair[i]);
            }
        }

        int rand = Random.Range(0, chairs.Count);
        chairs[rand].guest = guest;
        guest.agent.SetDestination(chairs[rand].transform.position);
        chairs.Clear();
    }
}
