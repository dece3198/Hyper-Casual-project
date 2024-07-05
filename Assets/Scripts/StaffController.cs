using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StaffState
{
    Idle, Walk, Sleep
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
        staff.ChangeState(StaffState.Walk);

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
            if (rand < 50)
            {
                staff.agent.SetDestination(BurgerGenerator.instance.transform.position);
            }
            else
            {
                staff.agent.SetDestination(FizzCupGenerator.instance.transform.position);
            }
        }
        else
        {
            staff.agent.SetDestination(BurgerGenerator.instance.transform.position);
        }
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
    public StaffState staffState;
    public List<Transform> foodPos = new List<Transform>();
    [SerializeField] private List<Transform> upFoodPos = new List<Transform>();
    public Stack<GameObject> foodStack = new Stack<GameObject>();
    private StateMachine<StaffState, StaffController> StateMachine = new StateMachine<StaffState, StaffController>();
    public int foodCount;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StateMachine.Reset(this);
        StateMachine.AddState(StaffState.Idle,new StaffIdle());
        StateMachine.AddState(StaffState.Walk, new StaffWalk());
        StateMachine.AddState(StaffState.Sleep, new StaffSleep());
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
}
