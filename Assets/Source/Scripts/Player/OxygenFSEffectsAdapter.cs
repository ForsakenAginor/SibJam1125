using Assets.Source.Scripts.DI.Services.Game;
using Assets.Source.Scripts.Utility;

public class OxygenFSEffectsAdapter
{
    private readonly INoiseVignetteEffect _noiseEffect;
    private readonly IColorizationFSEffect _colorizationEffect;
    private readonly IResource _oxygen;

    public OxygenFSEffectsAdapter(INoiseVignetteEffect noiseEffect, IColorizationFSEffect colorizationFSEffect, IResource oxygen)
    {
        _noiseEffect = noiseEffect;
        _colorizationEffect = colorizationFSEffect;
        _oxygen = oxygen;

        _oxygen.ResourcesAmountChanged += OnOxyChanged;
    }

    ~OxygenFSEffectsAdapter()
    {
        _oxygen.ResourcesAmountChanged -= OnOxyChanged;
    }

    private void OnOxyChanged()
    {
        _noiseEffect.SetEffectStrength(_oxygen.Percent);
        _colorizationEffect.SetStrength(_oxygen.Percent);
    }
}