using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzCupGenerator : MonoBehaviour
{
    public static FizzCupGenerator instance;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Transform[] foodPos;
    private Queue<GameObject> foodQueue = new Queue<GameObject>();
    private Stack<GameObject> foodStack = new Stack<GameObject>();
    private bool isFizzCupCool = true;
    private int posCount = 0;
    public float speed = 0;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        GameManager.instance.isFizzCupMachine = true;
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
                if (other.GetComponent<PlayerController>().burgerStack.Count > 0)
                {
                    if (other.GetComponent<PlayerController>().fizzCupStack.Count < other.GetComponent<PlayerController>().FoodPos3.Count)
                    {
                        for (int i = 0; i < other.GetComponent<PlayerController>().FoodPos.Count; i++)
                        {
                            if (other.GetComponent<PlayerController>().FoodPos[i].childCount > 0)
                            {
                                other.GetComponent<PlayerController>().FoodPos[i].GetChild(0).transform.position = other.GetComponent<PlayerController>().FoodPos2[i].position;
                                other.GetComponent<PlayerController>().FoodPos[i].GetChild(0).transform.parent = other.GetComponent<PlayerController>().FoodPos2[i];
                            }
                        }
                        if(isFizzCupCool)
                        {
                            StartCoroutine(FizzCupExitPool(other.GetComponent<PlayerController>(), other.GetComponent<PlayerController>().FoodPos3[other.GetComponent<PlayerController>().fizzCount]));
                            isFizzCupCool = false;
                        }
                    }
                }
                else
                {
                    if (other.GetComponent<PlayerController>().fizzCupStack.Count < other.GetComponent<PlayerController>().FoodPos.Count)
                    {
                        if(isFizzCupCool)
                        {
                            StartCoroutine(FizzCupExitPool(other.GetComponent<PlayerController>(), other.GetComponent<PlayerController>().FoodPos[other.GetComponent<PlayerController>().fizzCount]));
                            isFizzCupCool = false;
                        }
                    }
                }
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

    private IEnumerator FizzCupExitPool(PlayerController player, Transform foodPos)
    {
        float time = 0.5f;
        posCount--;
        GameObject food = foodStack.Pop();
        player.fizzCupStack.Push(food);
        food.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        while (time > 0)
        {
            time -= Time.deltaTime;
            food.transform.position = Vector3.Lerp(food.transform.position, foodPos.position, 0.001f);
            yield return null;
        }
        food.transform.parent = foodPos;
        food.transform.position = foodPos.position;
        player.fizzCount++;
        food.GetComponent<Rigidbody>().isKinematic = true;
        isFizzCupCool = true;
    }


    public void EnterPool(GameObject _burger)
    {
        foodQueue.Enqueue(_burger);
        _burger.transform.parent = transform;
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
        if (foodStack.Count < foodPos.Length)
        {
            while (true)
            {
                yield return new WaitForSeconds(speed);
                if (posCount < foodPos.Length)
                {
                    Generator();
                }
            }
        }
    }

}
