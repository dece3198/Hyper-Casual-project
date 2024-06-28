using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Transform[] FoodPos;
    public Transform[] FoodPos2;
    public Transform[] FoodPos3;
    public Stack<GameObject> burgerStack = new Stack<GameObject>();
    public Stack<GameObject> FizzCupStack = new Stack<GameObject>();
    [SerializeField] private GameObject tray;
    public int burgerCount = 0;
    public int fizzCount = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //Time.timeScale = 2;
    }

    private void Update()
    {
        if(burgerStack.Count <= 0)
        {
            animator.SetBool("Hold", false);
        }

        if(burgerCount > 0 && fizzCount > 0)
        {
            tray.SetActive(true);
        }
        else
        {
            tray.SetActive(false);
        }
    }
}
