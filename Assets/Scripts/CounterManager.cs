using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterManager : MonoBehaviour
{
    [SerializeField] private Transform[] burgerPos;
    private Stack<GameObject> burgerStack = new Stack<GameObject>();
    private int burgerCount = 0;

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            if(other.GetComponent<PlayerController>().burgerStack.Count > 0)
            {
                if(burgerCount < burgerPos.Length)
                {
                    StartCoroutine(CounterCo(other));
                }
            }
        }
    }

    private IEnumerator CounterCo(Collider player)
    {
        float time = 0.5f;
        GameObject burger = player.GetComponent<PlayerController>().burgerStack.Pop();
        burger.GetComponent<Rigidbody>().isKinematic = false;
        burger.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        while(time > 0)
        {
            time -= Time.deltaTime;
            burger.transform.position = Vector3.Lerp(burger.transform.position, burgerPos[burgerCount].position, 0.01f);
            yield return null;
        }
        burger.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<PlayerController>().burgerCount--;
        burger.transform.parent = burgerPos[burgerCount];
        burger.transform.position = burgerPos[burgerCount].position;
        burgerStack.Push(burger);
        burgerCount++;
    }
}
