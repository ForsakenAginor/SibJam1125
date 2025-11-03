using Assets.Source.Scripts.DI.Services.Global;
using System.Collections;
using System.Linq;
using UnityEngine;

public class FleeState : State
{
    private readonly SpiderNavigator _navigator;
    private readonly ICoroutineRunner _coroutineRunner;

    private Coroutine _coroutine;

    public FleeState(ICoroutineRunner coroutineRunner, SpiderNavigator navigator,
        Transition[] transitions) : base(transitions)
    {
        _navigator = navigator;
        _coroutineRunner = coroutineRunner;
    }

    ~FleeState()
    {
        _coroutineRunner.StopCoroutine(_coroutine);
    }

    public override void DoThing()
    {
        _coroutine = _coroutineRunner.StartCoroutine(Flee());
    }

    private IEnumerator Flee()
    {
        _navigator.Flee();
        yield return new WaitUntil(() => _navigator.IsCloseToPoint());
        Transitions.First(o => o is ToPatrollTransition).SetIsReady(true);
        CallBecomeReadyToTransit();
    }
}


