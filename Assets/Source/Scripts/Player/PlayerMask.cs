using Assets.Source.Scripts.Utility;
using UnityEngine;

public class PlayerMask : MonoBehaviour
{
    [SerializeField] private SwitchableElement light;

    private bool _isOn = false;

    private void Start()
    {
        _isOn = light.gameObject.activeSelf;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isOn)
            {
                _isOn = false;
                light.Disable();
            }
            else
            {
                _isOn = true;
                light.Enable();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DirtPainter.Instance.StartClean();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            DirtPainter.Instance.FillAll_Test();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            DirtPainter.Instance.FillAll();
        }
    }
}
