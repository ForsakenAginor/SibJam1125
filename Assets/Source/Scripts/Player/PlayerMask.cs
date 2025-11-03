using Assets.Source.Scripts.Utility;
using UnityEngine;

public class PlayerMask : MonoBehaviour
{
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

        flashlight.IsRecharge = false;
        if (Input.GetKey(KeyCode.R))
        {
            flashlight.IsRecharge = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DirtPainter.Instance.StartClean();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //DirtPainter.Instance.FillAll_Test();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            DirtPainter.Instance.AddComplexStains();
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

        DirtPainter.Instance.lightKoef = flashlight.IntensityKoef;
    }
}
