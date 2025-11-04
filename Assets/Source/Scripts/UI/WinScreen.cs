using Assets.Source.Scripts.DI.Services.Global;
using Assets.Source.Scripts.Utility;
using UnityEngine;
using Zenject;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private SwitchableElement _endgameScreen;
    [SerializeField] private WinTrigger _winTrigger;

    private IInputStateManager _input;
    private IMusicPlayer _player;

    [Inject]
    public void Construct(IInputStateManager input, IMusicPlayer musicPlayer)
    {
        _input = input;
        _player = musicPlayer;
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
        _player.PlayBossFightMusic();
        Time.timeScale = 0f;
        _endgameScreen.Enable();
        _input.ToFinishState();
    }
}
