using UnityEngine;

public class PlayerMask : MonoBehaviour
{
    [SerializeField] private Light light;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            light.enabled = !light.enabled;
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
