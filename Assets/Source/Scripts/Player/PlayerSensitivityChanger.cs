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
