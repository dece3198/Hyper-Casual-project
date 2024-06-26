using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    [SerializeField] private GameObject burgerPrefab;
    [SerializeField] private Transform[] burgerPos;
    private Queue<GameObject> burgerQueue = new Queue<GameObject>();
    private Stack<GameObject> burgerStack = new Stack<GameObject>();

    private int posCount = 0;

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject burger = Instantiate(burgerPrefab, transform);
            burger.SetActive(false);
            burgerQueue.Enqueue(burger);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            if(burgerStack.Count > 0)
            {
                ExitPool(other);
            }
                other.GetComponent<PlayerController>().animator.SetBool("Hold", true);
        }
    }

    private void Generator()
    {
        GameObject burger = burgerQueue.Dequeue();
        burgerStack.Push(burger);
        burger.SetActive(true);
        burger.transform.position = burgerPos[posCount].position;
        posCount++;
    }

    private void ExitPool(Collider player)
    {
        for(int i = 0; i < burgerStack.Count; i++)
        {

        }
    }

    public void EnterPool(GameObject _burger)
    {

    }

    private IEnumerator BurgerCo()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            Generator();
        }
    }
}
