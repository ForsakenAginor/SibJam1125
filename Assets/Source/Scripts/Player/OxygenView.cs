using Assets.Source.Scripts.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OxygenView : MonoBehaviour
{
    [SerializeField] private Slider _bar;
    [SerializeField] private TMP_Text _text;

    private IResource _oxygen;

    [Inject]
    public void Construct(PlayerOxygenManager player)
    {
        _oxygen = player.Oxigen;
    }

    private void Start()
    {
        _bar.minValue = 0;
        _bar.maxValue = _oxygen.Maximum;
        OnOxygenChanged();

        _oxygen.ResourcesAmountChanged += OnOxygenChanged;
    }

    private void OnDestroy()
    {
        _oxygen.ResourcesAmountChanged -= OnOxygenChanged;
    }

    private void OnOxygenChanged()
    {
        _bar.value = _oxygen.Amount;
        _text.text = _oxygen.Percent.ToString("P0");
    }
}
