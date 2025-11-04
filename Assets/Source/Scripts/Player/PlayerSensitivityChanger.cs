using Assets.Source.Scripts.SaveSystem;
using UnityEngine;

public class PlayerSensitivityChanger : MonoBehaviour
{
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    [SerializeField] private PlayerController _controller;

    public void SetXSens(float x)
    {
        float target = Mathf.Lerp(_minX, _maxX, x);

        _controller.SetXSens(target);
    }

    public void SetYSens(float y)
    {
        float target = Mathf.Lerp(_minY, _maxY, y);

        _controller.SetYSens(target);
    }
}

public class SensitivitySaveLoadService : IDataSaveLoadService
{
    private readonly InGameMenu _menu;
    private SaveData _saveData;
    private bool _isInited;
    private bool _isLoaded;

    public SensitivitySaveLoadService(InGameMenu menu)
    {
        _menu = menu;
    }

    public bool IsLoaded => _isInited;

    public bool IsInited => _isLoaded;

    public void Init(SaveData saveData, IDataSaveLoadService[] dependentSystems = null)
    {
        _saveData = saveData;
        _isInited = true;
    }

    public void Load()
    {
        if (_saveData.SensitivitySettings != null)
            _menu.SetSens(_saveData.SensitivitySettings.X, _saveData.SensitivitySettings.Y);
        else
            _menu.SetSens(0.2f, 0.4f);

        _isLoaded = true;
    }

    public void Save()
    {
        SensitivitySettings settings = new SensitivitySettings();
        settings.X = _menu.XSens;
        settings.Y = _menu.YSens;
        _saveData.SensitivitySettings = settings;
    }
}
