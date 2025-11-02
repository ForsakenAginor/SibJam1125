using Assets.Source.Scripts.Utility;
using UnityEngine;

public class PlayerMask : MonoBehaviour
{
    [SerializeField] private SwitchableElement light;
    [SerializeField] private Flashlight flashlight;

    private bool _isOn = false;

    private void Start()
    {
        _isOn = flashlight.gameObject.activeSelf;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isOn)
            {
                _isOn = false;
                flashlight.Disable();
            }
            else
            {
                _isOn = true;
                flashlight.Enable();
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
