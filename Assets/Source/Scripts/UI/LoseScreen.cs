using Assets.Source.Scripts.Utility;
using UnityEngine;
using Zenject;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private SwitchableElement _endgameScreen;
    [SerializeField] private SwitchableElement _buttonCanvas;

    private IResource _oxygen;
    private IInputStateManager _input;

    [Inject]
    public void Costruct(PlayerFacade player, IInputStateManager input)
    {
        _oxygen = player.Oxygen.Oxigen;
        _input = input;

        _oxygen.ResourceOver += OnOxygenEnd;
    }

    private void OnDestroy()
    {
        _oxygen.ResourceOver -= OnOxygenEnd;
    }

    private void OnOxygenEnd()
    {
        _buttonCanvas.Disable();
        Time.timeScale = 0f;
        _input.ToFinishState();
        _endgameScreen.Enable();
    }
}
