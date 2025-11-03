using Assets.Source.Scripts.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FlashlightView : MonoBehaviour
{
    [SerializeField] private Slider _bar;
    [SerializeField] private TMP_Text _text;

    private Flashlight _flashlight;

    [Inject]
    public void Construct(PlayerFacade player)
    {
        _flashlight = player.Flashlight;
    }

    private void Start()
    {
        _bar.minValue = 0;
        _bar.maxValue = 1;
        OnChanged();
    }

    private void Update()
    {
        OnChanged();
    }

    private void OnChanged()
    {
        _bar.value = _flashlight.IntensityKoef;
        _text.text = _flashlight.IntensityKoef.ToString("P0");
    }
}
