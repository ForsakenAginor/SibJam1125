using Assets.Source.Scripts.DI.Services.Global;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackState : State
{
    private readonly SpiderNavigator _navigator;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly PlayerDetector _playerDetector;

    private Coroutine _coroutine;

    public AttackState(ICoroutineRunner coroutineRunner, SpiderNavigator navigator, PlayerDetector detector,
        Transition[] transitions) : base(transitions)
    {
        _navigator = navigator;
        _coroutineRunner = coroutineRunner;
        _playerDetector = detector;
    }

    ~AttackState()
    {
        Unsubscribe();
    }

    public override void DoThing()
    {
        _playerDetector.Scared += OnScared;
        _coroutine = _coroutineRunner.StartCoroutine(Attack());
    }

    private void OnScared()
    {
        Unsubscribe();
        Transitions.First(o => o is ToFleeTransition).SetIsReady(true);
        CallBecomeReadyToTransit();
    }

    private void Unsubscribe()
    {
        _playerDetector.Scared -= OnScared;
        _coroutineRunner.StopCoroutine(_coroutine);
    }

    private IEnumerator Attack()
    {
        bool isWorking = true;

        while (isWorking)
        {
            _navigator.MoveToPlayer();
            yield return new WaitUntil(() => _navigator.AtAttackDistance());

            if (_navigator.CanSeePlayer())
            {
                _playerDetector.Scared -= OnScared;
                yield return _navigator.Attack();
                isWorking = false;
                Transitions.First(o => o is ToFleeTransition).SetIsReady(true);
                CallBecomeReadyToTransit();
            }
        }
    }
}


