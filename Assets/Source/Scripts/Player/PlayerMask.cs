using Assets.Source.Scripts.DI.Services.Game;
using Assets.Source.Scripts.Utility;
using System;
using UnityEngine;
using Zenject;

public class PlayerMask : MonoBehaviour
{
    [SerializeField] private Flashlight flashlight;

    private IPlayerInput _input;
    private AudioPlayer _player;

    [Inject]
    public void Construct(IPlayerInput input, AudioPlayer audioPlayer)
    {
        _input = input;
        _player = audioPlayer;

        _input.OnClean += OnClean;
    }

    private void OnDestroy()
    {
        _input.OnClean -= OnClean;
    }

    private void OnClean()
    {
        _player.PlayCleanScreen();
        DirtPainter.Instance.StartClean();
    }

    private void Update()
    {
        flashlight.IsRecharge = false;

        if(_input.IsCharging)
            flashlight.IsRecharge = true;



#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            DirtPainter.Instance.AddComplexStains();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            flashlight.StrongFade();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            DirtPainter.Instance.FillAll();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
#endif

        DirtPainter.Instance.lightKoef = flashlight.IntensityKoef;
    }
}
