using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Transform[] foodPos;
    private Queue<GameObject> foodQueue = new Queue<GameObject>();
    private Stack<GameObject> foodStack = new Stack<GameObject>();

    private int posCount = 0;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject burger = Instantiate(foodPrefab, transform);
            burger.SetActive(false);
            foodQueue.Enqueue(burger);
        }

        StartCoroutine(BurgerCo());
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.GetComponent<PlayerController>() != null)
        {
            if(foodStack.Count > 0)
            {
                if (transform.CompareTag("Burger"))
                {
                    if (other.GetComponent<PlayerController>().FizzCupStack.Count > 0)
                    {
                        if (other.GetComponent<PlayerController>().burgerStack.Count < other.GetComponent<PlayerController>().FoodPos2.Length)
                        {

                            for (int i = 0; i < other.GetComponent<PlayerController>().FoodPos.Length; i++)
                            {
                                if (other.GetComponent<PlayerController>().FoodPos[i].childCount > 0)
                                {
                                    other.GetComponent<PlayerController>().FoodPos[i].GetChild(0).transform.position = other.GetComponent<PlayerController>().FoodPos3[i].position;
                                    other.GetComponent<PlayerController>().FoodPos[i].GetChild(0).transform.parent = other.GetComponent<PlayerController>().FoodPos3[i];
                                }
                            }
                            StartCoroutine(BurgerExitPool(other.GetComponent<PlayerController>(), other.GetComponent<PlayerController>().FoodPos2[other.GetComponent<PlayerController>().burgerCount]));
                        }
                    }
                    else
                    {
                        if (other.GetComponent<PlayerController>().burgerStack.Count < other.GetComponent<PlayerController>().FoodPos.Length)
                        {
                            StartCoroutine(BurgerExitPool(other.GetComponent<PlayerController>(), other.GetComponent<PlayerController>().FoodPos[other.GetComponent<PlayerController>().burgerCount]));
                        }
                    }
                    if (other.GetComponent<PlayerController>().burgerCount < 14)
                    {
                        other.GetComponent<PlayerController>().burgerCount++;
                    }
                }
                else if (transform.CompareTag("FizzCup"))
                {
                    if (other.GetComponent<PlayerController>().burgerStack.Count > 0)
                    {
                        if (other.GetComponent<PlayerController>().FizzCupStack.Count < other.GetComponent<PlayerController>().FoodPos3.Length)
                        {
                            for(int i = 0; i < other.GetComponent<PlayerController>().FoodPos.Length; i++)
                            {
                                if(other.GetComponent<PlayerController>().FoodPos[i].childCount > 0)
                                {
                                    other.GetComponent<PlayerController>().FoodPos[i].GetChild(0).transform.position = other.GetComponent<PlayerController>().FoodPos2[i].position;
                                    other.GetComponent<PlayerController>().FoodPos[i].GetChild(0).transform.parent = other.GetComponent<PlayerController>().FoodPos2[i];
                                }
                            }
                            StartCoroutine(FizzCupExitPool(other.GetComponent<PlayerController>(), other.GetComponent<PlayerController>().FoodPos3[other.GetComponent<PlayerController>().fizzCount]));
                        }
                    }
                    else
                    {
                        if (other.GetComponent<PlayerController>().FizzCupStack.Count < other.GetComponent<PlayerController>().FoodPos.Length)
                        {
                            StartCoroutine(FizzCupExitPool(other.GetComponent<PlayerController>(), other.GetComponent<PlayerController>().FoodPos[other.GetComponent<PlayerController>().fizzCount]));
                        }
                    }
                    if(other.GetComponent<PlayerController>().fizzCount < 14)
                    {
                        other.GetComponent<PlayerController>().fizzCount++;
                    }
                }
            }
            other.GetComponent<PlayerController>().animator.SetBool("Hold", true);
        }
    }


    private void Update()
    {
        if (foodQueue.Count < 1)
        {
            Refill(5);
        }
    }

    private void Generator()
    {
        GameObject burger = foodQueue.Dequeue();
        foodStack.Push(burger);
        burger.SetActive(true);
        burger.transform.position = foodPos[posCount].position;
        posCount++;
    }

    private IEnumerator BurgerExitPool(PlayerController player, Transform foodPos)
    {
        float time = 0.5f;
        posCount--;
        GameObject food = foodStack.Pop();
        player.burgerStack.Push(food);
        food.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        while (time > 0)
        {
            time -= Time.deltaTime;
            food.transform.position = Vector3.Lerp(food.transform.position, foodPos.position, 0.01f);
            yield return null;
        }
        food.transform.parent = foodPos;
        food.transform.position = foodPos.position;
        food.GetComponent<Rigidbody>().isKinematic = true;
    }

    private IEnumerator FizzCupExitPool(PlayerController player, Transform foodPos)
    {
        float time = 0.5f;
        posCount--;
        GameObject food = foodStack.Pop();
        player.FizzCupStack.Push(food);
        food.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        while (time > 0)
        {
            time -= Time.deltaTime;
            food.transform.position = Vector3.Lerp(food.transform.position, foodPos.position, 0.01f);
            yield return null;
        }
        food.transform.parent = foodPos;
        food.transform.position = foodPos.position;
        food.GetComponent<Rigidbody>().isKinematic = true;
    }


    public void EnterPool(GameObject _burger)
    {

    }

    private void Refill(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject burger = Instantiate(foodPrefab, transform);
            burger.SetActive(false);
            foodQueue.Enqueue(burger);
        }
    }

    private IEnumerator BurgerCo()
    {
        if(foodStack.Count < foodPos.Length)
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if(posCount < foodPos.Length)
                {
                    Generator();
                }
            }
        }
    }
}
