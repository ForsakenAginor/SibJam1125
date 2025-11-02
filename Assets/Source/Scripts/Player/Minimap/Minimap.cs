using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform _arrow;
    [SerializeField] private Transform _minimapCamera;

    private void Update()
    {
        Vector3 position = new Vector3(_arrow.position.x, 10, _arrow.position.z);
        _minimapCamera.position = position;
    }
}
