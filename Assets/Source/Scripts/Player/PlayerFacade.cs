using UnityEngine;

public class PlayerFacade : MonoBehaviour
{
    [SerializeField] private PlayerController _characterController;
    [SerializeField] private PlayerSensitivityChanger _sensitivityChanger;

    public PlayerSensitivityChanger SensitivityChanger => _sensitivityChanger;
}
