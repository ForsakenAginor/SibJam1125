using Assets.Source.Scripts.Utility;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private SwitchableElement _settingsPanel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _openButton;
    [SerializeField] private Slider _sensXSlider;
    [SerializeField] private Slider _sensYSlider;

    private IInputStateManager _input;
    private PlayerSensitivityChanger _sensitivityChanger;

    private bool _isOpen = false;

    [Inject]
    public void Construct(IInputStateManager input, PlayerSensitivityChanger sensitivityChanger)
    {
        _input = input;
        _sensitivityChanger = sensitivityChanger;

        _input.EscPerformed += OnEscPressed;
        _closeButton.onClick.AddListener(Close);
        _openButton.onClick.AddListener(Open);

        _sensXSlider.onValueChanged.AddListener(OnSensXChanged);
        _sensYSlider.onValueChanged.AddListener(OnSensYChanged);
    }

    private void OnDestroy()
    {
        _input.EscPerformed -= OnEscPressed;
        _closeButton.onClick.RemoveListener(Close);
        _openButton.onClick.RemoveListener(Open);

        _sensXSlider.onValueChanged.RemoveListener(OnSensXChanged);
        _sensYSlider.onValueChanged.RemoveListener(OnSensYChanged);
    }

    private void OnSensXChanged(float value)
    {
        _sensitivityChanger.SetXSens(value);
    }

    private void OnSensYChanged(float value)
    {
        _sensitivityChanger.SetYSens(value);
    }

    private void OnEscPressed()
    {
        if(_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        _settingsPanel.Enable();
        Time.timeScale = 0f;
        _input.ToMenuState();
        _isOpen = true;
    }

    private void Close()
    {
        _settingsPanel.Disable();
        Time.timeScale = 1.0f;
        _input.ToWorldState();
        _isOpen = false;
    }
}
