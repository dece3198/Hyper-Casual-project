using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
   None, Single, Double, Set
}

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public PlayerState state;
    public Transform[] FoodPos;
    public Transform[] FoodPos2;
    public Transform[] FoodPos3;
    public Stack<GameObject> burgerStack = new Stack<GameObject>();
    public int burgerCount = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(burgerStack.Count <= 0)
        {
            animator.SetBool("Hold", false);
        }
    }

    public void EnterFoodPool(GameObject food)
    {
        if(burgerStack.Count > 0)
        {

        }
        burgerStack.Push(food);
    }

    public void DoubleFoodMood()
    {

    }
}
