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
        for (int i = 0; i < 10; i++)
        {
            GameObject burger = Instantiate(burgerPrefab, transform);
            burger.SetActive(false);
            burgerQueue.Enqueue(burger);
        }

        StartCoroutine(BurgerCo());
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.GetComponent<PlayerController>() != null)
        {
            if(burgerStack.Count > 0)
            {
                if(other.GetComponent<PlayerController>().burgerStack.Count < other.GetComponent<PlayerController>().FoodPos.Length)
                {
                    StartCoroutine(ExitPool(other));
                }
                else
                {
                    UiManager.instance.maxText.gameObject.SetActive(true);
                }
            }
            other.GetComponent<PlayerController>().animator.SetBool("Hold", true);
        }
    }


    private void Update()
    {
        if (burgerQueue.Count < 1)
        {
            Refill(5);
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

    private IEnumerator ExitPool(Collider player)
    {
        float time = 0.5f;
        posCount--;
        GameObject burger = burgerStack.Pop();
        player.GetComponent<PlayerController>().EnterFoodPool(burger);
        burger.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        while (time > 0)
        {
            time -= Time.deltaTime;
            burger.transform.position = Vector3.Lerp(burger.transform.position, player.GetComponent<PlayerController>().FoodPos[player.GetComponent<PlayerController>().burgerCount].position, 0.01f);
            yield return null;
        }
        burger.transform.parent = player.GetComponent<PlayerController>().FoodPos[player.GetComponent<PlayerController>().burgerCount];
        burger.transform.position = player.GetComponent<PlayerController>().FoodPos[player.GetComponent<PlayerController>().burgerCount].position;
        player.GetComponent<PlayerController>().burgerCount++;
        burger.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void EnterPool(GameObject _burger)
    {

    }

    private void Refill(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject burger = Instantiate(burgerPrefab, transform);
            burger.SetActive(false);
            burgerQueue.Enqueue(burger);
        }
    }

    private IEnumerator BurgerCo()
    {
        if(burgerStack.Count < burgerPos.Length)
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if(posCount < burgerPos.Length)
                {
                    Generator();
                }
            }
        }
    }
}
