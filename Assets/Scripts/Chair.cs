using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public GuestController guest;
    [SerializeField] private Transform[] burgerPos;
    [SerializeField] private Transform[] fizzCupPos;
    private Stack<GameObject> burgerStack = new Stack<GameObject>();
    private Stack<GameObject> fizzCupStack = new Stack<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GuestController>() != null)
        {
            if(guest == other.GetComponent<GuestController>())
            {
                guest.ChangeState(GuestState.Sit);
                guest.transform.rotation = transform.rotation;
                guest.transform.position = transform.position;
                for(int i = 0; i < guest.burgerCount; i++)
                {
                    GameObject burger = guest.burgerStack.Pop();
                    burgerStack.Push(burger);
                    burger.transform.parent = burgerPos[i];
                    burger.transform.position = burgerPos[i].position;

                }

                for(int j = 0; j < guest.fizzCupCount; j++)
                {
                    GameObject fizzCup = guest.fizzCupStack.Pop();
                    fizzCupStack.Push(fizzCup);
                    fizzCup.transform.parent = fizzCupPos[j];
                    fizzCup.transform.position = fizzCupPos[j].position;
                }

                StartCoroutine(BurgerEnterCo());
                StartCoroutine(FizzCupEnterCo());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<GuestController>() != null)
        {
            if(burgerStack.Count <= 0 && fizzCupStack.Count <= 0)
            {
                if(guest != null)
                {
                    guest.ChangeState(GuestState.Out);
                    guest = null;
                    ChairManager.instance.chairCount--;
                }
            }
        }
    }

    private IEnumerator BurgerEnterCo()
    {
        while(burgerStack.Count > 0)
        {
            yield return new WaitForSeconds(20);
            GameObject burger = burgerStack.Pop();
            guest.burgerCount--;
            BurgerGenerator.instance.EnterPool(burger);
        }
    }

    private IEnumerator FizzCupEnterCo()
    {
        while(fizzCupStack.Count > 0)
        {
            yield return new WaitForSeconds(20);
            GameObject fizzCup = fizzCupStack.Pop();
            guest.fizzCupCount--;
            FizzCupGenerator.instance.EnterPool(fizzCup);
        }
    }
}
