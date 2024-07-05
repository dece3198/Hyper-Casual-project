using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public List<Transform> FoodPos = new List<Transform>();
    public List<Transform> FoodPos2 = new List<Transform>();
    public List<Transform> FoodPos3 = new List<Transform>();
    public Stack<GameObject> burgerStack = new Stack<GameObject>();
    public Stack<GameObject> fizzCupStack = new Stack<GameObject>();
    [SerializeField] private GameObject tray;
    public int burgerCount = 0;
    public int fizzCount = 0;
    public List<Transform> foodList = new List<Transform>();
    public List<Transform> foodList2 = new List<Transform>();
    public List<Transform> foodList3 = new List<Transform>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(burgerStack.Count > 0 || fizzCupStack.Count > 0)
        {
            animator.SetBool("Hold", true);
        }
        else
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

        if(Input.GetKeyDown(KeyCode.L))
        {
            UpGrade();
        }
    }

    public void UpGrade()
    {
        FoodPos.Add(foodList[0]);
        foodList.Remove(foodList[0]);
        FoodPos.Add(foodList2[0]);
        foodList2.Remove(foodList2[0]);
    }
}
