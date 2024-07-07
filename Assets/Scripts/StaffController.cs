using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;

public enum StaffState
{
    Idle, Walk, Sleep, Counter
}

public class StaffIdle : BaseState<StaffController>
{
    public override void Enter(StaffController staff)
    {
        staff.StartCoroutine(RandomCo(staff));
        staff.animator.SetBool("Run", false);
    }

    public override void Exit(StaffController staff)
    {

    }

    public override void Update(StaffController staff)
    {
        
    }
    private IEnumerator RandomCo(StaffController staff)
    {
        int rand = Random.Range(10, 20);
        yield return new WaitForSeconds(rand);
        if(staff.stamina <= 0)
        {
            staff.ChangeState(StaffState.Sleep);
        }
        else
        {
            staff.ChangeState(StaffState.Walk);
        }
    }
}

public class StaffWalk : BaseState<StaffController>
{
    int rand;
    public override void Enter(StaffController staff)
    {
        rand = Random.Range(0, 100);
        staff.animator.SetBool("Run", true);

        if (GameManager.instance.isFizzCupMachine)
        {
            if(Counter.instance.counter == null)
            {
                staff.ChangeState(StaffState.Counter);
            }
            else
            {
                if(CounterManager.instance.burgerStack.Count < CounterManager.instance.foodPos.Length)
                {
                    if (rand < 50)
                    {
                        staff.target = BurgerGenerator.instance.gameObject;
                    }
                    else
                    {
                        staff.target = FizzCupGenerator.instance.gameObject;
                    }
                }
                else
                {
                    staff.target = FizzCupGenerator.instance.gameObject;
                }    
            }
        }
        else
        {
            if (Counter.instance.counter == null)
            {
                staff.ChangeState(StaffState.Counter);
            }
            else
            {
                staff.target = BurgerGenerator.instance.gameObject;
            }
        }

        staff.agent.SetDestination(staff.target.transform.position);
    }

    public override void Exit(StaffController staff)
    {

    }

    public override void Update(StaffController staff)
    {
    }
}

public class StaffSleep : BaseState<StaffController>
{
    public override void Enter(StaffController staff)
    {
        staff.StartCoroutine(SleepCo(staff));
    }

    public override void Exit(StaffController staff)
    {

    }

    public override void Update(StaffController staff)
    {

    }

    private IEnumerator SleepCo(StaffController staff)
    {
        yield return new WaitForSeconds(5f);
        staff.stamina = 100;
        staff.ChangeState(StaffState.Idle);
    }
}

public class StaffCounter : BaseState<StaffController>
{
    public override void Enter(StaffController staff)
    {
        staff.animator.SetBool("Run", true);
        staff.target = Counter.instance.counterPos;
        Counter.instance.counter = staff.gameObject;
        staff.agent.SetDestination(staff.target.transform.position);
    }

    public override void Exit(StaffController staff)
    {

    }

    public override void Update(StaffController staff)
    {

    }

}

public class StaffController : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject target;
    public StaffState staffState;
    public List<Transform> foodPos = new List<Transform>();
    [SerializeField] private List<Transform> upFoodPos = new List<Transform>();
    public Stack<GameObject> foodStack = new Stack<GameObject>();
    private StateMachine<StaffState, StaffController> StateMachine = new StateMachine<StaffState, StaffController>();
    public int stamina;
    public int foodCount;
    public bool isWalk = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StateMachine.Reset(this);
        StateMachine.AddState(StaffState.Idle,new StaffIdle());
        StateMachine.AddState(StaffState.Walk, new StaffWalk());
        StateMachine.AddState(StaffState.Sleep, new StaffSleep());
        StateMachine.AddState(StaffState.Counter, new StaffCounter());
        ChangeState(StaffState.Idle);
    }

    private void OnEnable()
    {
        ChangeState(StaffState.Idle);
    }

    private void Update()
    {
        if(foodStack.Count > 0)
        {
            animator.SetBool("Hold", true);
        }
        else
        {
            animator.SetBool("Hold", false);
        }
    }

    public void ChangeState(StaffState state)
    {
        staffState = state;
        StateMachine.ChanageState(state);
    }

    public void VolumeUp()
    {
        foodPos.Add(upFoodPos[0]);
        upFoodPos.RemoveAt(0);
    }
}
