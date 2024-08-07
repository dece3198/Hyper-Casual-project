using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerGenerator : MonoBehaviour
{
    public static BurgerGenerator instance;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Transform[] foodPos;
    private Queue<GameObject> foodQueue = new Queue<GameObject>();
    private Stack<GameObject> foodStack = new Stack<GameObject>();
    private bool isBurgerCool = true;
    public bool isStaffCool = true;
    private int posCount = 0;
    public float speed = 0;

    private void Awake()
    {
        instance = this;
    }

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
            if (foodStack.Count > 0)
            {
                if (other.GetComponent<PlayerController>().fizzCupStack.Count > 0)
                {
                    if (other.GetComponent<PlayerController>().burgerStack.Count < other.GetComponent<PlayerController>().FoodPos2.Count)
                    {

                        for (int i = 0; i < other.GetComponent<PlayerController>().FoodPos.Count; i++)
                        {
                            if (other.GetComponent<PlayerController>().FoodPos[i].childCount > 0)
                            {
                                other.GetComponent<PlayerController>().FoodPos[i].GetChild(0).transform.position = other.GetComponent<PlayerController>().FoodPos3[i].position;
                                other.GetComponent<PlayerController>().FoodPos[i].GetChild(0).transform.parent = other.GetComponent<PlayerController>().FoodPos3[i];
                            }
                        }
                        if(isBurgerCool)
                        {
                            StartCoroutine(BurgerExitPool(other.GetComponent<PlayerController>(), other.GetComponent<PlayerController>().FoodPos2[other.GetComponent<PlayerController>().burgerCount]));
                            isBurgerCool = false;
                        }
                    }
                }
                else
                {
                    if (other.GetComponent<PlayerController>().burgerStack.Count < other.GetComponent<PlayerController>().FoodPos.Count)
                    {
                        if(isBurgerCool)
                        {
                            StartCoroutine(BurgerExitPool(other.GetComponent<PlayerController>(), other.GetComponent<PlayerController>().FoodPos[other.GetComponent<PlayerController>().burgerCount]));
                            isBurgerCool = false;
                        }
                    }
                }
            }
        }


        if (other.GetComponent<StaffController>() != null)
        {
            other.GetComponent<StaffController>().animator.SetBool("Run", false);
            if (other.GetComponent<StaffController>().foodStack.Count < other.GetComponent<StaffController>().foodPos.Count)
            {
                if(foodStack.Count > 0)
                {
                    if (isStaffCool)
                    {
                        StartCoroutine(BurgerStaffExtiPool(other.GetComponent<StaffController>(), other.GetComponent<StaffController>().foodPos[other.GetComponent<StaffController>().foodCount]));
                        isStaffCool = false;
                    }
                }
            }
            else
            {
                other.GetComponent<StaffController>().animator.SetBool("Run", true);
                other.GetComponent<StaffController>().isWalk = true;
                other.GetComponent<StaffController>().agent.SetDestination(CounterManager.instance.staffPos.position);
            }
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
            food.transform.position = Vector3.Lerp(food.transform.position, foodPos.position, 0.001f);
            yield return null;
        }
        food.transform.parent = foodPos;
        food.transform.position = foodPos.position;
        player.burgerCount++;
        food.GetComponent<Rigidbody>().isKinematic = true;
        isBurgerCool = true;
    }

    private IEnumerator BurgerStaffExtiPool(StaffController staff, Transform foodPos)
    {
        float time = 1f;
        posCount--;
        GameObject food = foodStack.Pop();
        food.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
        while(time > 0)
        {
            time -= Time.deltaTime;
            food.transform.position = Vector3.Lerp(food.transform.position, foodPos.position, 0.01f);
            yield return null;
        }
        staff.foodStack.Push(food);
        food.transform.parent = foodPos;
        food.transform.position = foodPos.position;
        staff.foodCount++;
        food.GetComponent<Rigidbody>().isKinematic = true;
        isStaffCool = true;
    }

    public void EnterPool(GameObject _burger)
    {
        foodQueue.Enqueue(_burger);
        _burger.transform.parent = transform;
        _burger.GetComponent<Rigidbody>().isKinematic = true;
        _burger.SetActive(false);
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
                yield return new WaitForSeconds(speed);
                if(posCount < foodPos.Length)
                {
                    Generator();
                }
            }
        }
    } 
}
