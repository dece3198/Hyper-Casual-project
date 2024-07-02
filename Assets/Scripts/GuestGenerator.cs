using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> guests;
    [SerializeField] private GameObject destination;
    private Queue<GameObject> guestQueue = new Queue<GameObject>();

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            int rand = Random.Range(0, guests.Count);
            GameObject guest = Instantiate(guests[rand]);
            guest.transform.parent = transform;
            guest.transform.position = transform.position;
            guest.SetActive(false);
            guestQueue.Enqueue(guest);
        }

        StartCoroutine(GuestCo());
    }
    
    private void ExitPool()
    {
        GameObject guest = guestQueue.Dequeue();
        guest.SetActive(true);
        guest.GetComponent<GuestController>().agent.SetDestination(destination.transform.position);
        guest.GetComponent<GuestController>().ChangeState(GuestState.Walk);
    }

    private IEnumerator GuestCo()
    {
        while(true) 
        {
            ExitPool();
            yield return new WaitForSeconds(20f);
        }
    }
}
