public class SpiderStateMachine
{
    private State _state;

    public SpiderStateMachine(State state)
    {
        _state = state != null ? state : throw new System.ArgumentNullException(nameof(state));
        _state.BecomeReadyToTransit += OnStateBecomeReadyToTransit;
        _state.DoThing();
    }

    private void SetState(Transition transition)
    {
        _state.BecomeReadyToTransit -= OnStateBecomeReadyToTransit;
        transition.SetIsReady(false);
        _state = transition.TargetState;
        _state.BecomeReadyToTransit += OnStateBecomeReadyToTransit;
        _state.DoThing();
    }

    private void OnStateBecomeReadyToTransit()
    {
        foreach (Transition transition in _state.Transitions)
            if (transition.IsReadyToTransit)
                SetState(transition);
    }
}


