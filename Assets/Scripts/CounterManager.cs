using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CounterManager : MonoBehaviour
{
    [SerializeField] private Waiting waiting;
    [SerializeField] private Transform[] foodPos;
    [SerializeField] private Transform[] foodPos2;
    private Stack<GameObject> burgerStack = new Stack<GameObject>();
    private Stack<GameObject> fizzCupStack = new Stack<GameObject>();
    private int burgerCount = 0;
    private int fizzCupCount = 0;
    private bool isFizzCup = true;
    private bool isBurger = true;

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            if(other.GetComponent<PlayerController>().burgerStack.Count > 0)
            {
                if (burgerCount < foodPos.Length)
                {
                    StartCoroutine(CounterCo(other.GetComponent<PlayerController>().burgerStack.Pop(), foodPos[burgerCount], burgerStack));
                    burgerCount++;
                    other.GetComponent<PlayerController>().burgerCount--;
                }
            }

            if(other.GetComponent<PlayerController>().fizzCupStack.Count > 0)
            {
                if (fizzCupCount < foodPos2.Length)
                {
                    StartCoroutine(CounterCo(other.GetComponent<PlayerController>().fizzCupStack.Pop(), foodPos2[fizzCupCount], fizzCupStack));
                    fizzCupCount++;
                    other.GetComponent<PlayerController>().fizzCount--;
                }
            }
        }
    }

    private void Update()
    {
        if(waiting.guest != null)
        {
            if (waiting.guest.guestState == GuestState.Order)
            {
                if (isFizzCup)
                {
                    if(fizzCupStack.Count > 0)
                    StartCoroutine(fizzCupExit(waiting));
                }

                if(isBurger)
                {
                    if (burgerStack.Count > 0)
                        StartCoroutine(BurgerExit(waiting));
                }
            }
        }
    }

    private IEnumerator fizzCupExit(Waiting _waiting)
    {
        isFizzCup = false;
        while (waiting.guest.fizzCupRand > 0)
        {
            yield return new WaitForSeconds(6f);
            if(fizzCupStack.Count > 0)
            {
                float time = 0.5f;
                GameObject fizzcup = fizzCupStack.Pop();
                _waiting.guest.fizzCupStack.Push(fizzcup);
                fizzCupCount--;
                fizzcup.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);

                while (time > 0)
                {
                    time -= Time.deltaTime;
                    fizzcup.transform.position = Vector3.Lerp(fizzcup.transform.position, _waiting.guest.fizzCupPos[_waiting.guest.fizzCupCount].position, 0.01f);
                    yield return null;
                }

                fizzcup.transform.parent = _waiting.guest.fizzCupPos[_waiting.guest.fizzCupCount];
                fizzcup.transform.position = _waiting.guest.fizzCupPos[_waiting.guest.fizzCupCount].position;
                _waiting.guest.fizzCupCount++;
                _waiting.guest.fizzCupRand--;
            }
        }
        isFizzCup = true;
    }

    private IEnumerator BurgerExit(Waiting _waiting)
    {
        isBurger = false;
        while (waiting.guest.burgerRand > 0)
        {
            yield return new WaitForSeconds(3f);
            if(burgerStack.Count > 0)
            {
                float time = 0.5f;
                GameObject burger = burgerStack.Pop();
                _waiting.guest.burgerStack.Push(burger);
                burgerCount--;
                burger.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);

                while (time > 0)
                {
                    time -= Time.deltaTime;
                    burger.transform.position = Vector3.Lerp(burger.transform.position, _waiting.guest.burgerPos[_waiting.guest.burgerCount].position, 0.01f);
                    yield return null;
                }

                burger.transform.parent = _waiting.guest.burgerPos[_waiting.guest.burgerCount];
                burger.transform.position = _waiting.guest.burgerPos[_waiting.guest.burgerCount].position;
                _waiting.guest.burgerCount++;
                _waiting.guest.burgerRand--;
            }
        }
        isBurger = true;
    }

    private IEnumerator CounterCo(GameObject food, Transform pos,Stack<GameObject> stack)
    {
        float time = 0.5f;
        food.GetComponent<Rigidbody>().isKinematic = false;
        food.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        while(time > 0)
        {
            time -= Time.deltaTime;
            food.transform.position = Vector3.Lerp(food.transform.position, pos.position, 0.001f);
            yield return null;
        }
        food.GetComponent<Rigidbody>().isKinematic = true;
        food.transform.parent = pos;
        food.transform.position = pos.position;
        stack.Push(food);
    }
}
