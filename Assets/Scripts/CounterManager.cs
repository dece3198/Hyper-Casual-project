using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterManager : MonoBehaviour
{
    [SerializeField] private Transform[] foodPos;
    [SerializeField] private Transform[] foodPos2;
    private Stack<GameObject> burgerStack = new Stack<GameObject>();
    private int burgerCount = 0;
    private int fizzCupCount = 0;

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            if(other.GetComponent<PlayerController>().burgerStack.Count > 0)
            {
                if(burgerCount < foodPos.Length)
                {
                    StartCoroutine(CounterCo(other.GetComponent<PlayerController>().burgerStack.Pop(), foodPos[burgerCount]));
                    burgerCount++;
                    other.GetComponent<PlayerController>().burgerCount--;
                }
            }

            if(other.GetComponent<PlayerController>().FizzCupStack.Count > 0)
            {
                if(fizzCupCount < foodPos2.Length)
                {
                    StartCoroutine(CounterCo(other.GetComponent<PlayerController>().FizzCupStack.Pop(), foodPos2[fizzCupCount]));
                    fizzCupCount++;
                    other.GetComponent<PlayerController>().fizzCount--;
                }
            }
        }
    }

    private IEnumerator CounterCo(GameObject food, Transform pos)
    {
        float time = 0.5f;
        food.GetComponent<Rigidbody>().isKinematic = false;
        food.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        while(time > 0)
        {
            time -= Time.deltaTime;
            food.transform.position = Vector3.Lerp(food.transform.position, pos.position, 0.01f);
            yield return null;
        }
        food.GetComponent<Rigidbody>().isKinematic = true;
        food.transform.parent = pos;
        food.transform.position = pos.position;
        burgerStack.Push(food);
    }
}
