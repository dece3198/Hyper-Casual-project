using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum GuestState
{
    Idle, Walk, Order, Specify, Sit, Out
}

public abstract class BaseState<T>
{
    public abstract void Enter(T guest);
    public abstract void Update(T guest);
    public abstract void Exit(T guest); 
}

public class GuestIdle : BaseState<GuestController>
{
    public override void Enter(GuestController guest)
    {
        guest.animator.SetBool("Walk", false);
    }

    public override void Exit(GuestController guest)
    {
    }

    public override void Update(GuestController guest)
    {
    }
}

public class GuestWalk : BaseState<GuestController>
{
    public override void Enter(GuestController guest)
    {
        guest.animator.SetBool("Walk", true);
    }

    public override void Exit(GuestController guest)
    {
    }

    public override void Update(GuestController guest)
    {
    }
}

public class GuestOrder : BaseState<GuestController>
{
    int maxRand;
    public override void Enter(GuestController guest)
    {
        guest.animator.SetBool("Walk", false);
        guest.animator.SetBool("Hold", true);
        guest.canvas.gameObject.SetActive(true);
        maxRand = 3 + (GameManager.instance.Level / 5);
        if(maxRand > 12)
        {
            maxRand = 12;
        }
        guest.burgerRand = Random.Range(1, maxRand);
        if(GameManager.instance.isFizzCupMachine)
        {
            guest.fizzCupImage.SetActive(true);
            guest.fizzCupRand = Random.Range(1, maxRand);
        }
        guest.tray.SetActive(true);
    }

    public override void Exit(GuestController guest)
    {
    }

    public override void Update(GuestController guest)
    {
        guest.burgerText.text = guest.burgerRand.ToString();
        guest.fizzCupText.text = guest.fizzCupRand.ToString();
        if(guest.burgerRand <= 0 && guest.fizzCupRand <= 0)
        {
            guest.ChangeState(GuestState.Specify);
        }
    }
}


public class GuestSpecify : BaseState<GuestController>
{
    public override void Enter(GuestController guest)
    {
        guest.animator.SetBool("Walk", true);
        WaitingManager.instance.waitingCount--;
        guest.canvas.gameObject.SetActive(false);
        ChairManager.instance.Specify(guest);
    }

    public override void Exit(GuestController guest)
    {
    }

    public override void Update(GuestController guest)
    {
    }
}

public class GuestSit : BaseState<GuestController>
{
    public override void Enter(GuestController guest)
    {
        guest.animator.Play("Sit_A");
        guest.animator.SetBool("Hold", false);
        guest.tray.SetActive(false);
    }

    public override void Exit(GuestController guest)
    {
    }

    public override void Update(GuestController guest)
    {
    }
}

public class GuestOut : BaseState<GuestController>
{
    public override void Enter(GuestController guest)
    {
        guest.animator.Play("Idle");
        guest.animator.SetBool("Walk", true);
        guest.agent.SetDestination(WaitingManager.instance.gameObject.transform.position);
    }

    public override void Exit(GuestController guest)
    {
    }

    public override void Update(GuestController guest)
    {
    }
}

public class GuestController : MonoBehaviour
{
    public Animator animator;
    public GuestState guestState;
    public NavMeshAgent agent;
    public Canvas canvas;
    public Transform[] burgerPos;
    public Transform[] fizzCupPos;
    public StateMachine<GuestState, GuestController> stateMachine = new StateMachine<GuestState, GuestController>();
    public Stack<GameObject> burgerStack = new Stack<GameObject>();
    public Stack<GameObject> fizzCupStack = new Stack<GameObject>();
    public TextMeshProUGUI burgerText;
    public TextMeshProUGUI fizzCupText;
    public GameObject tray;
    public GameObject fizzCupImage;
    public int burgerRand;
    public int fizzCupRand;
    public int burgerCount = 0;
    public int fizzCupCount = 0;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Reset(this);
        stateMachine.AddState(GuestState.Idle, new GuestIdle());
        stateMachine.AddState(GuestState.Walk, new GuestWalk());
        stateMachine.AddState(GuestState.Order, new GuestOrder());
        stateMachine.AddState(GuestState.Specify, new GuestSpecify());
        stateMachine.AddState(GuestState.Sit, new GuestSit());
        stateMachine.AddState(GuestState.Out, new GuestOut());
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(GuestState state)
    {
        guestState = state;
        stateMachine.ChanageState(state);
    }
}
