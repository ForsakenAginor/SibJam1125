using Assets.Source.Scripts.DI.Services.Global;
using Zenject;

public class SpiderStateMachineFactory
{
    private readonly ICoroutineRunner _coroutineRunner;

    [Inject]
    public SpiderStateMachineFactory(ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
    }

    public SpiderStateMachine CreateStateMachine(SpiderNavigator navigator, PlayerDetector playerDetector)
    {
        //transitions
        ToPatrollTransition toPatroll = new ToPatrollTransition();
        ToFleeTransition toFlee = new ToFleeTransition();
        ToAttackTransition toAttack = new ToAttackTransition();

        //states
        PatrollState patrollState = new PatrollState(_coroutineRunner, navigator, playerDetector,
            new Transition[]
            {
                toFlee, toAttack
            });
        FleeState fleeState = new FleeState(_coroutineRunner, navigator,
            new Transition[]
            {
                toPatroll
            });

        AttackState attackState = new AttackState(_coroutineRunner, navigator, playerDetector,
            new Transition[]
            {
                 toFlee
            });

        //transitions initialize
        toPatroll.SetTargetState(patrollState);
        toFlee.SetTargetState(fleeState);
        toAttack.SetTargetState(attackState);

        //create State machine
        SpiderStateMachine machine = new SpiderStateMachine(patrollState);
        return machine;
    }
}


