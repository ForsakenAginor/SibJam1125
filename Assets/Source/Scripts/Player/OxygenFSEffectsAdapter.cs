using Assets.Source.Scripts.DI.Services.Game;
using Assets.Source.Scripts.Utility;
using UnityEngine;

public class OxygenFSEffectsAdapter
{
    private const float MaxVolume = 0.5f;
    private const int MinVolume = 0;

    private readonly INoiseVignetteEffect _noiseEffect;
    private readonly IColorizationFSEffect _colorizationEffect;
    private readonly IResource _oxygen;
    private readonly AudioPlayer _audioPlayer;

    public OxygenFSEffectsAdapter(INoiseVignetteEffect noiseEffect, IColorizationFSEffect colorizationFSEffect, IResource oxygen, AudioPlayer audioPlayer)
    {
        _noiseEffect = noiseEffect;
        _colorizationEffect = colorizationFSEffect;
        _oxygen = oxygen;
        _audioPlayer = audioPlayer;

        _colorizationEffect.Enable();
        OnOxyChanged();

        _oxygen.ResourcesAmountChanged += OnOxyChanged;
    }

    ~OxygenFSEffectsAdapter()
    {
        _oxygen.ResourcesAmountChanged -= OnOxyChanged;
    }

    private void OnOxyChanged()
    {
        //_noiseEffect.SetEffectStrength(_oxygen.Percent);
        _colorizationEffect.SetStrength(_oxygen.Percent);

        if(_oxygen.Percent == 0)
        {
            _audioPlayer.PlayHeartbeat(0);
        }
        else if(_oxygen.Percent < 0.5f)
        {
            float remapedValue = Mathf.Lerp(_oxygen.Percent, MinVolume, MaxVolume);
            _audioPlayer.PlayHeartbeat(1 - remapedValue);
        }
        else
        {
            _audioPlayer.PlayHeartbeat(0);
        }

    }
}