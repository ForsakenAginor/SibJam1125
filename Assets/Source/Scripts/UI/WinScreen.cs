using Assets.Source.Scripts.Utility;
using UnityEngine;
using Zenject;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private SwitchableElement _endgameScreen;
    [SerializeField] private WinTrigger _winTrigger;

    private IInputStateManager _input;

    [Inject]
    public void Construct(IInputStateManager input)
    {
        _input = input;
    }

    private void Awake()
    {
        _winTrigger.PlayerWon += OnPlayerWon;
    }

    private void OnDestroy()
    {
        _winTrigger.PlayerWon -= OnPlayerWon;        
    }

    private void OnPlayerWon()
    {
        Time.timeScale = 0f;
        _endgameScreen.Enable();
        _input.ToFinishState();
    }
}
